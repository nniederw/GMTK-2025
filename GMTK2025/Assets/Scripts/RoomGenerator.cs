using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomGenerator : MonoBehaviour
{
    private static RoomGenerator Instance;
    [SerializeField] private List<Room> Rooms = new List<Room>();
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private List<Item> SpawnableItems;
    private const float MinEnemyDistanceToPlayer = 6f;
    private const float CommonItemProbability = .6f;
    private const float RareItemProbability = .5f;
    private const float EpicItemProbability = .4f;
    private const float LegendaryItemProbability = .1f;
    private const float RecoverLastDeathRoom = .25f;
    private static Scene? OldRoom = null;
    private static int LoadRoom = -1; //Load in frame where LoadRoom == 0, otherwise -- unless already -1
    private static List<(uint health, uint damage)> NextEnemiesStats;
    private static List<Vector2> NextEnemiesPositions;
    private static List<List<Item>> NextEnemiesDrops;
    private static Transform PlayerTransform;
    private static List<(Vector2 pos, Item item)> LastDeathItemsToSpawn = new List<(Vector2 pos, Item item)>();
    public static void GenerateRoom(Transform playerTransform, uint difficulty = 1)
    {
        PlayerTransform = playerTransform;
        var roomIndex = Random.Range(0, Instance.Rooms.Count);
        Room room = Instance.Rooms[roomIndex];
        var y = room.BottomLeft.y;
        var x = (room.BottomLeft.x + room.TopRight.x) / 2f;
        PlayerTransform.position = new Vector2(x, y);
        uint maxEnemies = (difficulty + 1) / 2 + 1;
        uint enemyCount = (uint)Random.Range(1, (int)maxEnemies + 1);
        List<(uint health, uint damage)> enemies = new List<(uint health, uint damage)>((int)enemyCount);
        uint maxHealth = (difficulty + 2);
        uint maxDamage = (difficulty + 1) / 2 + 1;
        for (int i = 0; i < enemyCount; i++)
        {
            uint health = (uint)Random.Range(1, (int)maxHealth + 1);
            uint damage = (uint)Random.Range(0, (int)maxDamage);
            enemies.Add((health, damage));
        }
        List<Vector2> enemiesPos = new List<Vector2>((int)enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            enemiesPos.Add(RandomEnemyPos(PlayerTransform.position, room.BottomLeft, room.TopRight));
        }
        if (OldRoom != null)
        {
            SceneManager.UnloadSceneAsync(OldRoom.Value);
        }
        SceneManager.LoadScene(room.SceneName, LoadSceneMode.Additive);
        uint comItems = 0;
        uint rarItems = 0;
        uint epiItems = 0;
        uint legItems = 0;
        if (difficulty <= 2)
        {
            comItems = 1;
        }
        else if (difficulty <= 4)
        {
            comItems = 2;
            rarItems = 1;
        }
        else if (difficulty <= 6)
        {
            comItems = 1;
            rarItems = 2;
            epiItems = 1;
        }
        else if (difficulty <= 8)
        {
            comItems = 0;
            rarItems = 1;
            epiItems = 2;
        }
        else
        {
            comItems = 0;
            rarItems = 1;
            epiItems = 2;
            legItems = 1;
        }
        List<List<Item>> enemyDrops = new List<List<Item>>((int)enemyCount);
        for (int i = 0; i < enemyCount; i++)
        {
            enemyDrops.Add(new List<Item>(0));
        }
        FillListWithItems(enemyDrops, comItems, rarItems, epiItems, legItems);
        NextEnemiesStats = enemies;
        NextEnemiesPositions = enemiesPos;
        NextEnemiesDrops = enemyDrops;
        LoadRoom = 1;
        if (GameManager.HasLastDeathItems() && GameManager.GetLastDeathDifficulty() == difficulty && Random.Range(0f, 1f) < RecoverLastDeathRoom)
        {
            Debug.Log("Items respawned, lucky you");
            LastDeathItemsToSpawn = GameManager.GetLastDeathItems();
        }
        else
        {
            LastDeathItemsToSpawn = new List<(Vector2 pos, Item item)>();
        }
    }
    public static void LoadThroneRoom(string sceneName)
    {
        if (OldRoom != null)
        {
            SceneManager.UnloadSceneAsync(OldRoom.Value);
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        LoadRoom = 1;
    }
    private void Update()
    {
        if (LoadRoom == 0)
        {
            OldRoom = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(OldRoom.Value);
            SpawnEnemies(NextEnemiesStats, NextEnemiesPositions, NextEnemiesDrops);
            NextEnemiesStats.Clear();
            NextEnemiesPositions.Clear();
            NextEnemiesDrops.Clear();
            TakableItemsSpawner.SpawnItems(LastDeathItemsToSpawn);
        }
        if (LoadRoom >= 0)
        {
            LoadRoom--;
        }
    }
    //private static async Task WaitForUnloading()    {        await UnloadingRoom;    }
    private static Vector2 RandomEnemyPos(Vector2 PlayerPos, Vector2 bottomLeftBound, Vector2 topRightBound, uint iteration = 0)
    {
        const uint maxIterations = 100;
        float x = Random.Range(bottomLeftBound.x, topRightBound.x);
        float y = Random.Range(bottomLeftBound.y, topRightBound.y);
        Vector2 res = new Vector2(x, y);
        if ((res - PlayerPos).sqrMagnitude > MinEnemyDistanceToPlayer * MinEnemyDistanceToPlayer)
        {
            return res;
        }
        if (iteration == maxIterations)
        {
            Debug.Log($"Couldn't generate random valid enemy position in {maxIterations} attempts, returning a nonvalid position.");
            return res;
        }
        return RandomEnemyPos(PlayerPos, bottomLeftBound, topRightBound, iteration + 1);
    }
    private static void FillListWithItems(List<List<Item>> enemyDrops, uint comItems, uint rarItems, uint epiItems, uint legItems)
    {
        for (int i = 0; i < comItems; i++)
        {
            if (Random.Range(0f, 1f) < CommonItemProbability)
            {
                AddRandomItemAtRandomIndex(enemyDrops, CommonItems().ToList());
            }
        }
        for (int i = 0; i < rarItems; i++)
        {
            if (Random.Range(0f, 1f) < RareItemProbability)
            {
                AddRandomItemAtRandomIndex(enemyDrops, RareItems().ToList());
            }
        }
        for (int i = 0; i < epiItems; i++)
        {
            if (Random.Range(0f, 1f) < EpicItemProbability)
            {
                AddRandomItemAtRandomIndex(enemyDrops, EpicItems().ToList());
            }
        }
        for (int i = 0; i < legItems; i++)
        {
            if (Random.Range(0f, 1f) < LegendaryItemProbability)
            {
                AddRandomItemAtRandomIndex(enemyDrops, LegendaryItems().ToList());
            }
        }
    }
    private static void AddRandomItemAtRandomIndex(List<List<Item>> enemyDrops, List<Item> possibleItems)
    {
        if (enemyDrops.Count == 0 || possibleItems.Count == 0) { return; }
        int ind1 = Random.Range(0, enemyDrops.Count);
        int ind2 = Random.Range(0, possibleItems.Count);
        enemyDrops[ind1].Add(possibleItems[ind2]);
    }
    private static void SpawnEnemies(List<(uint health, uint damage)> stats, List<Vector2> pos, List<List<Item>> drops)
    {
        var enemies = stats.Zip(pos, (a, pos) => (a.health, a.damage, pos)).Zip(drops, (a, drops) => (a.health, a.damage, a.pos, drops));
        foreach (var enemy in enemies)
        {
            SpawnEnemy(enemy.health, enemy.damage, enemy.pos, enemy.drops);
        }
    }
    private static void SpawnEnemy(uint health, uint damage, Vector2 pos, List<Item> drops)
    {
        var obj = Instantiate(Instance.EnemyPrefab, pos, Quaternion.identity);
        EnemyAI enemyAI = obj.GetComponent<EnemyAI>();
        enemyAI.SetTarget(PlayerTransform);
        enemyAI.SetDamage(damage);
        enemyAI.SetDrops(drops);
        enemyAI.SetHealth(health);
    }
    private void Awake()
    {
        if (Rooms.Count == 0) { throw new System.Exception($"There are no Rooms assigned to {nameof(RoomGenerator)}, please assign some."); }
        if (SpawnableItems.Distinct().Count() != SpawnableItems.Count) { throw new System.Exception($"There are duplicates in the {nameof(SpawnableItems)} in {nameof(RoomGenerator)}, please remove them."); }
        Instance = this;
        OldRoom = null;
    }
    private static IEnumerable<Item> CommonItems() => Instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Common);
    private static IEnumerable<Item> RareItems() => Instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Rare);
    private static IEnumerable<Item> EpicItems() => Instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Epic);
    private static IEnumerable<Item> LegendaryItems() => Instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Legendary);

}
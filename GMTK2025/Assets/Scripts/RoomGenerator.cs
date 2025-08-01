using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RoomGenerator : MonoBehaviour
{
    private static RoomGenerator instance;
    [SerializeField] private List<Room> Rooms = new List<Room>();
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private List<Item> SpawnableItems;
    private const float MinEnemyDistanceToPlayer = 4f;
    private const float CommonItemProbability = .6f;
    private const float RareItemProbability = .5f;
    private const float EpicItemProbability = .4f;
    private const float LegendaryItemProbability = .2f;
    private static Scene? OldRoom = null;

    public static void GenerateRoom(Transform PlayerTransform, uint difficulty = 1)
    {
        var roomIndex = Random.Range(0, instance.Rooms.Count);
        Room room = instance.Rooms[roomIndex];
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
            uint damage = (uint)Random.Range(1, (int)maxDamage + 1);
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
        OldRoom = SceneManager.GetSceneByName(room.SceneName);
        SceneManager.SetActiveScene(OldRoom.Value);
        uint comItems = 0;
        uint rarItems = 0;
        uint epiItems = 0;
        uint legItems = 0;
        if (difficulty <= 2)
        {
            comItems = 2;
            rarItems = 1;
        }
        else if (difficulty <= 4)
        {
            comItems = 1;
            rarItems = 2;
            epiItems = 1;
        }
        else if (difficulty <= 6)
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
        FillListWithItems(enemyDrops, comItems, rarItems, epiItems, legItems);
        SpawnEnemies(enemies, enemiesPos, enemyDrops);
    }
    private static Vector2 RandomEnemyPos(Vector2 PlayerPos, Vector2 bottomLeftBound, Vector2 topRightBound)
    {
        float x = Random.Range(bottomLeftBound.x, topRightBound.x);
        float y = Random.Range(bottomLeftBound.y, topRightBound.y);
        Vector2 res = new Vector2(x, y);
        if ((res - PlayerPos).sqrMagnitude < MinEnemyDistanceToPlayer * MinEnemyDistanceToPlayer)
        {
            return res;
        }
        return RandomEnemyPos(PlayerPos, bottomLeftBound, topRightBound);
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
        var obj = Instantiate(instance.EnemyPrefab);
        EnemyAI enemyAI = obj.GetComponent<EnemyAI>();
        EnemyStats stats = obj.GetComponent<EnemyStats>();
        enemyAI.SetDamage(damage);
        stats.SetHealth(health);
        stats.SetDrops(drops);
    }
    private void Start()
    {
        if (Rooms.Count == 0) { throw new System.Exception($"There are no Rooms assigned to {nameof(RoomGenerator)}, please assign some."); }
        instance = this;
    }
    private static IEnumerable<Item> CommonItems() => instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Common);
    private static IEnumerable<Item> RareItems() => instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Rare);
    private static IEnumerable<Item> EpicItems() => instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Epic);
    private static IEnumerable<Item> LegendaryItems() => instance.SpawnableItems.Where(i => i.Rarity == ItemRarity.Legendary);

}
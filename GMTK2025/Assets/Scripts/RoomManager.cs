using System.Linq;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    private static RoomManager Instance;
    [SerializeField] private uint CurrentDifficulty = 0;
    private Transform PlayerTransform;
    [SerializeField] private uint ThroneroomSpawnDifficulty = 10;
    private static string ThroneroomScene = "Throneroom";
    [SerializeField] private Vector2 ThroneRoomPlayerPos;
    public static void FinishedRoom()
    {
        Instance.CurrentDifficulty++;
        if (Instance.CurrentDifficulty == Instance.ThroneroomSpawnDifficulty)
        {
            RoomGenerator.LoadThroneRoom(ThroneroomScene);
            Instance.PlayerTransform.position = Instance.ThroneRoomPlayerPos;
        }
        else
        {
            RoomGenerator.GenerateRoom(Instance.PlayerTransform, Instance.CurrentDifficulty);
        }
    }
    private void Awake()
    {
        Instance = this;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        var playerValues = PlayerTransform.GetComponent<CharacterValues>();
        playerValues.SubscibeToOnDeath(OnPlayerDeath);
    }
    private void OnPlayerDeath()
    {
        GameManager.SaveLastDeathItems(TakableItemsSpawner.GetActiveItems().Select(i => ((Vector2)i.transform.position, i.GetItem())), CurrentDifficulty);
        GameManager.RestartGame();
    }
}
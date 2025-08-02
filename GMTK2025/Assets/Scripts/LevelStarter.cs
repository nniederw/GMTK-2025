using UnityEngine;
public class LevelStarter : MonoBehaviour
{
    private void Start()
    {
        RoomManager.FinishedRoom();
        Destroy(gameObject);
    }
}
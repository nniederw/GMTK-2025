using System;
using System.Linq;
using UnityEngine;
public class RoomManager : MonoBehaviour
{
    private static RoomManager Instance;
    [SerializeField] private uint CurrentDifficulty = 0;
    private Transform PlayerTransform;
    public static void FinishedRoom()
    {
        Instance.CurrentDifficulty++;
        RoomGenerator.GenerateRoom(Instance.PlayerTransform, Instance.CurrentDifficulty);
    }
    private void Awake()
    {
        Instance = this;
        PlayerTransform = GameObject.FindGameObjectsWithTag("Player").First().transform;
    }
}
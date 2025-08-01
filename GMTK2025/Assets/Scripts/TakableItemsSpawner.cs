using System.Collections.Generic;
using UnityEngine;
public class TakableItemsSpawner : MonoBehaviour
{
    [SerializeField] private TakableItem TakableItemPrefab;
    [SerializeField] private float RandomDistance = 0.25f;
    private static TakableItemsSpawner instance;
    public static void SpawnItem(Vector2 approxSpawnPos, Item item)
    {
        var dir = Ext.RandomPointOnUnitCircle();
        var pos = approxSpawnPos + dir * instance.RandomDistance;
        var ti = Instantiate(instance.TakableItemPrefab, approxSpawnPos, Quaternion.identity);
        ti.SetItem(item);
        ti.SetTargetLocation(pos);
    }
    public static void SpawnItems(Vector2 approxSpawnPos, IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            SpawnItem(approxSpawnPos, item);
        }
    }
    private void Awake()
    {
        instance = this;
    }
}
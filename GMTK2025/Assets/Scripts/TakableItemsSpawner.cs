using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TakableItemsSpawner : MonoBehaviour
{
    [SerializeField] private TakableItem TakableItemPrefab;
    [SerializeField] private float RandomDistance = 0.25f;
    private static TakableItemsSpawner Instance;
    private static List<TakableItem> TakableItems = new List<TakableItem>();
    public static void SpawnItem(Vector2 approxSpawnPos, Item item)
    {
        var dir = Ext.RandomPointOnUnitCircle();
        var pos = approxSpawnPos + dir * Instance.RandomDistance;
        var ti = Instantiate(Instance.TakableItemPrefab, approxSpawnPos, Quaternion.identity);
        ti.SetItem(item);
        ti.SetTargetLocation(pos);
        TakableItems.Add(ti);
    }
    public static void SpawnItems(Vector2 approxSpawnPos, IEnumerable<Item> items)
    {
        foreach (var item in items)
        {
            SpawnItem(approxSpawnPos, item);
        }
    }
    public static void SpawnItems(IEnumerable<(Vector2 approxSpawnPos, Item item)> items)
    {
        foreach (var i in items)
        {
            SpawnItem(i.approxSpawnPos, i.item);
        }
    }
    public static List<TakableItem> GetActiveItems() => TakableItems.Where(i => i != null).ToList();
    private void Awake()
    {
        Instance = this;
    }
    private void FixedUpdate()
    {
        TakableItems = TakableItems.Where(i => i != null).ToList();
    }
}
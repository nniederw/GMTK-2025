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
        var ti = Instantiate(instance.TakableItemPrefab, pos, Quaternion.identity);
        ti.SetItem(item);
    }
    private void Awake()
    {
        instance = this;
    }
}
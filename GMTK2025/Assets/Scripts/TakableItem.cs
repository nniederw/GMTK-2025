using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))] //Collider to give the player an ability to pick it up.
public class TakableItem : MonoBehaviour
{
    [SerializeField] private Item Item;
    private Vector2 TargetLocation;
    private bool MovingToTarget = false;
    [SerializeField] private float TargetMovingSeconds = 0.1f;
    private SpriteRenderer Renderer;
    private HashSet<Collider2D> colliders = new HashSet<Collider2D>();
    public void SetItem(Item item)
    {
        Item = item;
        Renderer.sprite = item.Sprite;
    }
    public Item GetItem() => Item;
    public void SetTargetLocation(Vector2 target)
    {
        TargetLocation = target;
        MovingToTarget = true;
    }
    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = Item.Sprite;
    }
    private void Update()
    {
        var itemTakers = colliders.Where(i => i != null).Select(i => i.GetInterfaceComponent<ItemTaker>()).Where(i => i != null).ToList();
        foreach (var itemTaker in itemTakers)
        {
            if (itemTaker.WantsItem(Item))
            {
                itemTaker.TakeItem(Item);
                Destroy(gameObject);
                break;
            }
        }
        if (MovingToTarget)
        {
            transform.position = Vector2.Lerp(transform.position, TargetLocation, Time.deltaTime / TargetMovingSeconds);
            if (((Vector2)transform.position - TargetLocation).sqrMagnitude < 0.0001f)
            {
                MovingToTarget = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collision);
    }
}
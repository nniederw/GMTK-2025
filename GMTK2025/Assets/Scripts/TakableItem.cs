using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(SpriteRenderer))] //Collider to give the player an ability to pick it up.
public class TakableItem : MonoBehaviour
{
    [SerializeField] private Item Item;
    private SpriteRenderer Renderer;
    private HashSet<Collider2D> colliders = new HashSet<Collider2D>();
    public void SetItem(Item item)
    {
        Item = item;
        Renderer.sprite = item.Sprite;
    }
    public Item GetItem() => Item;
    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Renderer.sprite = Item.Sprite;
    }
    private void Update()
    {
        var cols = colliders.Where(i => i != null).ToList();
        foreach (var collider in cols)
        {
            var itemTaker = collider.GetInterfaceComponent<ItemTaker>();
            if (itemTaker.WantsItem())
            {
                itemTaker.TakeItem(Item);
                Destroy(gameObject);
                break;
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
    /*
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("OnTriggerStay");
        var itemTaker = other.GetInterfaceComponent<ItemTaker>();
        if (itemTaker.WantsItem())
        {
            itemTaker.TakeItem(Item);
            Destroy(gameObject);
        }
    }*/
}
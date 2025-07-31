using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;
public class PlayerValues : MonoBehaviour, IDamagable, ItemTaker
{
    [SerializeField] private uint Health = 10;
    [SerializeField] private float AttackCooldownSeconds = 1f;
    private float AttackCooldown = 0f;
    [SerializeField] private float AttackRange = 0.5f;

    [SerializeField] private Item Sword = null;
    [SerializeField] private Item Armor = null;
    [SerializeField] private Item Shield = null;
    [SerializeField] private Item Amulet = null;
    private Item TotalItemsStats;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private Image Slot1UI;
    [SerializeField] private Image Slot2UI;
    [SerializeField] private Image Slot3UI;
    [SerializeField] private Image Slot4UI;
    [SerializeField] private Sprite DefaultItemSprite;

    [SerializeField] private List<Item> StartItems = new List<Item>();
    public void RecieveDamage(uint damage)
    {
        uint newHealth = Health - DamageAfterReduction(damage);
        if (newHealth > Health)
        {
            Health = 0;
        }
    }
    private void Start()
    {
        if (InventoryUI == null) throw new Exception($"{nameof(InventoryUI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot1UI == null) throw new Exception($"{nameof(Slot1UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot2UI == null) throw new Exception($"{nameof(Slot2UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot3UI == null) throw new Exception($"{nameof(Slot3UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot4UI == null) throw new Exception($"{nameof(Slot4UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (DefaultItemSprite == null) throw new Exception($"{nameof(DefaultItemSprite)} was null in {nameof(PlayerValues)}, please assing it.");
        TotalItemsStats = ScriptableObject.CreateInstance<Item>();
        foreach (Item item in StartItems)
        {
            AddItem(item);
        }
        UpdateItems(); //In case of no start items.
    }
    private void AddItem(Item item)
    {
        Item OldItem = null;
        switch (item.Class)
        {
            case ItemClass.Sword:
                if (Sword != null)
                {
                    OldItem = Sword;
                }
                Sword = item; break;
            case ItemClass.Armor:
                if (Armor != null)
                {
                    OldItem = Armor;
                }
                Armor = item; break;
            case ItemClass.Shield:
                if (Shield != null)
                {
                    OldItem = Shield;
                }
                Shield = item; break;
            case ItemClass.Amulet:
                if (Amulet != null)
                {
                    OldItem = Amulet;
                }
                Amulet = item; break;
        }
        if (OldItem != null)
        {
            TakableItemsSpawner.SpawnItem(transform.position, OldItem);
        }
        UpdateItems();
    }
    private void UpdateItems()
    {
        var items = CurrentlyHoldingItems().ToList();
        TotalItemsStats.DamageIncrease = 0;
        TotalItemsStats.DamageIncrease = 0;
        foreach (var i in items)
        {
            TotalItemsStats.DamageIncrease += i.DamageIncrease;
            TotalItemsStats.DamageIncrease += i.DamageAbsorbtion;
        }
        Slot1UI.sprite = Sword != null ? Sword.Sprite : DefaultItemSprite;
        Slot2UI.sprite = Armor != null ? Armor.Sprite : DefaultItemSprite;
        Slot3UI.sprite = Shield != null ? Shield.Sprite : DefaultItemSprite;
        Slot4UI.sprite = Amulet != null ? Amulet.Sprite : DefaultItemSprite;
    }
    private IEnumerable<Item> CurrentlyHoldingItems()
    {
        if (Sword != null) { yield return Sword; }
        if (Shield != null) { yield return Shield; }
        if (Armor != null) { yield return Armor; }
        if (Amulet != null) { yield return Amulet; }
    }
    private uint DamageAfterReduction(uint damage)
    {
        int reducedDamage = (int)damage - (int)TotalItemsStats.DamageAbsorbtion;
        return (uint)Math.Max(reducedDamage, 0);
    }
    private void OpenInventory()
    {
        InventoryUI.SetActive(true);
    }
    private void CloseInventory()
    {
        InventoryUI.SetActive(false);
    }
    private void ToggleInventory()
    {
        if (InventoryUI.activeSelf)
        {
            CloseInventory();
            return;
        }
        OpenInventory();
    }
    private void Update()
    {
        if (Health == 0)
        {
            Debug.Log("Death!");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
    public bool WantsItem()
    {
        Debug.Log("Wants Item");
        return Input.GetKeyDown(KeyCode.E);
    }

    public void TakeItem(Item item)
    {
        AddItem(item);
    }
}
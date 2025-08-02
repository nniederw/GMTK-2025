using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerValues : MonoBehaviour, IDamagable, ItemTaker, IPlayer
{
    [SerializeField] private uint Health = 10;
    [SerializeField] private float AttackCooldownSeconds = 0.7f;
    private float AttackCooldown = 0f;
    //[SerializeField] private float AttackRange = 0.5f;
    private float SwordSwingPercentage = 1f;
    [SerializeField] private float SwordSwingClockSeconds = 0.2f;
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
    [SerializeField] private TMP_Text Slot1UIText;
    [SerializeField] private TMP_Text Slot2UIText;
    [SerializeField] private TMP_Text Slot3UIText;
    [SerializeField] private TMP_Text Slot4UIText;

    [SerializeField] private TMP_Text HeartText;

    [SerializeField] private Sprite DefaultItemSprite;

    [SerializeField] private List<Item> StartItems = new List<Item>();
    [SerializeField] private GameObject SwordPivotPoint;
    [SerializeField] private SpriteRenderer SwordSpriteRenderer;
    private SwordCollider SwordCollider;
    private event Action OnSwordAttack;
    public void RecieveDamage(uint damage, DamagableTeam source)
    {
        if (source == DamagableTeam.Player)
        {
            return;
        }
        uint newHealth = Health - DamageAfterReduction(damage);
        if (newHealth > Health)
        {
            Health = 0;
        }
        Health = newHealth;
        HeartText.text = Health.ToString();
    }
    public void OnSwordHittingCollider(Collider2D col)
    {
        //Debug.Log($"Hit collider {col.name} with sword");
        var dmab = col.GetInterfaceComponent<IDamagable>();
        if (dmab != null)
        {
            dmab.RecieveDamage(Damage(), DamagableTeam.Player);
        }
    }
    public bool WantsItem()
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    public void TakeItem(Item item)
    {
        AddItem(item);
    }
    public void SubscribeToSwordAttack(Action playSwordAttackSound)
    {
        OnSwordAttack += playSwordAttackSound;
    }
    private void Start()
    {
        if (InventoryUI == null) throw new Exception($"{nameof(InventoryUI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot1UI == null) throw new Exception($"{nameof(Slot1UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot2UI == null) throw new Exception($"{nameof(Slot2UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot3UI == null) throw new Exception($"{nameof(Slot3UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot4UI == null) throw new Exception($"{nameof(Slot4UI)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot1UIText == null) throw new Exception($"{nameof(Slot1UIText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot2UIText == null) throw new Exception($"{nameof(Slot2UIText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot3UIText == null) throw new Exception($"{nameof(Slot3UIText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (Slot4UIText == null) throw new Exception($"{nameof(Slot4UIText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (DefaultItemSprite == null) throw new Exception($"{nameof(DefaultItemSprite)} was null in {nameof(PlayerValues)}, please assing it.");
        if (HeartText == null) throw new Exception($"{nameof(HeartText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (SwordPivotPoint == null) throw new Exception($"{nameof(SwordPivotPoint)} was null in {nameof(PlayerValues)}, please assing it.");
        if (SwordSpriteRenderer == null) throw new Exception($"{nameof(SwordSpriteRenderer)} was null in {nameof(PlayerValues)}, please assing it.");

        SwordCollider = GetComponentInChildren<SwordCollider>();
        if (SwordCollider == null) throw new Exception($"{nameof(SwordCollider)} was not found in {nameof(PlayerValues)}, please make sure any child of it has an instance of {nameof(SwordCollider)}.");
        SwordCollider.AddSwordCollisionListener(OnSwordHittingCollider);
        TotalItemsStats = ScriptableObject.CreateInstance<Item>();
        foreach (Item item in StartItems)
        {
            AddItem(item);
        }
        UpdateItems(); //In case of no start items.
    }
    private void Update()
    {
        if (SwordSwingPercentage != 1f)
        {
            SwordSwingPercentage += Time.deltaTime / SwordSwingClockSeconds;
            SwordSwingPercentage = Math.Min(1f, SwordSwingPercentage);
            if (SwordSwingPercentage == 1f)
            {
                SwordCollider.DisableCollider();
            }
            SwordPivotPoint.transform.eulerAngles = new Vector3(0, 0, 360f * SwordSwingPercentage);
        }
        if (AttackCooldown > 0f)
        {
            AttackCooldown -= Time.deltaTime;
            AttackCooldown = Math.Max(0f, AttackCooldown);
        }
        if (Health == 0)
        {
            Debug.Log("Death!");
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        if (AttackCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            SwordSwingPercentage = 0f;
            SwordCollider.EnableCollider();
            AttackCooldown = AttackCooldownSeconds;
            OnSwordAttack?.Invoke();
        }
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
        Slot1UIText.text = Sword != null ? Sword.GetText() : "";
        Slot2UIText.text = Armor != null ? Armor.GetText() : "";
        Slot3UIText.text = Shield != null ? Shield.GetText() : "";
        Slot4UIText.text = Amulet != null ? Amulet.GetText() : "";
        if (Sword != null)
        {
            SwordSpriteRenderer.sprite = Sword.Sprite;
        }
    }
    private IEnumerable<Item> CurrentlyHoldingItems()
    {
        if (Sword != null) { yield return Sword; }
        if (Shield != null) { yield return Shield; }
        if (Armor != null) { yield return Armor; }
        if (Amulet != null) { yield return Amulet; }
    }
    private uint Damage()
    {
        return (uint)Math.Max(0, TotalItemsStats.DamageIncrease);
    }
    private uint DamageAfterReduction(uint damage)
    {
        int reducedDamage = (int)damage - TotalItemsStats.DamageAbsorbtion;
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


}
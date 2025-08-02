using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private TMP_Text HeartText;

    [SerializeField] private Sprite EmptySwordSprite;

    [SerializeField] private List<Item> StartItems = new List<Item>();
    [SerializeField] private GameObject SwordPivotPoint;
    [SerializeField] private SpriteRenderer SwordSpriteRenderer;
    private SwordCollider SwordCollider;
    private event Action OnSwordAttack;
    private event Action<(Item Sword, Item Armor, Item Shield, Item Amulet)> OnPotentialInventoryChange;
    public void RecieveDamage(uint damage, DamagableTeam source)
    {
        if (source == DamagableTeam.Player)
        {
            return;
        }
        uint newHealth = Health - DamageAfterReduction(damage);
        if (newHealth > Health)
        {
            newHealth = 0;
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
    public void SubscribeToOnPotentialInventoryChange(Action<(Item Sword, Item Armor, Item Shield, Item Amulet)> action)
    {
        OnPotentialInventoryChange += action;
    }
    private void Start()
    {
        if (HeartText == null) throw new Exception($"{nameof(HeartText)} was null in {nameof(PlayerValues)}, please assing it.");
        if (SwordPivotPoint == null) throw new Exception($"{nameof(SwordPivotPoint)} was null in {nameof(PlayerValues)}, please assing it.");
        if (SwordSpriteRenderer == null) throw new Exception($"{nameof(SwordSpriteRenderer)} was null in {nameof(PlayerValues)}, please assing it.");
        if (EmptySwordSprite == null) throw new Exception($"{nameof(EmptySwordSprite)} was null in {nameof(PlayerValues)}, please assing it.");
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
            DropAllItems();
        }
        if (AttackCooldown <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            SwordSwingPercentage = 0f;
            SwordCollider.EnableCollider();
            AttackCooldown = AttackCooldownSeconds;
            OnSwordAttack?.Invoke();
        }
    }
    private void DropAllItems()
    {
        var toDrop = new List<Item>();
        if (Sword != null) { toDrop.Add(Sword); Sword = null; }
        if (Shield != null) { toDrop.Add(Shield); Shield = null; }
        if (Armor != null) { toDrop.Add(Armor); Armor = null; }
        if (Amulet != null) { toDrop.Add(Amulet); Amulet = null; }
        foreach (var item in toDrop)
        {
            TakableItemsSpawner.SpawnItem(transform.position, item);
        }
        UpdateItems();
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
        OnPotentialInventoryChange?.Invoke((Sword, Armor, Shield, Amulet));
        SwordSpriteRenderer.sprite = Sword == null ? EmptySwordSprite : Sword.Sprite;
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
}
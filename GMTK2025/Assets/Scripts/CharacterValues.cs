using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CharacterValues : MonoBehaviour, IDamagable, ItemTaker, IPlayer
{
    [SerializeField] private uint Health = 10;
    [SerializeField] private uint BaseDamage = 0;
    private uint Damage => (uint)Math.Max(0, BaseDamage + TotalItemsStats.DamageIncrease);
    [SerializeField] private float BaseAttackCooldownSeconds = 0.7f;
    private int BaseArmor = 0;
    private int ArmorValue => BaseArmor + TotalItemsStats.Armor;
    private float AttackCooldownSeconds => Sword == null ? BaseAttackCooldownSeconds * AttackSpeedMultiplier : BaseAttackCooldownSeconds * AttackSpeedMultiplier * Sword.SwordLength;
    private float AttackSpeedMultiplier => 1f - TotalItemsStats.AdditionalAttackSpeedPercentage / 100f;
    private float AttackCooldown = 0f;
    private float SwordSwingPercentage = 1f;
    [SerializeField] private float SwordSwingClockSeconds = 0.2f;
    [SerializeField] private float MinBlockSeconds = 0.2f;
    private float RemainingBlock = 0f;
    [SerializeField] private Item Sword = null;
    [SerializeField] private Item Armor = null;
    [SerializeField] private Item Shield = null;
    [SerializeField] private Item Amulet = null;
    private Item TotalItemsStats;
    private List<Item> DeleteOnDeath = new List<Item>();
    [SerializeField] private Sprite EmptySprite;
    [SerializeField] private List<Item> StartItems = new List<Item>();
    [SerializeField] private GameObject SwordPivotPoint;
    [SerializeField] private SpriteRenderer SwordSpriteRenderer;
    [SerializeField] private ShieldController ShieldController;
    [SerializeField] private SpriteController ArmorController;
    [SerializeField] private SpriteController AmuletController;
    [SerializeField] private DamagableTeam Team;
    private SwordCollider SwordCollider;
    private event Action OnSwordAttack;
    private event Action<uint> OnPotentialHealthChange;
    private event Action OnDeath;
    private event Action<(Item Sword, Item Armor, Item Shield, Item Amulet)> OnPotentialInventoryChange;
    private Func<Item, bool> WantsItemCheck = i => false;
    private Func<bool> WantsToAttack = () => false;
    private Func<bool> WantsToBlock = () => false;
    public void RecieveDamage(uint damage, DamagableTeam source)
    {
        if (source == Team)
        {
            return;
        }
        uint newHealth = Health - DamageAfterReduction(damage);
        if (newHealth > Health)
        {
            newHealth = 0;
        }
        Health = newHealth;
        OnPotentialHealthChange?.Invoke(Health);
    }
    public void OnSwordHittingCollider(Collider2D col)
    {
        var dmab = col.GetInterfaceComponent<IDamagable>();
        if (dmab != null)
        {
            dmab.RecieveDamage(Damage, Team);
        }
    }
    public bool WantsItem(Item item)
        => WantsItemCheck.Invoke(item);
    public void TakeItem(Item item)
        => AddItem(item);
    public void SubscribeToSwordAttack(Action playSwordAttackSound)
        => OnSwordAttack += playSwordAttackSound;
    public void SubscribeToOnPotentialInventoryChange(Action<(Item Sword, Item Armor, Item Shield, Item Amulet)> action)
        => OnPotentialInventoryChange += action;
    public void SubscribeToOnPotentialHealthChange(Action<uint> action)
        => OnPotentialHealthChange += action;
    public void SubscibeToOnDeath(Action action)
        => OnDeath += action;
    public void SetWantsItemFunction(Func<Item, bool> func)
        => WantsItemCheck = func;
    public void SetWantsToAttackFunction(Func<bool> func)
        => WantsToAttack = func;
    public void SetWantsToBlockFunction(Func<bool> func)
        => WantsToBlock = func;
    public void SetBaseDamage(uint damage)
        => BaseDamage = damage;
    public void SetBaseHealth(uint health)
        => Health = health;
    public void SetBaseArmor(int armor)
       => BaseArmor = armor;
    public void SetDeleteOnDeathItems(IEnumerable<Item> items)
        => DeleteOnDeath = items.ToList();
    public bool HasSword()
        => Sword != null;
    public bool HasArmor()
        => Armor != null;
    public Item GetSword()
        => Sword;
    public IEnumerable<Item> AllItems()
    {
        if(Sword != null) { yield return Sword; }
        if(Shield != null) { yield return Shield; }
        if(Armor != null) { yield return Armor; }
        if(Amulet != null) { yield return Amulet; }
    }
    private void Awake()
    {
        SwordCollider = GetComponentInChildren<SwordCollider>();
        if (SwordCollider == null) throw new Exception($"{nameof(SwordCollider)} was not found in {nameof(CharacterValues)}, please make sure any child of it has an instance of {nameof(SwordCollider)}.");
        SwordCollider.AddSwordCollisionListener(OnSwordHittingCollider);
        TotalItemsStats = ScriptableObject.CreateInstance<Item>();
    }
    private void Start()
    {
        if (SwordPivotPoint == null) throw new Exception($"{nameof(SwordPivotPoint)} was null in {nameof(CharacterValues)}, please assing it.");
        if (SwordSpriteRenderer == null) throw new Exception($"{nameof(SwordSpriteRenderer)} was null in {nameof(CharacterValues)}, please assing it.");
        if (EmptySprite == null) throw new Exception($"{nameof(EmptySprite)} was null in {nameof(CharacterValues)}, please assing it.");
        if (ShieldController == null) throw new Exception($"{nameof(ShieldController)} was null in {nameof(CharacterValues)}, please assing it.");
        if (ArmorController == null) throw new Exception($"{nameof(ArmorController)} was null in {nameof(CharacterValues)}, please assing it.");
        if (AmuletController == null) throw new Exception($"{nameof(AmuletController)} was null in {nameof(CharacterValues)}, please assing it.");
        foreach (Item item in StartItems)
        {
            AddItem(item);
        }
        UpdateItems();
    }
    private void Update()
    {
        if (SwordSwinging())
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
            Debug.Log($"Death of {name}");
            DropAllItems();
            OnDeath?.Invoke();
        }
        if (CanBlock() && RemainingBlock == 0f && WantsToBlock())
        {
            ShieldController.StartBlock();
            RemainingBlock = MinBlockSeconds;
        }
        if (RemainingBlock > 0f)
        {
            RemainingBlock -= Time.deltaTime;
            if (RemainingBlock < 0f)
            {
                RemainingBlock = 0f;
                if (CanBlock() && WantsToBlock())
                {
                    RemainingBlock = MinBlockSeconds;
                }
                else
                {
                    ShieldController.StopBlock();
                }
            }
        }
        if (CanSwingSword() && WantsToAttack())
        {
            SwordSwingPercentage = 0f;
            SwordCollider.EnableCollider();
            AttackCooldown = AttackCooldownSeconds;
            OnSwordAttack?.Invoke();
        }
    }
    private bool SwordSwinging() => SwordSwingPercentage != 1f;
    private bool CanBlock() => !SwordSwinging();
    private bool Blocking() => RemainingBlock > 0f;
    private bool CanSwingSword() => !SwordSwinging() && !Blocking() && AttackCooldown <= 0f;
    private void DropAllItems()
    {
        var toDrop = new List<Item>();
        if (Sword != null) { toDrop.Add(Sword); Sword = null; }
        if (Shield != null) { toDrop.Add(Shield); Shield = null; }
        if (Armor != null) { toDrop.Add(Armor); Armor = null; }
        if (Amulet != null) { toDrop.Add(Amulet); Amulet = null; }
        foreach (var item in toDrop.Where(i => !DeleteOnDeath.Contains(i)))
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
        TotalItemsStats.Armor = 0;
        TotalItemsStats.DamageBlockage = 0;
        TotalItemsStats.AdditionalAttackSpeedPercentage = 0;
        float attackSpeedPerc = 1f;
        foreach (var i in items)
        {
            TotalItemsStats.DamageIncrease += i.DamageIncrease;
            TotalItemsStats.Armor += i.Armor;
            TotalItemsStats.DamageBlockage += i.DamageBlockage;
            attackSpeedPerc *= 1f + i.AdditionalAttackSpeedPercentage / 100f;
        }
        TotalItemsStats.AdditionalAttackSpeedPercentage = (int)Math.Round(attackSpeedPerc * 100f - 100f);
        OnPotentialInventoryChange?.Invoke((Sword, Armor, Shield, Amulet));
        SwordSpriteRenderer.sprite = Sword == null ? EmptySprite : Sword.Sprite;
        ShieldController.UpdateSprite(Shield == null ? EmptySprite : Shield.Sprite);
        ArmorController.UpdateSprite(Armor == null ? EmptySprite : Armor.Sprite);
        AmuletController.UpdateSprite(Amulet == null ? EmptySprite : Amulet.Sprite);
        SwordCollider.OnSwordChange(Sword);
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
        int reducedDamage = (int)damage - ArmorValue;
        if (Blocking())
        {
            reducedDamage -= TotalItemsStats.DamageBlockage;
        }
        return (uint)Math.Max(reducedDamage, 0);
    }
}
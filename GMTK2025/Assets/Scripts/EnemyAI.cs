using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(CharacterValues)), RequireComponent(typeof(CharacterMovement))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform Target = null;
    [SerializeField] private float ViewDistance = 7f;
    [SerializeField] private float BaseAttackDistance = 1.5f;
    private float SwordSize = 1f;
    private float AttackDistance => BaseAttackDistance * SwordSize;
    [SerializeField] private Item StartSword;
    private CharacterValues CharacterValues;
    private CharacterMovement CharacterMovement;
    private List<Item> Drops = new List<Item>();
    private List<Item> ForDrops = new List<Item>();
    private event Action OnDeath;
    public void SetTarget(Transform target)
    {
        Target = target;
    }
    public void SetDamage(uint damage)
    {
        CharacterValues.SetBaseDamage(damage);
    }
    public void SetDrops(List<Item> drops)
    {
        ForDrops = drops;
    }
    public void SetHealth(uint health)
    {
        CharacterValues.SetBaseHealth(health);
    }
    public void SubscribeToOnDeath(Action playEnemyDieSound)
    {
        OnDeath += playEnemyDieSound;
    }
    private void Die()
    {
        TakableItemsSpawner.SpawnItems(transform.position, Drops);
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
    private void Awake()
    {
        CharacterValues = GetComponent<CharacterValues>();
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterValues.SetWantsToAttackFunction(WantsToAttack);
        CharacterValues.SubscibeToOnDeath(Die);
        CharacterMovement.SetMovementDirectionFunction(MovementDirection);
    }
    private void Start()
    {
        if (StartSword == null) { throw new Exception($"{nameof(StartSword)} was null on {nameof(EnemyAI)}, please assign."); }
        if (ForDrops.Any())
        {
            List<Item> toInv = new List<Item>();
            var swordL = ForDrops.Where(i => i.Class == ItemClass.Sword).ToList();
            if (swordL.Any()) { toInv.Add(swordL.MaxOf(i => i.DamageIncrease)); }
            var armorL = ForDrops.Where(i => i.Class == ItemClass.Armor).ToList();
            if (armorL.Any()) { toInv.Add(armorL.MaxOf(i => i.Armor)); }
            var shield = ForDrops.Where(i => i.Class == ItemClass.Shield).FirstOrDefault();
            if (shield != null) { toInv.Add(shield); }
            var amulet = ForDrops.Where(i => i.Class == ItemClass.Amulet).FirstOrDefault();
            if (amulet != null) { toInv.Add(amulet); }
            Drops = ForDrops.Except(toInv).ToList();
            foreach (var item in toInv)
            {
                CharacterValues.TakeItem(item);
            }
        }
        if (!CharacterValues.HasSword())
        {
            GenerateDefaultSword();
        }
        if (CharacterValues.HasArmor())
        {
            CharacterValues.SetBaseArmor(-1);
        }
        SwordSize = CharacterValues.GetSword().SwordLength;
    }
    private void GenerateDefaultSword()
    {
        CharacterValues.TakeItem(StartSword);
        CharacterValues.SetDeleteOnDeathItems(new List<Item> { StartSword });
    }
    private Vector2 MovementDirection()
    {
        if (Target == null) { return Vector2.zero; }
        var dir = Target.position - transform.position;
        var magsqrd = dir.sqrMagnitude;
        if (magsqrd <= ViewDistance * ViewDistance)
        {
            return dir.normalized;
        }
        return Vector2.zero;
    }
    private bool WantsToAttack()
    {
        if (Target == null) { return false; }
        Vector2 dir = Target.position - transform.position;
        var magsqrd = dir.sqrMagnitude;
        return magsqrd < AttackDistance * AttackDistance;
    }
}
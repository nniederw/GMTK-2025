using System;
using System.Collections.Generic;
using UnityEngine;
public class EnemyStats : MonoBehaviour, IDamagable
{
    [SerializeField] private uint Health = 2;
    private event Action OnDeath;
    private List<Item> Drops = new List<Item>();
    public void RecieveDamage(uint damage, DamagableTeam source)
    {
        if (source == DamagableTeam.Enemy) { return; }
        var newHealth = Health - damage;
        if (newHealth > Health)
        {
            newHealth = 0;
        }
        Health = newHealth;
        if (Health == 0)
        {
            Die();
        }
    }
    public void SetHealth(uint health)
    {
        Health = health;
    }
    public void SubscribeToOnDeath(Action playEnemyDieSound)
    {
        OnDeath += playEnemyDieSound;
    }
    public void SetDrops(List<Item> drops)
    {
        Drops = drops;
    }
    private void Die()
    {
        OnDeath?.Invoke();
        TakableItemsSpawner.SpawnItems(transform.position, Drops);
        Destroy(gameObject);
    }
}
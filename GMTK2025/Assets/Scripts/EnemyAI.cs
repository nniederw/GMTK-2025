using UnityEngine;
using System;
using System.Linq;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform Target = null;
    [SerializeField] private float ViewDistance = 5f;
    [SerializeField] private float AttackDistance = 0.2f;
    [SerializeField] private float AttackCooldownSec = 1.0f;
    [SerializeField] private float MovementSpeed = 1.0f;
    [SerializeField] private uint Damage = 1;
    private Rigidbody2D rb2d;
    [SerializeField] private float AttackCooldown = 0f;
    public void SetTarget(Transform target)
    {
        Target = target;
    }
    public void SetDamage(uint damage)
    {
        Damage = damage;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {

    }
    private void FixedUpdate()
    {
        if (Target == null) { return; }

        if (AttackCooldown > 0f)
        {
            AttackCooldown -= Time.fixedDeltaTime;
            AttackCooldown = Math.Max(0f, AttackCooldown);
        }

        Vector2 dir = Target.position - transform.position;
        float magnitude = dir.magnitude;
        if (magnitude < AttackDistance)
        {
            TryAttacking();
        }
        if (magnitude <= ViewDistance)
        {
            Vector2 MovementDir = dir.normalized * MovementSpeed;
            rb2d.linearVelocity = MovementDir;
        }
    }
    private void TryAttacking()
    {
        if (AttackCooldown > 0f)
        {
            return;
        }
        AttackCooldown = AttackCooldownSec;
        var cols = Physics2D.OverlapCircleAll(transform.position, AttackDistance);
        var damagables = cols.Select(i => i.GetInterfaceComponent<IDamagable>()).Where(i => i != null);
        foreach (var damagable in damagables)
        {
            damagable.RecieveDamage(Damage, DamagableTeam.Enemy);
        }
    }
}
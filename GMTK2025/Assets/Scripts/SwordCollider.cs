using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class SwordCollider : MonoBehaviour
{
    private event Action<Collider2D> OnSwordCollision;
    private Collider2D Collider2D;
    public void AddSwordCollisionListener(Action<Collider2D> callback)
    {
        OnSwordCollision += callback;
    }
    public void DisableCollider()
    {
        Collider2D.enabled = false;
    }
    public void EnableCollider()
    {
        Collider2D.enabled = true;
    }
    private void Start()
    {
        Collider2D = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSwordCollision?.Invoke(collision);
    }
}
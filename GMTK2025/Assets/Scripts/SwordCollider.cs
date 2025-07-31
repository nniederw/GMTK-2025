using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class SwordCollider : MonoBehaviour
{
    private event Action<Collider2D> OnSwordCollision;
    public void AddSwordCollisionListener(Action<Collider2D> callback)
    {
        OnSwordCollision += callback;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSwordCollision?.Invoke(collision);
    }
}
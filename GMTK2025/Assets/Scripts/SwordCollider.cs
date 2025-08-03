using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class SwordCollider : MonoBehaviour
{
    private event Action<Collider2D> OnSwordCollision;
    private Collider2D Collider2D;
    private Transform ParentTransform;
    public void AddSwordCollisionListener(Action<Collider2D> callback)
    {
        OnSwordCollision += callback;
    }
    public void OnSwordChange(Item currentSword)
    {
        float length = currentSword == null ? 0.5f : currentSword.SwordLength;
        SetParentPosition(length);
        SetColliderLength(length);
    }
    public void DisableCollider()
    {
        Collider2D.enabled = false;
    }
    public void EnableCollider()
    {
        Collider2D.enabled = true;
    }
    private void SetParentPosition(float swordLength)
    {
        //ParentTransform.localPosition = new Vector3(0.25f, 0.25f) + new Vector3(.25f,.25f) * swordLength;
        ParentTransform.localPosition = new Vector3(.25f, - .25f) * (swordLength - 1f);
    }
    private void SetColliderLength(float length)
    {
        transform.localScale = new Vector3(1, length, 1);
    }
    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();
        ParentTransform = transform.parent;

    }
    private void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnSwordCollision?.Invoke(collision);
    }
}
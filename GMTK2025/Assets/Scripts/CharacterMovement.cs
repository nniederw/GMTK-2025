using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float PlayerSpeed = 1.0f;
    private Func<Vector2> MovementDirection = () => Vector2.zero;
    public void SetMovementDirectionFunction(Func<Vector2> movementDirection)
    {
        MovementDirection = movementDirection;
    }
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        var MovementDir = MovementDirection();
        if (MovementDir.sqrMagnitude > 1.00001f)
        {
            MovementDir = MovementDir.normalized;
        }
        rb2d.linearVelocity = MovementDir * PlayerSpeed;
    }
}
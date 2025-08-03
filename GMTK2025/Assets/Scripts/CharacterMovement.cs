using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private Rigidbody2D Rb2d;
    [SerializeField] private float CharacterSpeed = 1.0f;
    private Func<Vector2> MovementDirection = () => Vector2.zero;
    public void SetMovementDirectionFunction(Func<Vector2> movementDirection)
    {
        MovementDirection = movementDirection;
    }
    public void SetCharacterSpeed(float speed)
    {
        CharacterSpeed = speed;
    }
    private void Start()
    {
        Rb2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        var MovementDir = MovementDirection();
        if (MovementDir.sqrMagnitude > 1.00001f)
        {
            MovementDir = MovementDir.normalized;
        }
        Rb2d.linearVelocity = MovementDir * CharacterSpeed;
    }
}
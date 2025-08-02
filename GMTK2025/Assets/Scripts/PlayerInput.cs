using UnityEngine;
[RequireComponent(typeof(CharacterValues)), RequireComponent(typeof(CharacterMovement))]
public class PlayerInput : MonoBehaviour
{
    private CharacterValues CharacterValues;
    private CharacterMovement CharacterMovement;
    private void Awake()
    {
        CharacterValues = GetComponent<CharacterValues>();
        CharacterValues.SetWantsItemFunction(WantsItem);
        CharacterValues.SetWantsToAttackFunction(WantsToAttack);
        CharacterValues.SetWantsToBlockFunction(WantsToBlock);
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterMovement.SetMovementDirectionFunction(MovementDirection);
    }
    private bool WantsItem(Item item)
    {
        return Input.GetKeyDown(KeyCode.E);
    }
    private bool WantsToAttack()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
    private bool WantsToBlock()
    {
        //Debug.Log($"Wants to block: {Input.GetMouseButton(1)}");
        return Input.GetMouseButton(1);
    }
    private Vector2 MovementDirection()
    {
        var ix = Input.GetAxis("Horizontal");
        var iy = Input.GetAxis("Vertical");
        return new Vector2(ix, iy).normalized;
    }
}
using UnityEngine;
[RequireComponent(typeof(CharacterValues)), RequireComponent(typeof(CharacterMovement))]
public class BossAI : MonoBehaviour
{
    private Transform PlayerTransform;
    private CharacterValues CharacterValues;
    private CharacterMovement CharacterMovement;
    [SerializeField] private float TryAttackRange = 1.5f;
    private void Start()
    {
        CharacterValues = GetComponent<CharacterValues>();
        CharacterValues.SetWantsToAttackFunction(WantsToAttack);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterMovement.SetMovementDirectionFunction(MovementDirection);
    }
    private bool WantsToAttack()
    {
        return (PlayerTransform.position - transform.position).sqrMagnitude <= TryAttackRange * TryAttackRange;
    }
    private Vector2 MovementDirection()
    {
        return PlayerTransform.position - transform.position;
    }
    private void Update()
    {

    }
}
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterValues)), RequireComponent(typeof(CharacterMovement)), RequireComponent(typeof(SpriteRenderer))]
public class BossAI : MonoBehaviour
{
    private Transform PlayerTransform;
    private CharacterValues CharacterValues;
    private CharacterMovement CharacterMovement;
    [SerializeField] private float TryAttackRange = 1.5f;
    [SerializeField] private List<Item> DefaultStartItems;
    [SerializeField] private Sprite NormalPlayerSprite;
    private void Start()
    {
        if (NormalPlayerSprite == null) { throw new System.Exception($"{nameof(NormalPlayerSprite)} was null on {nameof(BossAI)}, please assign it."); }
        CharacterValues = GetComponent<CharacterValues>();
        CharacterValues.SetWantsToAttackFunction(WantsToAttack);
        if (GameManager.BossHasBeenDefeatedBefore())
        {
            GameManager.GetBossDefeatingLoadout().ForEach(item => CharacterValues.TakeItem(item));
            GetComponent<SpriteRenderer>().sprite = NormalPlayerSprite;
        }
        else
        {
            DefaultStartItems.ForEach(item => CharacterValues.TakeItem(item));
        }
        CharacterValues.SubscibeToOnDeath(OnDeath);
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CharacterMovement = GetComponent<CharacterMovement>();
        CharacterMovement.SetMovementDirectionFunction(MovementDirection);
    }
    private void OnDeath()
    {
        GameManager.SaveBossDefeatingLoadout(PlayerTransform.GetComponent<CharacterValues>().AllItems());
        PlayerWinPopup.DisplayWinPopup();
        Destroy(gameObject);
    }
    private bool WantsToAttack()
    {
        return (PlayerTransform.position - transform.position).sqrMagnitude <= TryAttackRange * TryAttackRange;
    }
    private Vector2 MovementDirection()
    {
        return PlayerTransform.position - transform.position;
    }
}
using UnityEngine;
[RequireComponent(typeof(PlayerValues))]
public class SwordAttackSound : MonoBehaviour
{
    [SerializeField] private AudioClip SwordAttackClip;
    private AudioSource SwordAttackSoundSource;
    private void Start()
    {
        if (SwordAttackClip == null) { throw new System.Exception($"{nameof(SwordAttackClip)} was null in {nameof(SwordAttackSound)}, please assign it."); }
        PlayerValues playerValues = GetComponent<PlayerValues>();
        playerValues.SubscribeToSwordAttack(PlaySwordAttackSound);
    }
    private void PlaySwordAttackSound()
    {
        if (SwordAttackSoundSource == null)
        {
            SwordAttackSoundSource = gameObject.AddComponent<AudioSource>();
            SwordAttackSoundSource.clip = SwordAttackClip;
            SwordAttackSoundSource.playOnAwake = false;
        }
        SwordAttackSoundSource.PlayOneShot(SwordAttackClip);
    }
}
using UnityEngine;
[RequireComponent(typeof(EnemyAI))]
public class EnemyDieSound : MonoBehaviour
{
    [SerializeField] private AudioClip EnemyDieClip;
    private AudioSource EnemyDieSoundSource;
    private void Start()
    {
        if (EnemyDieClip == null) { throw new System.Exception($"{nameof(EnemyDieClip)} was null in {nameof(EnemyDieSound)}, please assign it."); }
        EnemyAI enemyAi = GetComponent<EnemyAI>();
        enemyAi.SubscribeToOnDeath(PlayEnemyDieSound);
    }
    private void PlayEnemyDieSound()
    {
        var gobj = new GameObject("Enemy Death");
        EnemyDieSoundSource = gobj.AddComponent<AudioSource>();
        EnemyDieSoundSource.clip = EnemyDieClip;
        EnemyDieSoundSource.playOnAwake = false;
        EnemyDieSoundSource.Play();
        Destroy(gobj, EnemyDieClip.length + 0.02f + 20f);
    }
}
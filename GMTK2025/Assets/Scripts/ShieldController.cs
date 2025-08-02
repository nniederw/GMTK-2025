using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ShieldController : SpriteController
{
    [SerializeField] private Vector2 BlockShieldDisplacement = new Vector2(0f, .2f);
    public void StartBlock()
    {
        //Debug.Log("Start Block");
        transform.localPosition += (Vector3)BlockShieldDisplacement;
    }
    public void StopBlock()
    {
        //Debug.Log("Stop Block");
        transform.localPosition -= (Vector3)BlockShieldDisplacement;
    }
}
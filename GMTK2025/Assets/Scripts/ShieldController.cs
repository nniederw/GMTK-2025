using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ShieldController : MonoBehaviour
{
    [SerializeField] private Vector2 BlockShieldDisplacement = new Vector2(0f, .2f);
    private SpriteRenderer SpriteRenderer;
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
    public void UpdateSprite(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
    }
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
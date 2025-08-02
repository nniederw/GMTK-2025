using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteController : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void UpdateSprite(Sprite sprite)
    {
        SpriteRenderer.sprite = sprite;
    }
}
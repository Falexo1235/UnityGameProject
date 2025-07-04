using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite newSprite;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeToNewSprite()
    {
        spriteRenderer.sprite = newSprite;
    }
}
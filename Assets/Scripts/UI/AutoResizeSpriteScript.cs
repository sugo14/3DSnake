using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoResizeSprite : MonoBehaviour
{
    void Start()
    {
        Resize();
    }

    public void Resize()
    {
        // Get the SpriteRenderer component
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null || spriteRenderer.sprite == null)
            return;

        // Get the sprite's size in pixels and its PPU
        Sprite sprite = spriteRenderer.sprite;
        Vector2 spriteSize = sprite.bounds.size; // Size in world units

        // Calculate the scale to make it exactly 1x1
        Vector3 scale = transform.localScale;
        scale.x = 1 / spriteSize.x;
        scale.y = 1 / spriteSize.y;

        // Apply the new scale
        transform.localScale = scale;
    }
}

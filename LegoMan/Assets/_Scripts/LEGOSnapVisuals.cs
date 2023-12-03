using UnityEngine;

public class LEGOSnapVisuals : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Brick"))
        {
            var topSpriteRenderer = other.GetComponent<SpriteRenderer>();
            
            if (topSpriteRenderer.sortingOrder <= _spriteRenderer.sortingOrder)
            {
                topSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder + 1;
            }
        }
    }
}

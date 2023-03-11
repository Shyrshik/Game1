using Unity.VisualScripting;
using UnityEngine;

public class ItemInWorld : Slot
{
    private void Awake()
    {
            }
    private void AddSprite(Sprite sprite)
    {
        SpriteRenderer image = GetComponent<SpriteRenderer>();
        image.sprite = sprite;
    }
    private void RemoveSprite(Sprite sprite)
    {
        Destroy(gameObject);
    }

}

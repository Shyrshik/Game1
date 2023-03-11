using Unity.VisualScripting;
using UnityEngine;

public class ItemInWorld : Slot
{
    protected override void AddSprite(Sprite sprite)
    {
        SpriteRenderer image = GetComponent<SpriteRenderer>();
        image.sprite = sprite;
    }
    protected override void RemoveSprite()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

}

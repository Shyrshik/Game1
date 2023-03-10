using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private int Index;
    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            SetSprite(_item.Icon);
        }
    }
    private Item _item;
    private void SetSprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }
}

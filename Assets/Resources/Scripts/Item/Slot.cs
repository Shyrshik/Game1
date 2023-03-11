using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public static readonly Item Empty;
    public Item Item
    {
        get
        {
            return _item;
        }
        //set
        //{
        //    _item = value;
        //    SetSprite(_item.Settings.Icon);
        //}
    }
    private Item _item = Empty;
    
    static Slot()
    {
        if (Empty.IsUnityNull())
        {
            Empty = new Item();
        }
    }
    private void SetSprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }
    
}

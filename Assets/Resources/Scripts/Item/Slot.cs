using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item Item { get; }
    private Item _item = Item.Empty;
    private static Sprite _defaultSprite;
    private void Awake()
    {
        if (_defaultSprite.IsUnityNull())
        {
            _defaultSprite = GetComponent<Image>().sprite;
        }
    }

    public bool AddItem(Item item)
    {
        if (IsEmpty())
        {
            _item = item;
            SetSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }
    public void RemoveItem()
    {
        _item = Item.Empty;
        SetSprite(_defaultSprite);
    }
    public bool IsEmpty() => _item == Item.Empty ? true : false;
    private void SetSprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }
}
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public static readonly Item Empty;
    public Item Item { get; }
    private Item _item = Empty;
    private static Sprite _defaultSprite;
    static Slot()
    {
        if (Empty.IsUnityNull())
        {
            Empty = new Item();
        }
    }
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
        _item = Empty;
        SetSprite(_defaultSprite);
    }
    public bool IsEmpty() => _item == Empty ? true : false;
    private void SetSprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }
}
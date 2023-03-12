using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotInInventory : MonoBehaviour
{
    public Item Item { get => _item; }
    public static readonly SlotInWorld Empty;
    protected Item _item = Item.Empty;

    private static Sprite _defaultSprite;
    private Image _image;
    static SlotInInventory()
    {
        if (Empty == null)
        {
            Empty = new GameObject().AddComponent<SlotInWorld>();
        }
    }
    public bool AddItem(Item item)
    {
        if (IsEmpty())
        {
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }
    public void RemoveItem()
    {
        _item = Item.Empty;
        RemoveSprite();
    }
    public bool IsEmpty() => _item == Item.Empty;

    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
        if (_defaultSprite.IsUnityNull())
        {
            _defaultSprite = _image.sprite;
        }
    }
    protected virtual void AddSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    protected virtual void RemoveSprite()
    {
        AddSprite(_defaultSprite);
    }
}
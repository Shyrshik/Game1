using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item Item { get => _item; }
    protected Item _item = Item.Empty;
    protected static Sprite _defaultSprite;
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
    private void AddSprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
    }
    private void RemoveSprite()
    {
        AddSprite(_defaultSprite);
    }
}
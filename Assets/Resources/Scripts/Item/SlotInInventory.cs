using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotInInventory : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Item Item { get => _item; }
    protected Item _item = Item.Empty;

    private static Sprite _defaultSprite;
    private Image _image;
    public static SlotInInventory _itemOnDrag;
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
    public void OnBeginDrag(PointerEventData eventData)
    {
        _itemOnDrag.RemoveItem();
        _itemOnDrag.AddItem(Item);
        _itemOnDrag.transform.position = transform.position;
        RemoveItem();
        _itemOnDrag.SetActiveImage(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        _itemOnDrag.transform.position = Mouse.current.position.ReadValue();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _itemOnDrag.SetActiveImage(false);
    }
    public void SetActiveImage(bool active)
    {
        _image.enabled = active;
    }
    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
        if (_defaultSprite.IsUnityNull())
        {
            _defaultSprite = _image.sprite;
        }
        if (_itemOnDrag.IsUnityNull())
        {
            _itemOnDrag = this;
            _itemOnDrag = Instantiate(gameObject).GetComponent<SlotInInventory>();
            _itemOnDrag.SetActiveImage(false);
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
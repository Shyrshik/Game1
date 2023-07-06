using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotInBag : MonoBehaviour, ISlot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Item Item { get => IsEmpty ? null : _item; }
    protected Item _item;
    public bool IsEmpty { get => _isEmpty; }
    protected bool _isEmpty;
    private static Sprite _defaultSprite;
    private Image _image;
    public virtual bool AddItem(Item item)
    {
        if (IsEmpty && !item.IsUnityNull())
        {
            _isEmpty = false;
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }
    public void RemoveItem()
    {
        _isEmpty = true;
        _item = null;
        RemoveSprite();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty)
        {
            eventData.Reset();
            return;
        }
        InventoryManager.Cursor.transform.position = transform.position;
        InventoryManager.Cursor.color = Color.white;
        InventoryManager.Cursor.sprite = _item.Settings.Icon;
        RemoveSprite();
    }
    public void OnDrag(PointerEventData eventData)
    {
        InventoryManager.Cursor.transform.position = Mouse.current.position.ReadValue();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryManager.Cursor.color = new Color(1, 1, 1, 0);
        InventoryManager.Cursor.sprite = _defaultSprite;
        ISlot target;
        if (!eventData.pointerCurrentRaycast.gameObject.IsUnityNull() &&
            eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ISlot>(out target) &&
            target.AddItem(_item))
        {
            RemoveItem();
        }
        else
        {
            AddSprite(_item.Settings.Icon);
        }
    }
    protected virtual void Awake()
    {
        _isEmpty = true;
        _image = GetComponent<Image>();
        if (_defaultSprite.IsUnityNull())
        {
            _defaultSprite = _image.sprite;
        }
        //if (_itemOnDrag.IsUnityNull())
        //{
        //    _itemOnDrag = this;
        //    _itemOnDrag = Instantiate(gameObject).GetComponent<SlotInBag>();
        //    _itemOnDrag.SetActiveImage(false);
        //}
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
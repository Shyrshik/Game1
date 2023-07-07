using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SlotInBag : MonoBehaviour, ISlot, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField] private List<TypeOfItem> _typeOfItems;
    public Item Item { get => IsEmpty ? null : _item; }
    public Image ImageBack { get; set; }
    public Image ImageFront { get; set; }
    protected Item _item;
    public bool IsEmpty { get => _isEmpty; }
    protected bool _isEmpty;
    private Image _image;
    private enum TypeOfItem
    {
        Any,
        AnyWeapon,
        AnyArmor,
        None
    }
    public virtual bool AddItem(Item item)
    {
        if (IsEmpty && !item.IsUnityNull() && CanAdd(item))
        {
            _isEmpty = false;
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }
    public bool CanAdd(Item item)
    {
        foreach (var type in _typeOfItems)
        {
            if (type switch
            {
                TypeOfItem.Any => item is Item,
                TypeOfItem.AnyWeapon => item is Weapon,
                TypeOfItem.AnyArmor => item is Armor,
                TypeOfItem.None => false,
                _ => false,
            })
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveItem()
    {
        _isEmpty = true;
        _item = null;
        HideSprite();
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
        HideSprite();
        InventoryManager.OnBeginDrag(_item);
        ImageFront.sprite = InventoryManager.SlotSprite.FrontNotActivity;
    }
    public void OnDrag(PointerEventData eventData)
    {
        InventoryManager.OnDrag();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryManager.Cursor.color = Color.clear;
        InventoryManager.OnEndDrag();
        ISlot target;
        if (!eventData.pointerCurrentRaycast.gameObject.IsUnityNull() &&
            eventData.pointerCurrentRaycast.gameObject.TryGetComponent<ISlot>(out target) &&
            target.CanAdd(_item) &&
           (target.IsEmpty || CanAdd(target.Item)))
        {
            Item targetItem = target.Item;
            target.RemoveItem();
            target.AddItem(_item);
            RemoveItem();
            AddItem(targetItem);
        }
        else
        {
            UnhideSprite();
        }
    }
    protected virtual void Awake()
    {
        _image = GetComponent<Image>();
        RemoveItem();
        ImageBack = GetComponentInParent<Image>();
        foreach (var image in GetComponentsInChildren<Image>())
        {
            if (image != _image)
            {
                ImageFront = image;
            }
        }
    }
    private void OnValidate()
    {
        if (_typeOfItems.Count < 1)
        {
            _typeOfItems.Add(TypeOfItem.Any);
        }
    }
    protected virtual void AddSprite(Sprite sprite)
    {
        UnhideSprite();
        _image.sprite = sprite;
    }
    protected virtual void UnhideSprite()
    {
        _image.color = Color.white;
    }
    protected virtual void HideSprite()
    {
        _image.color = Color.clear;
    }

}
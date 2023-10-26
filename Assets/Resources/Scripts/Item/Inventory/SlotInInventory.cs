using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Items
{
    public class SlotInInventory : Slot, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField] private List<TypeOfItem> _typeOfItems;
        public Image ImageBack { get; set; }
        public Image ImageFront { get; set; }
        private Image _image;
        public override bool CanAdd(Item item)
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
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
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
            if (IsEmpty)
            {
                return;
            }
            InventoryManager.OnDrag();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            InventoryManager.Cursor.color = Color.clear;
            InventoryManager.OnEndDrag();
            ISlot target;
            if (!eventData.pointerCurrentRaycast.gameObject.IsUnityNull() &&
                eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out target) &&
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
        private void Awake()
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
        protected override void Add(Sprite sprite)
        {
            UnhideSprite();
            _image.sprite = sprite;
        }
        protected override void Remove()
        {
            HideSprite();
        }
        private void UnhideSprite()
        {
            _image.color = Color.white;
        }
        private void HideSprite()
        {
            _image.color = Color.clear;
        }

    }
}
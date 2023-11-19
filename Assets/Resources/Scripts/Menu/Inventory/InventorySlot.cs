using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class InventorySlot : MonoBehaviour, ISlot, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public Image ImageBack { get; set; }
        public Image ImageFront { get; set; }
        private ISlot _slot;
        public ISlot Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                Refresh();
            }
        }
        public Item Item => Slot.Item;
        public bool IsEmpty => Slot.IsEmpty;

        private Image _image;

        public bool TryAddItem(Item item)
        {
            if (Slot.TryAddItem(item))
            {
                Refresh();
                return true;
            }
            return false;
        }
        public bool CanAdd(Item item) => Slot.CanAdd(item);
        public void RemoveItem() => Slot.RemoveItem();
        public void Refresh()
        {
            if (_slot is not null &&
                Item is not null)
            {
                _image.sprite = Item.Settings.Icon;
                UnhideSprite();
            }
            else
            {
                _image.sprite = null;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            Inventory.Cursor.transform.position = transform.position;
            Inventory.Cursor.color = Color.white;
            Inventory.Cursor.sprite = Item.Settings.Icon;
            HideSprite();
            Inventory.OnBeginDrag(Item);
            ImageFront.sprite = Inventory.SlotSprite.FrontNotActivity;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            Inventory.OnDrag();
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            Inventory.Cursor.color = Color.clear;
            Inventory.OnEndDrag();
            ISlot target;
            if (eventData.pointerCurrentRaycast.gameObject is not null &&
                eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out target) &&
                target.CanAdd(Item) &&
               (target.IsEmpty || CanAdd(target.Item)))
            {
                Item targetItem = target.Item;
                target.RemoveItem();
                target.TryAddItem(Item);
                RemoveItem();
                TryAddItem(targetItem);
            }
            else
            {
                UnhideSprite();
            }
        }
        private void Awake()
        {
            _image = GetComponent<Image>();
            //RemoveItem();
            ImageBack = GetComponentInParent<Image>();
            foreach (var image in GetComponentsInChildren<Image>())
            {
                if (image != _image)
                {
                    ImageFront = image;
                }
            }
        }
        //protected void Add(Sprite sprite)
        //{
        //    UnhideSprite();
        //    _image.sprite = sprite;
        //}
        //protected void RemoveSlot()
        //{
        //    HideSprite();
        //}
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
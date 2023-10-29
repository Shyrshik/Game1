using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Menu
{
    public class Inventory : MonoBehaviour, IMenu
    {
        [SerializeField] private Image _cursor;
        [SerializeField] private Sprite _slotBack;
        [SerializeField] private Sprite _slotFrontNotActivity;
        [SerializeField] private Sprite _slotFrontEnable;
        [SerializeField] private Sprite _slotFrontDisable;
        [SerializeField] private Transform _bagSlotsContainer;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Equipment _equipment;
        [SerializeField] private InventorySlot _helmet;
        [SerializeField] private InventorySlot _body;
        [SerializeField] private InventorySlot _leftWeapon;
        [SerializeField] private InventorySlot _rightWeapon;
        public Equipment Equipment
        {
            get => _equipment;
            set => _equipment = value;
        }
        public static Image Cursor { get; private set; }
        private static InventorySlot[] _slots;
        private Equipment _actualEquipment;
        private Canvas _inventoryCanvas;
        public static class SlotSprite
        {
            public static Sprite Back { get; set; }
            public static Sprite FrontNotActivity { get; set; }
            public static Sprite FrontEnable { get; set; }
            public static Sprite FrontDisable { get; set; }
        }
        public static void OnBeginDrag(Item addingItem)
        {
            SetSlotFrontStatus(addingItem);
        }
        public static void OnDrag()
        {
            Cursor.transform.position = Mouse.current.position.ReadValue();
        }
        public static void OnEndDrag()
        {
            RestoreSlotFrontStatus();
        }
        public void RefreshSlots()
        {
            if (_actualEquipment != _equipment)
            {
                SetSlots();
                _actualEquipment = _equipment;
            }
            RestoreSlotFrontStatus();
        }
        public void SetSlots()
        {
            ClearAllSlots();
            _helmet.Slot = Equipment.Helmet;
            _body.Slot = Equipment.BodyArmor;
            _leftWeapon.Slot = Equipment.LeftWeapon;
            _rightWeapon.Slot = Equipment.RightWeapon;
            GameObject instance = null;
            foreach (ISlot slot in Equipment.Bag)
            {
                instance = Instantiate(_slotPrefab, _bagSlotsContainer);
                instance.GetComponentInChildren<InventorySlot>().Slot = slot;
            }
            _slots = GetComponentsInChildren<InventorySlot>();
        }
        public void ClearAllSlots()
        {
            _helmet.Slot = null;
            _body.Slot = null;
            _leftWeapon.Slot = null;
            _rightWeapon.Slot = null;
            List<Transform> slots = new List<Transform>();
            _bagSlotsContainer.GetComponentsInChildren(slots);
            foreach (Transform t in slots)
            {
                if (t is null ||
                    t == _bagSlotsContainer)
                {
                    continue;
                }
                Destroy(t);
            }
        }
        public void Hide()
        {
            if (_inventoryCanvas is not null)
                _inventoryCanvas.enabled = false;
        }
        public void Show()
        {
            if (_inventoryCanvas is not null)
                _inventoryCanvas.enabled = true;
            RefreshSlots();
            RefreshAllItems();
        }
        private void Awake()
        {
            Cursor = _cursor;
            SlotSprite.Back = _slotBack;
            SlotSprite.FrontNotActivity = _slotFrontNotActivity;
            SlotSprite.FrontEnable = _slotFrontEnable;
            SlotSprite.FrontDisable = _slotFrontDisable;
            _inventoryCanvas = GetComponent<Canvas>();
        }
        private void OnValidate()
        {
            if (_cursor is null)
            {
                Debug.LogError("Not set Cursor in Inventory!");
            }
            if (_slotBack is null ||
                _slotFrontNotActivity is null ||
                _slotFrontEnable is null ||
                _slotFrontDisable is null)
            {
                Debug.LogError("Not set Slot Sprite in Inventory!");
            }
            if (_bagSlotsContainer is null)
            {
                Debug.LogError("Not set bagSlots in Inventory!");
            }
            if (_slotPrefab is null)
            {
                Debug.LogError("Not set slotPrefab in Inventory!");
            }
            if (_leftWeapon is null)
                Debug.LogError("LeftWeapon is Null in Inventory.");
            if (_rightWeapon is null)
                Debug.LogError("RightWeapon is Null in Inventory.");
            if (_helmet is null)
                Debug.LogError("Helmet is Null in Inventory.");
            if (_body is null)
                Debug.LogError("Body is Null in Inventory.");
        }
        private static void SetSlotBack()
        {
            foreach (var slot in _slots)
            {
                slot.ImageBack.color = Color.white;
                slot.ImageBack.sprite = SlotSprite.Back;
            }
        }
        private static void SetSlotFrontStatus(Item addingItem)
        {
            foreach (var slot in _slots)
            {
                slot.ImageFront.color = Color.white;
                if (slot.CanAdd(addingItem))
                {
                    slot.ImageFront.sprite = SlotSprite.FrontEnable;
                }
                else
                {
                    slot.ImageFront.sprite = SlotSprite.FrontDisable;
                }
            }
        }
        private static void RestoreSlotFrontStatus()
        {
            foreach (var slot in _slots)
            {
                slot.ImageFront.color = Color.clear;
            }
        }
        private static void RefreshAllItems()
        {
            foreach (var slot in _slots)
            {
                slot.Refresh();
            }
        }

    }
}
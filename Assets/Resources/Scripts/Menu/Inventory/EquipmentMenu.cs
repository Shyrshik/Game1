using Items;
using UnityEngine;

namespace Menu
{
    public class EquipmentMenu : MonoBehaviour
    {
        [SerializeField] private InventorySlot _leftWeapon;
        [SerializeField] private InventorySlot _rightWeapon;
        [SerializeField] private InventorySlot _helmet;
        [SerializeField] private InventorySlot _body;
        private Item _movedItem;
        public Item AddLeftWeapon(Item item) => AddItem(_leftWeapon, item);
        public Item AddRightWeapon(Item item) => AddItem(_rightWeapon, item);
        public Item AddHelmet(Item item) => AddItem(_helmet, item);
        public Item AddBody(Item item) => AddItem(_body, item);
        private void OnValidate()
        {
            if (_leftWeapon is null)
                Debug.LogError("LeftWeapon is Null in Equipment.");
            if (_rightWeapon is null)
                Debug.LogError("RightWeapon is Null in Equipment.");
            if (_helmet is null)
                Debug.LogError("Helmet is Null in Equipment.");
            if (_body is null)
                Debug.LogError("Body is Null in Equipment.");
        }
        private Item AddItem(InventorySlot slot, Item item)
        {
            _movedItem = slot.Item;
            slot.RemoveItem();
            slot.AddItem(item);
            return _movedItem;
        }

    }
}
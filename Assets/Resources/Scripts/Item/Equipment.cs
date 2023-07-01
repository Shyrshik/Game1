using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Equipment : MonoBehaviour
{
   [SerializeField] private SlotInInventory _leftWeapon;
    [SerializeField] private SlotInInventory _rightWeapon;
    [SerializeField] private SlotInInventory _helmet;
    [SerializeField] private SlotInInventory _body;
    private Item _item;
    public Item AddLeftWeapon(Item item)
    {
        _item = _leftWeapon.Item;
        _leftWeapon.AddItem(item);
        if (IsEmpty())
        {
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return _item;
    }

}

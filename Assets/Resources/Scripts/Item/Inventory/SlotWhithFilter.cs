using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class SlotWithFilter : Slot
    {
        [SerializeField] private List<TypeOfItem> _typeOfItems;
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
    }
}
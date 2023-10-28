using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class SlotWithFilter : Slot
    {
        [SerializeField] private List<TypeOfItems> _typeOfItem;

        public SlotWithFilter():this(new List<TypeOfItems>{ TypeOfItems.Any }) {        }

        public SlotWithFilter(List<TypeOfItems> typeOfItem)
        {
            if (typeOfItem is null)
            {
                _typeOfItem = new List<TypeOfItems> { TypeOfItems.None };
            }
            else
            {
                _typeOfItem = typeOfItem;
            }
        }

        public List<TypeOfItems> TypeOfItem
        {
            get => _typeOfItem;
            set
            {
                if (IsEmpty)
                {
                    _typeOfItem = value;
                }
                else
                {
                    Debug.LogError("TypeOfItem not set, slot is not Empty.");
                }
            }
        }
        public override bool CanAdd(Item item)
        {
            foreach (var type in _typeOfItem)
            {
                if (type switch
                {
                    TypeOfItems.Any => item is Item,
                    TypeOfItems.AnyWeapon => item is Weapon,
                    TypeOfItems.AnyArmor => item is Armor,
                    TypeOfItems.None => false,
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
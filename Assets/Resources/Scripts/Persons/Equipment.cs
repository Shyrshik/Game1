using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DisallowMultipleComponent]
public class Equipment : MonoBehaviour
{
    [SerializeField] private List<TypeOfItems> _helmetType  = new(1){TypeOfItems.None};
    [SerializeField] private List<TypeOfItems> _bodyArmorType = new(1){TypeOfItems.None};
    [SerializeField] private List<TypeOfItems> _leftWeaponType = new(1){TypeOfItems.None};
    [SerializeField] private List<TypeOfItems> _rightWeaponType = new(1){TypeOfItems.None};
    public int ItemAmountInBag
    {
        get
        {
            return _itemAmountInBag;
        }
        set
        {
            if (value < 1)
            {
                _itemAmountInBag = 1;
            }
            else
            {
                _itemAmountInBag = value;
            }
        }
    }
    [SerializeField, Min(1)] private int _itemAmountInBag = 1;
    public ISlot Helmet { get => _helmet; }
    private ISlot _helmet;
    public ISlot BodyArmor { get => _bodyArmor; }
    private ISlot _bodyArmor;
    public ISlot LeftWeapon { get => _leftWeapon; }
    private ISlot _leftWeapon;
    public ISlot RightWeapon { get => _rightWeapon; }
    private ISlot _rightWeapon;
    public List<ISlot> Bag { get => _bag; }
    private List< ISlot> _bag;

    private void Awake()
    {
        _helmet = new SlotWithFilter(_helmetType);
        _bodyArmor = new SlotWithFilter(_bodyArmorType);
        _leftWeapon = new SlotWithFilter(_leftWeaponType);
        _rightWeapon = new SlotWithFilter(_rightWeaponType);
        _bag = Enumerable.Repeat(new SlotSimple(), _itemAmountInBag).ToList<ISlot>();
    }
    public bool AddItemToEmptySlot(Item item)
    {
        bool AddToBag(Item item)
        {
            foreach (var slot in _bag)
            {
                if (slot.AddItem(item))
                {
                    return true;
                }
            }
            return false;
        }
        if (item is not null ||
            _helmet.AddItem(item) ||
            _bodyArmor.AddItem(item) ||
            _leftWeapon.AddItem(item) ||
            _rightWeapon.AddItem(item) ||
            AddToBag(item))
        {
            return true;
        }
        return false;
    }
}

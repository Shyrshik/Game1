using Items;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class Equipment : MonoBehaviour
{
    //[SerializeField] private GameObject _inventory;
    //[SerializeField] private Transform _items;
    //[SerializeField] private GameObject _slot;
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
    [SerializeField] private List<TypeOfItems> _itemsInBagType = new(1){TypeOfItems.Any};
    private Canvas _canvas;
    private SlotInInventory[] _slots;
    private InputСontroller _inputController;
    private bool _open = true;


    private void Awake()
    {
        //for (int i = 0; i < _itemAmountInBag; i++)
        //{
        //    Instantiate(_slot, _items);
        //}
        //_slots = _items.GetComponentsInChildren<SlotInInventory>();
        //_canvas = _inventory.GetComponent<Canvas>();
        //_inputController = new();
        //_inputController.Player.Inventory.performed += context => Open();
        //Open();
    }
    public bool AddItem(Item Item1)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty && slot.CanAdd(Item1))
            {
                slot.AddItem(Item1);
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(Item item)
    {
        foreach (var slot in _slots)
        {
            if (slot.Item == item)
            {
                slot.RemoveItem();
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(int index)
    {
        if (index < _slots.Length)
        {
            _slots[index].RemoveItem();
            return true;
        }
        return false;
    }

    public void Open()
    {
        if (_open)
        {
            Time.timeScale = 1f;
            _open = false;
            _canvas.enabled = false;
        }
        else
        {
            Time.timeScale = 0f;
            _open = true;
            _canvas.enabled = true;
        }
    }
    private void OnEnable() => _inputController.Enable();
    private void OnDisable() => _inputController.Disable();
}

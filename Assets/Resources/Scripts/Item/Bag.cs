using Unity.VisualScripting;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private Transform _items;
    [SerializeField] private GameObject _slot;
    public int ItemAmount
    {
        get
        {
            return _itemAmount;
        }
        set
        {
            if (value < 1)
            {
                _itemAmount = 1;
            }
            else
            {
                _itemAmount = value;
            }
        }
    }
    [SerializeField, Min(1)] private int _itemAmount = 5;
    private Canvas _canvas;
    private Slot[] _slots;
    private InputСontroller _inputController;
    private bool _open = true;


    private void Awake()
    {
        for (int i = 0; i < _itemAmount; i++)
        {
            Instantiate(_slot, _items);
        }
        _slots = _inventory.GetComponentsInChildren<Slot>();
        _canvas = _inventory.GetComponent<Canvas>();
        _inputController = new();
        _inputController.Player.Inventory.performed += context => Open();
        Open();
    }
    public bool AddItem(Item item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            Item slot = _slots[i].Item;
            if (slot.IsUnityNull())
            {
                _slots[i].Item = item;
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(Item item)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            Item slot = _slots[i].Item;
            if (slot == item)
            {
                _slots[i].Item = null;
                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(int index)
    {
        if (index < _slots.Length)
        {
            _slots[index].Item = null;
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

using Unity.VisualScripting;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;
    private Canvas _canvas;
    private Slot[] _slots;
    private InputСontroller _inputController;
    private bool _open = true;
    private int _itemAmount = 6;


    private void Awake()
    {
        //_slots= new Item[_itemAmount];
        _slots= Inventory.GetComponentsInChildren<Slot>();
        _canvas = Inventory.GetComponent<Canvas>();
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

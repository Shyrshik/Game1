using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Canvas _canvas;
    private InputСontroller _inputСontroller;
    private bool _open = true;

    [SerializeField] private GameObject _player;
    [SerializeField] private Bag _bag;
    [SerializeField] private Image[] _slots;
    [SerializeField] private Transform _inventorySlots;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _inputСontroller = new();
        _inputСontroller.Player.Inventory.performed += context => Open();
        Open();

        //_slots = _inventorySlots.GetComponentInChildren<Image>();
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
    private void OnEnable() => _inputСontroller.Enable();
    private void OnDisable() => _inputСontroller.Disable();
}

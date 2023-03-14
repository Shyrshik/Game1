using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
[DisallowMultipleComponent]
[RequireComponent(typeof(Moved)),
    RequireComponent(typeof(Bag))]
public class Player : MonoBehaviour
{
    [SerializeField] private WeaponSettings _defaultWeaponSettings;
    [SerializeField] private TextMeshProUGUI _textAboveThePlayer;

    private InputСontroller _inputController;
    private Moved _move;
    private Bag _bag;
    private Weapon _defaultWeapon;
    private Weapon _firstWeapon;
    private Weapon _secondWeapon;
    private LayerMask _MyEnemies;
    private SlotInWorld _itemInWorld;

    private void Awake()
    {
        _inputController = new();
        _move = GetComponentInChildren<Moved>();
        _bag = GetComponentInChildren<Bag>();
        if (_textAboveThePlayer.IsUnityNull())
        {
            _textAboveThePlayer = GetComponentInChildren<TextMeshProUGUI>();
        }
        _textAboveThePlayer.text = "";
    }
    private void Start()
    {
        _MyEnemies = LayerMask.GetMask("Enemies");

        _defaultWeapon = _defaultWeaponSettings.GetNewWeapon(1);

        _firstWeapon = WeaponSpawner.GetRandomWeapon(1);
        _firstWeapon.EnemyLayers = _MyEnemies;
        _firstWeapon.OwnerTransform = transform;
        _inputController.Player.FirstWeapon.performed += context => ControlFirstWeapon();
        ControlFirstWeapon();
        _bag.AddItem(_firstWeapon);

        _secondWeapon = WeaponSpawner.GetRandomWeapon(1);
        _secondWeapon.EnemyLayers = _MyEnemies;
        _secondWeapon.OwnerTransform = transform;
        _inputController.Player.SecondWeapon.performed += context => ControlSecondWeapon();
        ControlSecondWeapon();
        _bag.AddItem(_secondWeapon);

        _inputController.Player.Run.performed += context => Run();
        _inputController.Player.Run.canceled += context => NotRun();

        _inputController.Player.Action.performed += context => TakeItem();
    }

    private void FixedUpdate()
    {
        _move.Move(_inputController.Player.Move.ReadValue<Vector2>());
    }
    private void ControlFirstWeapon()
    {
        if (IsInvoking(nameof(AttackFirstWeapon)))
        {
            CancelInvoke(nameof(AttackFirstWeapon));
            EventManager.SendFirstWeapon(false);
        }
        else
        {
            AttackFirstWeapon();
            EventManager.SendFirstWeapon(true);
        }
    }
    private void AttackFirstWeapon() => Invoke(nameof(AttackFirstWeapon), _firstWeapon.Attack());
    private void ControlSecondWeapon()
    {
        if (IsInvoking(nameof(AttackSecondWeapon)))
        {
            CancelInvoke(nameof(AttackSecondWeapon));
            EventManager.SendSecondWeapon(false);
        }
        else
        {
            AttackSecondWeapon();
            EventManager.SendSecondWeapon(true);
        }
    }
    private void AttackSecondWeapon() => Invoke(nameof(AttackSecondWeapon), _secondWeapon.Attack());
    private void Run() => _move.SetRun(true);
    private void NotRun() => _move.SetRun(false);
    private void OnEnable() => _inputController.Enable();
    private void OnDisable() => _inputController.Disable();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _itemInWorld = collision.gameObject.GetComponentInChildren<SlotInWorld>();
        if (!_itemInWorld.IsUnityNull())
        {
            _textAboveThePlayer.text = "Get\r\n" + _itemInWorld.Item.Settings.Title;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _textAboveThePlayer.text = "";
        _itemInWorld = null;
    }
    private void TakeItem()
    {
        if (!_itemInWorld.IsUnityNull())
        {

            if (_bag.AddItem(_itemInWorld.Item))
            {
                _itemInWorld.RemoveItem();
            }
            else
            {
                _itemInWorld.PlayAnimation();
            }
        }
    }

}
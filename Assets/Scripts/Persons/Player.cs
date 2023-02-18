using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D)),
    RequireComponent(typeof(SpriteRenderer)),
    RequireComponent(typeof(Moved)),
    RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private UnityEngine.Vector2 _movePlayer;
    private UnityEngine.Vector2 _movePlayerOld;
    private Rigidbody2D _rigidBody;
    private Vector2 _cameraPosition;
    private Vector2 _moveCamera;
    private InputСontroller _inputСontroller;
    private Moved _move;
    private Bag _bag;
    /// ////////////////////////////////////

    //[SerializeField] private float AttackRange = 2f;
    //[SerializeField] private float _attackCoolDown = 0.3f;
    //[SerializeField] private float _damage = 40f;
    private Weapon _firstWeapon;
    //private float _ticFirstWeapon = 0.1f;
    private Weapon _secondWeapon;
    //private float _ticSecondWeapon = 0.1f;
    private LayerMask _MyEnemies;
    [field: SerializeField] private Sword BaseKnuckle { get; set; }

    /// /////////////////////////////////////////
    private void Awake()
    {
        _inputСontroller = new();
        _move = GetComponent<Moved>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _bag = GetComponent<Bag>();
    }
    private void Start()
    {
        _MyEnemies = LayerMask.GetMask("Enemies");
        _firstWeapon = WeaponSpawner.GetRandomWeapon(1);
        _firstWeapon.EnemyLayers = _MyEnemies;
        _firstWeapon.OwnerTransform = transform;
        _inputСontroller.Player.FirstWeapon.performed += context => ControlFirstWeapon();
        ControlFirstWeapon();
        _bag.AddItem(_firstWeapon);

        _secondWeapon = WeaponSpawner.GetRandomWeapon(1);
        _secondWeapon.EnemyLayers = _MyEnemies;
        _secondWeapon.OwnerTransform = transform; 
        _inputСontroller.Player.SecondWeapon.performed += context => ControlSecondWeapon();
        ControlSecondWeapon();
        _bag.AddItem(_secondWeapon);

        _inputСontroller.Player.Run.performed += context => Run();
        _inputСontroller.Player.Run.canceled += context => NotRun();
    }
    private void FixedUpdate()
    {
        _movePlayer = _inputСontroller.Player.Move.ReadValue<Vector2>();
        _move.Move(_movePlayer);                                                //Перемещение Player.
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
    private void AttackSecondWeapon()=> Invoke(nameof(AttackSecondWeapon), _secondWeapon.Attack());
    private void Run() => _move.SetRun(true);
    private void NotRun() => _move.SetRun(false);
    private void OnEnable() => _inputСontroller.Enable();
    private void OnDisable() => _inputСontroller.Disable();
}
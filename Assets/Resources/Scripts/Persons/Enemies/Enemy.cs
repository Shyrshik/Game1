using UnityEngine;
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    public float MaxDistanceToPlayer
    {
        get => _maxDistanceToPlayer;
        set
        {
            if (value < 5f)
                _maxDistanceToPlayer = 5f;
            else
                _maxDistanceToPlayer = value;
        }
    }
    [SerializeField, Min(5)] private float _maxDistanceToPlayer = 15f;
    private Rigidbody2D _rigidBody;
    private Vector2 _wayToPlayer;
    private GameManager _gameManager;
    private Rigidbody2D _rigidBodyPlayer;
    private float _distanceToPlayer;
    private Weapon _firstWeapon;
    private float _ticFirstWeapon = 0.3f;
    private Moved _move;
    private LayerMask _MyEnemies;
    private void Awake()
    {
        _move = GetComponent<Moved>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBodyPlayer = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
        _gameManager = FindObjectOfType<GameManager>();
        _maxDistanceToPlayer = Mathf.Pow(_maxDistanceToPlayer, 2);
    }
    private void Start()
    {
        _MyEnemies = LayerMask.GetMask("Allies");
        _firstWeapon = WeaponSpawner.GetRandomWeapon(1);

        _firstWeapon.EnemyLayers = _MyEnemies;
        _firstWeapon.OwnerTransform = transform;
        _firstWeapon.BaseDamageCorrect(0.25f);
        _firstWeapon.Radius /= 2f;
        AttackFirstWeapon();
    }
    private void AttackFirstWeapon()
    {
        _ticFirstWeapon = _firstWeapon.Attack();
        Invoke(nameof(AttackFirstWeapon), _ticFirstWeapon);
    }
    private void FixedUpdate()
    {
        _wayToPlayer = _rigidBodyPlayer.position - _rigidBody.position;
        _distanceToPlayer = _wayToPlayer.sqrMagnitude;
        if (_distanceToPlayer > _maxDistanceToPlayer)
        {
            _gameManager.PostEnemy(gameObject);
            return;
        }
        _wayToPlayer.Normalize();
        _move.Move(_wayToPlayer);
    }
}


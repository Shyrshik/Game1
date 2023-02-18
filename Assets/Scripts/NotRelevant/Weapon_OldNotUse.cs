using UnityEngine;

//[CreateAssetMenu(fileName = "Weapon", menuName = "MySO/NewWeapon", order = 0)]
public class Weapon_OldNotUse//:Swords
{
    public LayerMask EnemyLayers { get; set; } = LayerMask.GetMask("Enemies");
    [field: SerializeField] public int Damage { get; set; } = 1;

    public float Range
    {
        get => _range;
        set => _range = value < 0 ? 0 : value;
    }
    [SerializeField, Min(0f)] private float _range = 0f;
    public float CoolDown
    {
        get => _coolDown;
        set => _coolDown = value > 0.01f ? value : 0.01f;
    }
    [SerializeField, Min(0.01f)] private float _coolDown = 0.05f;
    [field: SerializeField] public bool AttackAll { get; set; } = false;
    public float TicAttack
    {
        get => _ticAttack;
        set => _ticAttack = value > 0.01f ? value : 0.01f;
    }
    [SerializeField, Min(0.01f)] private float _ticAttack = 0.1f;
    private Collider2D _targetAttackLinked;
    private Vector2 _targetPosition;
    public Weapon_OldNotUse(LayerMask enemyLayers, int damage = 1, float range = 0f, float coolDown = 0.05f, bool attackAll = false, float ticAttack = -1f)
    {
        EnemyLayers = enemyLayers;
        Damage = damage;
        Range = range;
        CoolDown = coolDown;
        AttackAll = attackAll;
        if (ticAttack > 0) TicAttack = ticAttack;

    }
    public float AttackHim(Vector2 youPosition)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(youPosition, Range, EnemyLayers);
        if (hitEnemies.Length < 1)
            return TicAttack;
        Vector2 enemyMagnitude;
        if (!AttackAll && _targetAttackLinked != null)
        {
            _targetPosition = _targetAttackLinked.transform.position;
            enemyMagnitude = youPosition - _targetPosition;
            if (enemyMagnitude.magnitude > Range)
                _targetAttackLinked = null;
            else if (_targetAttackLinked.GetComponent<Health>().ApplyDamage(Damage))
                return CoolDown;
            else
                _targetAttackLinked = null;

        }
        foreach (Collider2D enemy in hitEnemies)
        {
            _targetAttackLinked = enemy;
            if (_targetAttackLinked.GetComponent<Health>().ApplyDamage(Damage) && !AttackAll)
                break;
        }
        return CoolDown;
    }
}

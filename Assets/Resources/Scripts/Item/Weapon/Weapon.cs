using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [field: SerializeField, Min(1f)] public int Level { get; set; } = 1;
    [field: SerializeField, Min(1f)] public float LevelMultiplier { get; set; } = 1f;
    public int BaseMinDamage
    {
        get => _baseMinDamage;
        set => _baseMinDamage = value < 0 ? 0 : value;
    }
    [SerializeField, Min(0)] private int _baseMinDamage;
    public int BaseMaxDamage
    {
        get => _baseMaxDamage;
        set => _baseMaxDamage = value < _baseMinDamage ? _baseMinDamage : value;
    }
    [SerializeField, Min(0)] private int _baseMaxDamage;
    public float Radius
    {
        get => _radius;
        set => _radius = value < 0 ? 0 : value;
    }
    [SerializeField, Min(0f)] private float _radius = 0f;
    public float CoolDown
    {
        get => _coolDown;
        set => _coolDown = value > 0.01f ? value : 0.01f;
    }
    [SerializeField, Min(0.01f)] private float _coolDown = 0.01f;
    public float TicTimeAttack
    {
        get => _ticAttack;
        set => _ticAttack = value > 0.01f ? value : 0.01f;
    }
    [SerializeField, Min(0.01f)] private float _ticAttack = 0.01f;
    [field: SerializeField] public int CountEnemies { get; set; } = 1;
    public LayerMask EnemyLayers { get; set; }
    public Transform OwnerTransform { get; set; }
    public List<Collider2D> FixTargetEnemies { get; set; } = new List<Collider2D>();
    public abstract float Attack();
    public void BaseDamageCorrect(float multiplier)
    {
        _baseMinDamage = (int)(_baseMinDamage * multiplier);
        _baseMaxDamage = (int)(_baseMaxDamage * multiplier);
    }
}

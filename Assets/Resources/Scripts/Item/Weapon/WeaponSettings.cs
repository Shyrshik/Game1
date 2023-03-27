using UnityEngine;

public abstract class WeaponSettings : ItemSettings
{
    [field: SerializeField, Min(1)] public int Level { get; protected set; } = 1;
    [field: SerializeField, Min(1f)] public float LevelMultiplier { get; private set; } = 1f;
    [field: SerializeField, Min(0)] public int BaseMinDamageMin { get; private set; } = 0;
    [field: SerializeField, Min(0)] public int BaseMinDamageMax { get; private set; } = 0;
    [field: SerializeField, Min(0)] public int BaseMaxDamageMin { get; private set; } = 0;
    [field: SerializeField, Min(0)] public int BaseMaxDamageMax { get; private set; } = 0;
    [field: SerializeField, Min(0f)] public float RadiusAttackMin { get; private set; } = 0f;
    [field: SerializeField, Min(0f)] public float RadiusAttackMax { get; private set; } = 0f;
    [field: SerializeField, Min(0.01f)] public float CoolDownMin { get; private set; } = 0.01f;
    [field: SerializeField, Min(0.01f)] public float CoolDownMax { get; private set; } = 0.01f;
    [field: SerializeField, Min(0.01f)] public float TicAttackMin { get; private set; } = 0.01f;
    [field: SerializeField, Min(0.01f)] public float TicAttackMax { get; private set; } = 0.01f;
    [field: SerializeField] public int CountEnemiesMin { get; private set; } = 1;
    [field: SerializeField] public int CountEnemiesMax { get; private set; } = 1;
    public abstract Weapon GetNewWeapon(int level);
}

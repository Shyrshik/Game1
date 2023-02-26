using UnityEngine;
[CreateAssetMenu(fileName = "SwordProperties", menuName = "MySO/Weapon/NewSwordProperties", order = 0)]
public class SwordSettings : WeaponSettings
{
    public override Sword GetNewWeapon(int level)
    {
        return GetNewWeapon(this);
    }
    public static Sword GetNewWeapon(SwordSettings settings)
    {
       Sword result = new()
        {
            Icon = settings.Icon,
            Level = settings.Level,
            LevelMultiplier = settings.LevelMultiplier,
            BaseMinDamage = (int)(Random.Range(settings.BaseMinDamageMin, settings.BaseMaxDamageMax) * settings.Level * settings.LevelMultiplier),
            BaseMaxDamage = (int)(Random.Range(settings.BaseMaxDamageMin, settings.BaseMaxDamageMax) * settings.Level * settings.LevelMultiplier),
            Radius = Random.Range(settings.RadiusAttackMin, settings.RadiusAttackMax),
            CoolDown = Random.Range(settings.CoolDownMin, settings.CoolDownMax),
            TicTimeAttack = Random.Range(settings.TicAttackMin, settings.TicAttackMax),
            CountEnemies = Random.Range(settings.CountEnemiesMin, settings.CountEnemiesMax),
        };
        if (result.BaseMaxDamage < result.BaseMinDamage)
            result.BaseMaxDamage = result.BaseMinDamage;
        return result;
    }
}

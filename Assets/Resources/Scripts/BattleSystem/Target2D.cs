using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Target2D
{
    public static List<Collider2D> GetNearest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        List<Collider2D> enemies = new();
        if (radius < 0 || count < 1)
        {
            return enemies;
        }
        ContactFilter2D enemyFilter = new()
        {
            layerMask = enemyLayers,
            useLayerMask = true
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        return enemies.OrderBy(x => (weaponPosition - x.transform.position).sqrMagnitude).Take(count).ToList<Collider2D>();
    }
    public static List<Collider2D> GetFarthest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        List<Collider2D> enemies = new();
        if (radius < 0 || count < 1)
        {
            return enemies;
        }
        ContactFilter2D enemyFilter = new()
        {
            layerMask = enemyLayers,
            useLayerMask = true
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        return enemies.OrderByDescending(x => (weaponPosition - x.transform.position).sqrMagnitude).Take(count).ToList<Collider2D>();
    }
    public static List<Collider2D> GetRandom(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        List<Collider2D> enemies = new();
        if (radius < 0 || count < 1)
        {
            return enemies;
        }
        ContactFilter2D enemyFilter = new()
        {
            layerMask = enemyLayers,
            useLayerMask = true
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        if (enemies.Count <= count)
        {
            return enemies;
        }
        return enemies.OrderBy(x => UnityEngine.Random.Range(int.MinValue, int.MaxValue)).Take(count).ToList();
    }
    public static void GetFixTarget(ref List<Collider2D> fixTargets, AttackPosition enemyPosition, Vector3 weaponPosition, float radius,
        LayerMask enemyLayers, int count = 1)
    {
        if (count < 1)
        {
            fixTargets = new();
        }
        List<Collider2D> enemies = enemyPosition switch
        {
            AttackPosition.Nearest => GetNearest(weaponPosition, radius, enemyLayers, count),
            AttackPosition.Farthest => GetFarthest(weaponPosition, radius, enemyLayers, count),
            AttackPosition.Random => GetRandom(weaponPosition, radius, enemyLayers, count),
            _ => new(),
        };
        if (enemies.Count < 1 || fixTargets.Count < 1)
        {
            fixTargets = enemies;
        }
        float sqrRadius = radius * radius;
        fixTargets = fixTargets.Where(x => !x.IsUnityNull() && ((weaponPosition - x.transform.position).sqrMagnitude < sqrRadius))
            .Union(enemies).Take(count).ToList();
    }
    public enum AttackPosition
    {
        Nearest,
        Farthest,
        Random
    }
    /// <summary>
    /// Return Weapon.CoolDown or Weapon.TicTimeAttack
    /// </summary>
    /// <param name="weapon"></param>
    /// <param name="enemyPosition"></param>
    /// <param name="fixTarget"></param>
    /// <returns></returns>
    public static float Attack(Weapon weapon, AttackPosition enemyPosition, bool fixTarget = true)
    {
        List<Collider2D> enemies = weapon.FixTargetEnemies;
        if (fixTarget)
        {
            switch (enemyPosition)
            {
                case AttackPosition.Nearest:
                    GetFixTarget(ref enemies, AttackPosition.Nearest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                        weapon.CountEnemies);
                    break;
                case AttackPosition.Farthest:
                    GetFixTarget(ref enemies, AttackPosition.Farthest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                        weapon.CountEnemies);
                    break;
                case AttackPosition.Random:
                    GetFixTarget(ref enemies, AttackPosition.Random, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                        weapon.CountEnemies);
                    break;
            }
        }
        else
        {
            enemies = enemyPosition switch
            {
                AttackPosition.Nearest => GetNearest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies),
                AttackPosition.Farthest => GetFarthest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies),
                AttackPosition.Random => GetRandom(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies),
                _ => new()
            };
        }
        weapon.FixTargetEnemies = enemies;
        float result = weapon.TicTimeAttack;
        if (enemies.Count < 1)
        {
            return result;
        }
        int damage = UnityEngine.Random.Range(weapon.BaseMinDamage, weapon.BaseMaxDamage);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.GetComponent<Health>().ApplyDamage(damage))
                result = weapon.CoolDown;
        }
        return result;
    }
}

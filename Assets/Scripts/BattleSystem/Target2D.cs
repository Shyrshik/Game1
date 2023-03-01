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
        ContactFilter2D enemyFilter = new ()
        {
            layerMask = enemyLayers
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        if (enemies.Count <= count)
        {
            return enemies;
        }
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
            layerMask = enemyLayers
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        if (enemies.Count <= count)
        {
            return enemies;
        }
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
            layerMask = enemyLayers
        };
        Physics2D.OverlapCircle(weaponPosition, radius, enemyFilter, enemies);
        if (enemies.Count <= count)
        {
            return enemies;
        }
        List<Collider2D> resultEnemy = new (count);
        for (int i = 0; i < count; i++)
        {
            int random = UnityEngine.Random.Range(i, enemies.Count);
            resultEnemy[i] = enemies[random];
            enemies[random] = enemies[i];
            enemies[i] = resultEnemy[i];
        }
        return resultEnemy;
    }
    public static List<Collider2D> GetFixTarget(ref List<Collider2D> fixTargets, AttackPosition enemyPosition, Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        List<Collider2D> enemies = enemyPosition switch
        {
            AttackPosition.Nearest => GetNearest(weaponPosition, radius, enemyLayers, count),
            AttackPosition.Farthest => GetFarthest(weaponPosition, radius, enemyLayers, count),
            AttackPosition.Random => GetRandom(weaponPosition, radius, enemyLayers, count),
            _ => new(),
        };
        if (enemies.IsUnityNull())
        {
            fixTargets = null;
            return fixTargets;
        }
        if (count < 0 || fixTargets.IsUnityNull())
        {
            fixTargets = enemies;
            return fixTargets;
        }
        int fixTargetsLength = fixTargets.Count;
        if (fixTargetsLength < 1)
        {
            fixTargets = enemies;
            return fixTargets;
        }
        float sqrRadius = radius * radius;
        List<Collider2D> result = new (count);
        Collider2D enemy;
        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < fixTargetsLength; j++)
            {
                enemy = fixTargets[j];
                if (enemy.IsUnityNull())
                    continue;
                if (Mathf.Abs(Vector2.SqrMagnitude((enemy.transform.position - weaponPosition))) > sqrRadius)
                {
                    fixTargets[j] = null;
                    continue;
                }
                for (int k = 0; k < enemies.Count; k++)
                {
                    if (enemies[k] == enemy)
                    {
                        enemies[k] = null;
                        break;
                    }
                }
                result[i] = enemy;
                i++;
                if (i >= count)
                    break;
            }
            if (i >= count)
                break;
            for (int k = 0; k < enemies.Count; k++)
            {
                if (!enemies[k].IsUnityNull())
                {
                    result[i] = enemies[k];
                    i++;
                }
                if (i >= count)
                    break;
            }

        }
        fixTargets = result;
        return fixTargets;
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
                    GetFixTarget(ref enemies, AttackPosition.Nearest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
                case AttackPosition.Farthest:
                    GetFixTarget(ref enemies, AttackPosition.Farthest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
                case AttackPosition.Random:
                    GetFixTarget(ref enemies, AttackPosition.Random, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
            }
        }
        else
        {
            switch (enemyPosition)
            {
                case AttackPosition.Nearest:
                    enemies = GetNearest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
                case AttackPosition.Farthest:
                    enemies = GetFarthest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
                case AttackPosition.Random:
                    enemies = GetRandom(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
                    break;
            }
        }
        weapon.FixTargetEnemies = enemies;
        float result = weapon.TicTimeAttack;
        if (enemies.IsUnityNull())
            return result;
        int damage = UnityEngine.Random.Range(weapon.BaseMinDamage, weapon.BaseMaxDamage);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.IsUnityNull())
                continue;
            if (enemy.GetComponent<Health>().ApplyDamage(damage))
                result = weapon.CoolDown;
        }
        return result;
    }
}

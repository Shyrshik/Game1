using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Target2D
{
    public static Collider2D[] GetNearest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        Collider2D[] enemies = new Collider2D[0];
        if (radius < 0 && count < 1) 
        { 
            return enemies;
        }
        var radiusPortion = radius / 4;
        for (int i = 1; i <= 4; i++)
        {
            enemies = Physics2D.OverlapCircleAll(weaponPosition, radiusPortion * i, enemyLayers);
            if (enemies.Length >= count)
                break;
        }
        if (enemies.Length < 1)
            return enemies;
        return enemies.OrderBy(x => (weaponPosition - x.transform.position).sqrMagnitude).Take(count).ToArray(); 
    }
    public static Collider2D[] GetFarthest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        if (count < 1)
            return null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(weaponPosition, radius, enemyLayers);
        int enemiesLength = enemies.Length;
        if (enemiesLength < 1)
            return null;
        if (count < 0)
            return enemies;
        float distance;
        float farthestDistance;
        Collider2D[] resultEnemy = new Collider2D[count];
        for (int i = 0; i < count; i++)
        {
            farthestDistance = -1;
            for (int j = i; j < enemiesLength; j++)
            {
                distance = Mathf.Abs(Vector2.SqrMagnitude((enemies[j].transform.position - weaponPosition)));
                if (distance > farthestDistance)
                {
                    farthestDistance = distance;
                    resultEnemy[i] = enemies[j];
                    enemies[j] = enemies[i];
                    enemies[i] = resultEnemy[i];
                }
            }
        }
        return resultEnemy;
    }
    public static Collider2D[] GetRandom(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        if (count < 1)
            return null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(weaponPosition, radius, enemyLayers);
        int enemiesLength = enemies.Length;
        if (enemiesLength < 1)
            return null;
        if (count < 0)
            return enemies;
        Collider2D[] resultEnemy = new Collider2D[count];
        for (int i = 0; (i < count) && (i < enemiesLength); i++)
        {
            int random = UnityEngine.Random.Range(i, enemiesLength);
            resultEnemy[i] = enemies[random];
            enemies[random] = enemies[i];
            enemies[i] = resultEnemy[i];
        }
        return resultEnemy;
    }
    public static Collider2D[] GetFixTarget(ref Collider2D[] fixTargets, AttackPosition enemyPosition, Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
    {
        Collider2D[] enemies = null;
        switch (enemyPosition)
        {
            case AttackPosition.Nearest:
                enemies = GetNearest(weaponPosition, radius, enemyLayers, count);
                break;
            case AttackPosition.Farthest:
                enemies = GetFarthest(weaponPosition, radius, enemyLayers, count);
                break;
            case AttackPosition.Random:
                enemies = GetRandom(weaponPosition, radius, enemyLayers, count);
                break;
            default:
                break;
        }
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
        int fixTargetsLength = fixTargets.Length;
        if (fixTargetsLength < 1)
        {
            fixTargets = enemies;
            return fixTargets;
        }
        float sqrRadius = radius * radius;
        Collider2D[] result = new Collider2D[count];
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
                for (int k = 0; k < enemies.Length; k++)
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
            for (int k = 0; k < enemies.Length; k++)
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
        Collider2D[] enemies = weapon.FixTargetEnemies;
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

using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class Target2D
{
    public static class Get
    {

        public static List<Collider2D> Nearest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
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
        public static List<Collider2D> Farthest(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
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
        public static List<Collider2D> Random(Vector3 weaponPosition, float radius, LayerMask enemyLayers, int count = 1)
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
    }
    public static class Attack
    {
        private static List<Collider2D> enemies;
        public static class FixTarget
        {
            public static float Nearest(Weapon weapon)
            {
                SetFixTargets(ref enemies, AttackPosition.Nearest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies);
                return Apply(weapon);
            }
            public static float Farthest(Weapon weapon)
            {
                SetFixTargets(ref enemies, AttackPosition.Farthest, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies);
                return Apply(weapon);
            }
            public static float Random(Weapon weapon)
            {
                SetFixTargets(ref enemies, AttackPosition.Random, weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers,
                    weapon.CountEnemies);
                return Apply(weapon);
            }
            private static void SetFixTargets(ref List<Collider2D> fixTargets, AttackPosition enemyPosition, Vector3 weaponPosition,
                float radius, LayerMask enemyLayers, int count = 1)
            {
                if (count < 1)
                {
                    fixTargets = new();
                }
                List<Collider2D> enemies = enemyPosition switch
                {
                    AttackPosition.Nearest => Get.Nearest(weaponPosition, radius, enemyLayers, count),
                    AttackPosition.Farthest => Get.Farthest(weaponPosition, radius, enemyLayers, count),
                    AttackPosition.Random => Get.Random(weaponPosition, radius, enemyLayers, count),
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
        }
        public static float Nearest(Weapon weapon)
        {
            enemies = Get.Nearest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
            return Apply(weapon);
        }
        public static float Farthest(Weapon weapon)
        {
            enemies = Get.Farthest(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
            return Apply(weapon);
        }
        public static float Random(Weapon weapon)
        {
            enemies = Get.Random(weapon.OwnerTransform.position, weapon.Radius, weapon.EnemyLayers, weapon.CountEnemies);
            return Apply(weapon);
        }
        private static float Apply(Weapon weapon)
        {
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
}

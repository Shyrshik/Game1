
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    private static WeaponSettings[] _weaponsPrefabs;
    private void Awake()
    {
        _weaponsPrefabs = Resources.LoadAll<WeaponSettings>("ScriptableObjects/ItemSettings/WeaponSettings");
    }
    public static Weapon GetRandomWeapon(int level)
    {
        return _weaponsPrefabs[Random.Range(0, _weaponsPrefabs.Length)].GetNewWeapon(level);
    }
}

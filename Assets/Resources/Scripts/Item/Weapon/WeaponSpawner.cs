using Items;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _slot;
    private static GameObject s_slot;
    private static WeaponSettings[] s_weaponsPrefabs;
    private void Awake()
    {
        s_weaponsPrefabs = Resources.LoadAll<WeaponSettings>("ScriptableObjects/ItemSettings/WeaponSettings");
        s_slot = _slot;
    }
    public static Weapon GetRandomWeapon(int level)
    {
        return s_weaponsPrefabs[Random.Range(0, s_weaponsPrefabs.Length)].GetNewWeapon(level);
    }
    public static void ThrowNewWeaponInWorld(Vector3 position)
    {
        Instantiate(s_slot, position, Quaternion.identity).GetComponentInChildren<WorldSlot>().TryAddItem(GetRandomWeapon(1));
    }
}
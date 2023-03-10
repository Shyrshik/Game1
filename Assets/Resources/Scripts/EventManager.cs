using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityEvent<int> OnHealthToInterface = new();
    public static UnityEvent<float> OnEnemyCount = new();
    public static UnityEvent<bool> OnFirstWeapon = new();
    public static UnityEvent<bool> OnSecondWeapon = new();
    public float SetCount
    {
        get => s_setCount;
        set => s_setCount += value;
    }
    private static float s_setCount = 0f;

    public static void SendHealthToInterface(int health) => OnHealthToInterface.Invoke(health);
    public static void SendEnemyCount(float count = 0f)
    {
        s_setCount += count;
        OnEnemyCount.Invoke(s_setCount);
    }

    public static void SendFirstWeapon(bool on) => OnFirstWeapon.Invoke(on);
    public static void SendSecondWeapon(bool on) => OnSecondWeapon.Invoke(on);
}

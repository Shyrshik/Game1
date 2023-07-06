using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Image _cursor;
    public static Image Cursor { get; private set; }
    private void OnValidate()
    {
        if (_cursor == null)
        {
            Debug.LogError("Not set Cursor in InventoryManager!");
        }
    }
    private void Awake()
    {
        Cursor = _cursor;
    }
}

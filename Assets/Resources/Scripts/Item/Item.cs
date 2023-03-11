using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    public ItemSettings Settings
    {
        get
        {
            return _settings;
        }
        set
        {
            if (_settings.IsUnityNull())
            {
                _settings = value;
            }
            else
            {
                Debug.LogError("Ñan not be changed Settings.");
            }
        }
    }
    [SerializeField] private ItemSettings _settings = null;
    public static readonly Item Empty;
    static Item()
    {
        if (Empty.IsUnityNull())
        {
            Empty = new Item();
        }
    }
    public static bool IsEmpty(Item item) => item == Item.Empty;
    public bool IsEmpty() => IsEmpty(this);
}

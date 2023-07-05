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
}

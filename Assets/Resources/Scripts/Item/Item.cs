using Unity.VisualScripting;
using UnityEngine;

namespace Items
{
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
                    Debug.LogError("Can not be changed Settings.");
                }
            }
        }
        [SerializeField] private ItemSettings _settings = null;
    }
}
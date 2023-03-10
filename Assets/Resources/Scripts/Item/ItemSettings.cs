using UnityEngine;

public class ItemSettings : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; set; }
    [field: SerializeField] public string Title { get; set; }
    [field: SerializeField, TextArea(3, 6)] public string Description { get; set; }
}

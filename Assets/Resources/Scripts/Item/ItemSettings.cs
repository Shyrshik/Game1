using UnityEngine;
public class ItemSettings : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get;private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField, TextArea(3, 6)] public string Description { get; private set; }
}

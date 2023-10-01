using Unity.VisualScripting;
using UnityEngine;

public abstract class Slot : MonoBehaviour, ISlot
{
    public Item Item { get => IsEmpty ? null : _item; }
    protected Item _item;

    public bool IsEmpty { get => _isEmpty; }
    protected bool _isEmpty;

    public virtual bool AddItem(Item item)
    {
        if (IsEmpty && !item.IsUnityNull() && CanAdd(item))
        {
            _isEmpty = false;
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }

    public abstract bool CanAdd(Item item);

    public void RemoveItem()
    {
        _isEmpty = true;
        _item = null;
        Remove();
    }

    protected abstract void AddSprite(Sprite sprite);

    protected abstract void Remove();
}

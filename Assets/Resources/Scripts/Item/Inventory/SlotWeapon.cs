using Unity.VisualScripting;
public class SlotWeapon : SlotInInventory
{
    public override bool AddItem(Item item)
    {
        if (IsEmpty && !item.IsUnityNull() && item is Weapon)
        {
            _isEmpty = false;
            _item = item;
            AddSprite(_item.Settings.Icon);
            return true;
        }
        return false;
    }
}

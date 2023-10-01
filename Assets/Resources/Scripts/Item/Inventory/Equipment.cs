using UnityEngine;
public class Equipment : MonoBehaviour
{
    [SerializeField] private SlotInInventory _leftWeapon;
    [SerializeField] private SlotInInventory _rightWeapon;
    [SerializeField] private SlotInInventory _helmet;
    [SerializeField] private SlotInInventory _body;
    private Item _movedItem;
    public Item AddLeftWeapon(Item item) => AddItem(_leftWeapon, item);
    public Item AddRightWeapon(Item item) => AddItem(_rightWeapon, item);
    public Item AddHelmet(Item item) => AddItem(_helmet, item);
    public Item AddBody(Item item) => AddItem(_body, item);
    private void OnValidate()
    {
        if (_leftWeapon == null)
            Debug.LogError("LeftWeapon is Null in Equipment.");
        if (_rightWeapon == null)
            Debug.LogError("RightWeapon is Null in Equipment.");
        if (_helmet == null)
            Debug.LogError("Helmet is Null in Equipment.");
        if (_body == null)
            Debug.LogError("Body is Null in Equipment.");
    }
    private Item AddItem(SlotInInventory slot, Item item)
    {
        _movedItem = slot.Item;
        slot.RemoveItem();
        slot.AddItem(item);
        return _movedItem;
    }

}

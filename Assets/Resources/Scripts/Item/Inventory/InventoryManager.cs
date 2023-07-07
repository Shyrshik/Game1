using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Image _cursor;
    [SerializeField] private Sprite _slotBack;
    [SerializeField] private Sprite _slotFrontNotActivity;
    [SerializeField] private Sprite _slotFrontEnable;
    [SerializeField] private Sprite _slotFrontDisable;
    private static SlotInBag[] _slots;
    public static Image Cursor { get; private set; }
    public static class SlotSprite
    {
        public static Sprite Back { get; set; }
        public static Sprite FrontNotActivity { get; set; }
        public static Sprite FrontEnable { get; set; }
        public static Sprite FrontDisable { get; set; }
    }
    public static void OnBeginDrag(Item addingItem)
    {
        SetSlotFrontStatus(addingItem);
    }
    public static void OnDrag()
    {
        Cursor.transform.position = Mouse.current.position.ReadValue();
    }
    public static void OnEndDrag()
    {
        RestoreSlotFrontStatus();
    }
    private void OnValidate()
    {
        if (_cursor == null)
        {
            Debug.LogError("Not set Cursor in InventoryManager!");
        }
        if (_slotBack == null ||
            _slotFrontNotActivity == null ||
            _slotFrontEnable == null ||
            _slotFrontDisable == null)
        {
            Debug.LogError("Not set Slot Sprite in InventoryManager!");
        }
    }
    private void Awake()
    {
        Cursor = _cursor;
        SlotSprite.Back = _slotBack;
        SlotSprite.FrontNotActivity = _slotFrontNotActivity;
        SlotSprite.FrontEnable = _slotFrontEnable;
        SlotSprite.FrontDisable = _slotFrontDisable;
        _slots= GetComponentsInChildren<SlotInBag>();
        SetSlotBack();
        OnEndDrag();
    }
    private static void SetSlotBack()
    {
        foreach (var slot in _slots)
        {
            slot.ImageBack.color = Color.white;
            slot.ImageBack.sprite = SlotSprite.Back;
        }
    }
    private static void SetSlotFrontStatus(Item addingItem)
    {
        foreach (var slot in _slots)
        {
            slot.ImageFront.color = Color.white;
            if (slot.CanAdd(addingItem))
            {
                slot.ImageFront.sprite = SlotSprite.FrontEnable;
            }
            else
            {
                slot.ImageFront.sprite = SlotSprite.FrontDisable;
            }
        }
    }
    private static void RestoreSlotFrontStatus()
    {
        foreach (var slot in _slots)
        {
            slot.ImageFront.color = Color.clear;
        }
    }

}

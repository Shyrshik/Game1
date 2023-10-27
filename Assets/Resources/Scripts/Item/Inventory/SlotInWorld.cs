using UnityEngine;

namespace Items
{
    public class SlotInWorld : MonoBehaviour, ISlot
    {
        public Item Item => ((ISlot)_slot).Item;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private readonly SlotSimple _slot = new();

        public bool IsEmpty => ((ISlot)_slot).IsEmpty;

        public void PlayAnimation()
        {
            _animator.SetTrigger("PlayAnimation");
        }

        public bool AddItem(Item item)
        {
            return _slot.AddItem(item);
        }

        public bool CanAdd(Item item)
        {
            return ((ISlot)_slot).CanAdd(item);
        }

        public void RemoveItem()
        {
            ((ISlot)_slot).RemoveItem();
        }
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private void AddSprite()
        {
            _spriteRenderer.sprite = _slot.Item.Settings.Icon;
        }
        private void Remove()
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
        private void OnEnable()
        {
            _slot.Added += AddSprite;
            _slot.Removed += Remove;
        }
        private void OnDisable()
        {
            _slot.Added -= AddSprite;
            _slot.Removed -= Remove;
        }
    }
}
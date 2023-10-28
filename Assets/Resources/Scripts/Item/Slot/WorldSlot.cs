using UnityEngine;

namespace Items
{
    public class WorldSlot : MonoBehaviour, ISlot
    {
        public Item Item => _slot.Item;
        public bool IsEmpty => _slot.IsEmpty;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private readonly ISlot _slot = new SlotSimple();

        public bool AddItem(Item item) => _slot.AddItem(item);
        public bool CanAdd(Item item) => _slot.CanAdd(item);
        public void RemoveItem() => _slot.RemoveItem();
        public void PlayAnimation()
        {
            _animator.SetTrigger("PlayAnimation");
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
            //Потом придумаем куда убирать пустые предметы с карты мира, а пока:
            Destroy(gameObject.transform.parent.gameObject);
        }
        private void OnEnable()
        {
            if (_slot is IAddedEvent slotAdd)
            {
                slotAdd.Added += AddSprite;
            }
            if (_slot is IRemovedEvent slotRemove)
            {
                slotRemove.Removed += Remove;
            }
        }
        private void OnDisable()
        {
            if (_slot is IAddedEvent slotAdd)
            {
                slotAdd.Added -= AddSprite;
            }
            if (_slot is IRemovedEvent slotRemove)
            {
                slotRemove.Removed -= Remove;
            }
        }
    }
}
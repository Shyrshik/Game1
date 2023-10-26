using UnityEngine;

namespace Items
{
    public class SlotInWorld : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private readonly SlotSimple _slot = new();
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
            Destroy(gameObject.transform.parent.gameObject);
        }
        private void OnEnable()
        {
            _slot.ItemAdded += AddSprite;
            _slot.ItemRemoved += Remove;
        }
        private void OnDisable()
        {
            _slot.ItemAdded -= AddSprite;
            _slot.ItemRemoved -= Remove;
        }
    }
}
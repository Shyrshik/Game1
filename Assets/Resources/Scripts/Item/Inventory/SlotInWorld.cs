using UnityEngine;

public class SlotInWorld : SlotInBag
{
        private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    public void PlayAnimation()
    {
        _animator.SetTrigger("PlayAnimation");
    }
    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void AddSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
    protected override void HideSprite()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

}

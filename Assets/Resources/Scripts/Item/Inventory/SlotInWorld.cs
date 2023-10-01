using UnityEngine;

public class SlotInWorld : Slot
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    public void PlayAnimation()
    {
        _animator.SetTrigger("PlayAnimation");
    }
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void AddSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
    protected override void Remove()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }

    public override bool CanAdd(Item item) => true;
}

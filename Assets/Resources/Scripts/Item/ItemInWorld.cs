using Unity.VisualScripting;
using UnityEngine;

public class ItemInWorld : Slot
{
    private Animator _animator;
    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    protected override void AddSprite(Sprite sprite)
    {
        SpriteRenderer image = GetComponent<SpriteRenderer>();
        image.sprite = sprite;
    }
    protected override void RemoveSprite()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
    public void PlayAnimation() 
    {
        _animator.SetTrigger("PlayAnimation");
    }
}

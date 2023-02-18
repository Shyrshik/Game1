using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value < 0 ? 0 : value;
    }
    [SerializeField, Min(1f)] private int _maxHealth = 1;
    [SerializeField] private bool _sendHealthToInterface = false;

    private int _сurrentHealth;
    private SpriteRenderer _spriteRendererThis;
    private Color _standartColor;

    private void Awake()
    {
        _spriteRendererThis = GetComponent<SpriteRenderer>();
        _standartColor = _spriteRendererThis.color;
        _сurrentHealth = _maxHealth;
    }
    private void Start()
    {
        if (_sendHealthToInterface)
        {
            EventManager.SendHealthToInterface(_сurrentHealth);
        }
    }
    public bool ApplyDamage(int damage = 0)
    {
        if (_сurrentHealth <= 0)
            return false;
        if (damage < 0)
            damage = 0;
        _сurrentHealth -= damage;
        if (_sendHealthToInterface)
        {
            EventManager.SendHealthToInterface(_сurrentHealth);
        }
        if (_spriteRendererThis != null)
        {
            _spriteRendererThis.color = Color.red;
            Invoke(nameof(ReturnStandartColor), 0.2f);
        }
        if (_сurrentHealth <= 0)
        {
            Death();
        }
        return true;
    }
    private void ReturnStandartColor()
    {
        if (_spriteRendererThis != null)
            _spriteRendererThis.color = _standartColor;
    }
    private void Death()
    {
        if (_sendHealthToInterface)
        {
            _spriteRendererThis.color = Color.black;
        }
        else if (!_sendHealthToInterface)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (!_sendHealthToInterface)
            EventManager.SendEnemyCount(-1f);
    }
}

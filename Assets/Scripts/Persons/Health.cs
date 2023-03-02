using UnityEngine;
[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value < 0 ? 0 : value;
    }
    [SerializeField, Min(1f)] private int _maxHealth = 1;
    [SerializeField] private bool _sendHealthToInterface = false;

    private int _currentHealth;
    private SpriteRenderer _spriteRendererThis;
    private Color _standardColor;

    private void Awake()
    {
        _spriteRendererThis = GetComponent<SpriteRenderer>();
        _standardColor = _spriteRendererThis.color;
        _currentHealth = _maxHealth;
    }
    private void Start()
    {
        if (_sendHealthToInterface)
        {
            EventManager.SendHealthToInterface(_currentHealth);
        }
    }
    public bool ApplyDamage(int damage = 0)
    {
        if (_currentHealth <= 0)
            return false;
        if (damage < 0)
            damage = 0;
        _currentHealth -= damage;
        if (_sendHealthToInterface)
        {
            EventManager.SendHealthToInterface(_currentHealth);
        }
        if (_spriteRendererThis != null)
        {
            _spriteRendererThis.color = Color.red;
            Invoke(nameof(ReturnStandardColor), 0.2f);
        }
        if (_currentHealth <= 0)
        {
            Death();
        }
        return true;
    }
    private void ReturnStandardColor()
    {
        if (_spriteRendererThis != null)
            _spriteRendererThis.color = _standardColor;
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

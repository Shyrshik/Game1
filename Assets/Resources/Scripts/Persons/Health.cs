using Unity.VisualScripting;
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
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Color _standardColor;

    private void Awake()
    {
        if (_spriteRenderer.IsUnityNull())
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        if (_spriteRenderer.IsUnityNull())
        {
            Debug.LogError("Не получены компоненты SpriteRenderer.");
        }
        _standardColor = _spriteRenderer.color;
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
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.red;
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
        if (_spriteRenderer != null)
            _spriteRenderer.color = _standardColor;
    }
    private void Death()
    {
        if (_sendHealthToInterface)
        {
            _spriteRenderer.color = Color.black;
        }
        else if (!_sendHealthToInterface)
        {
            if (Random.Range(0, 100) < 30)
            {
                WeaponSpawner.ThrowNewWeaponInWorld(transform.position);
            }
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if (!_sendHealthToInterface)
            EventManager.SendEnemyCount(-1f);
    }
}

using Unity.VisualScripting;
using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class Moved : MonoBehaviour
{
    public float BaseSpeed
    {
        get => _baseSpeed;
        set
        {
            if (value < -100f)
                _baseSpeed = -100f;
            else if (value > 100f)
                _baseSpeed = 100f;
            else
                _baseSpeed = value;
        }
    }
    [SerializeField, Range(-100f, 100f)] private float _baseSpeed = 1f;
    public float RunMultiplier
    {
        get => _runMultiplier;
        set
        {
            if (value < 1f)
                _runMultiplier = 1f;
            else
                _runMultiplier = value;
        }
    }
    [SerializeField, Min(1f)] private float _runMultiplier = 2f;
    public float SpeedBuffs
    {
        get => _speedBuffs;
        set
        {
            _speedBuffs = value;
            SetCurrentSpeed();
        }
    }
    private float _speedBuffs = 1f;
    public Vector2 VectorVelocity { get; private set; }
    public float CurrentSpeed { get; private set; }
    private Rigidbody2D _rigidBody;
    private float _currentMultiplier;
    private float _angle;
    private float _moveSpeed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    private void Awake()
    {
        if (_animator.IsUnityNull())
        {
            _animator = GetComponent<Animator>();
        }
        if (_spriteRenderer.IsUnityNull())
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _rigidBody = GetComponent<Rigidbody2D>();
        if (_animator.IsUnityNull() || _spriteRenderer.IsUnityNull() || _rigidBody.IsUnityNull())
        { 
            Debug.LogError("Не получены компоненты Animator, SpriteRenderer или Rigidbody2D.");
        }
        SetRun(false);
    }
    public void Move(Vector2 velocity)
    {
        VectorVelocity = velocity * CurrentSpeed;
        _rigidBody.velocity = VectorVelocity;
        _moveSpeed = velocity.sqrMagnitude;
        _angle = _moveSpeed == 0 ? _angle : Vector2.SignedAngle(Vector2.right, VectorVelocity);
        if (_angle > 90)
        {
            _angle = 180 - _angle;
            _spriteRenderer.flipX = true;
        }
        else if (_angle < -90)
        {
            _angle = -180 - _angle;
            _spriteRenderer.flipX = true;
        }
        else if (_moveSpeed > 0) _spriteRenderer.flipX = false;
        _animator.SetFloat("Angel", _angle);
        _animator.SetFloat("Speed", _moveSpeed * CurrentSpeed);
    }
    public void SetCurrentSpeed() => CurrentSpeed = _currentMultiplier * _baseSpeed * _speedBuffs;
    public void SetRun(bool run)
    {
        _currentMultiplier = run ? _runMultiplier : 1;
        SetCurrentSpeed();
    }
}

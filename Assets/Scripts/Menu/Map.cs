using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(Camera))]
public class Map : MonoBehaviour
{
    public Transform Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            transform.parent = _parent;
        }
    }
    [SerializeField] private Transform _parent;
    public Sprite MarkOnMap
    {
        get => _markOnMap;
        set
        {
            _markOnMap = value;
            _spriteRenderer.sprite = _markOnMap;
            _spriteRenderer.transform.localScale = Vector3.one * (_camera.orthographicSize / 10 * MarkSize); ;
        }
    }
    [SerializeField] private Sprite _markOnMap;
    [field: SerializeField] public float MarkSize { get; set; } = 1;
    public float EnemyVisibilityRadius
    {
        get => _enemyVisibilityRadius;
        set
        {
            _enemyVisibilityRadius = value;
            _transformSpriteMask.localScale = Vector3.one * (_enemyVisibilityRadius * 2 + 1);
        }
    }
    [SerializeField] private float _enemyVisibilityRadius = 1;
    public int Scale
    {
        get => _scale;
        set
        {
            if (value < 1)
                _scale = 1;
            else if (value > 100)
                _scale = 100;
            else
                _scale = value;
            _camera.orthographicSize = _scale;
            MarkOnMap = MarkOnMap;
        }
    }
    [SerializeField, Range(1, 100)] private int _scale = 10;

    private SpriteRenderer _spriteRenderer;
    private Transform _transformSpriteMask;
    private Camera _camera;
    private void Awake()
    {
        _camera = GetComponent<Camera>();
        SetSpriteRendering();
        SetTransformSpriteMask();
        Parent = Parent;
    }
    private void SetTransformSpriteMask()
    {
        _transformSpriteMask = GetComponentInChildren<SpriteMask>().transform;
        if (_transformSpriteMask.IsUnityNull())
        {
            Debug.LogError("Не получен компонент SpriteMask.Transform.");
        }
        else
        {
            EnemyVisibilityRadius = EnemyVisibilityRadius;
        }
    }
    private void SetSpriteRendering()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (_spriteRenderer.IsUnityNull())
        {
            Debug.LogError("Не получен компонент SpriteRenderer.");
        }
        else
        {
            MarkOnMap = MarkOnMap;
        }
    }
    [ContextMenu(nameof(Apply))]
    public void Apply()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            Parent = Parent;
            EnemyVisibilityRadius = EnemyVisibilityRadius;
            Scale = Scale;
        }
    }
}

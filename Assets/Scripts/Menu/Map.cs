using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
        }
    }
    [SerializeField] private Sprite _markOnMap;
    public float EnemyVisibilityRadius
    {
        get => _enemyVisibilityRadius;
        set
        {
            _enemyVisibilityRadius = value;
            _transformSpriteMask.localScale = Vector3.one * _enemyVisibilityRadius;
        }
    }
    [SerializeField] private float _enemyVisibilityRadius;
    private SpriteRenderer _spriteRenderer;
    private static GameObject _instance;
    private Transform _transformSpriteMask;
    private void Awake()
    {
        CheckInstance();
        GetSpriteRendering();
        GetTransformSpriteMask();
        Parent = Parent;
    }
    private void CheckInstance()
    {
        if (_instance.IsUnityNull())
        {
            _instance = gameObject;
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }    
    private void GetTransformSpriteMask()
    {
        Transform _transformSpriteMask = GetComponentInChildren<SpriteMask>().transform;
        if (_transformSpriteMask.IsUnityNull())
        {
            Debug.LogError("Не получен компонент SpriteMask.Transform.");
        }
        else
        {
            EnemyVisibilityRadius = EnemyVisibilityRadius;
        }
    }
    private void GetSpriteRendering()
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
            MarkOnMap = MarkOnMap;
            Parent = Parent;
            EnemyVisibilityRadius = EnemyVisibilityRadius;
        }
    }
}

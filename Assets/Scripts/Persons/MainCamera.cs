using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject _trackingObject;
    [SerializeField] private Vector3 _positionCorrect = new(0f, -10f, -6);
    [SerializeField, Tooltip("Отрицательное значение = Установить автоматически.")] private float _maxDistanceToTrackingObject = -1;
    private Rigidbody2D _rigidBody2D;
    private Moved _trackingObjectMove;
    private Rigidbody2D _trackingObjectRigidBody2D;
    private Vector2 _moveCamera;
    private Vector2 _correct;
    private void Awake()
    {
        SetTrackingObject(_trackingObject);
        _correct = _positionCorrect;
        _rigidBody2D = GetComponent<Rigidbody2D>();
        if (_maxDistanceToTrackingObject < 0f)
            SetAutoDistanceToTrackingObject();
    }
    private void FixedUpdate()
    {
        _moveCamera = _trackingObjectRigidBody2D.position + _correct - _rigidBody2D.position;
        _moveCamera += Vector2.ClampMagnitude(_trackingObjectMove.VectorVelocity, _maxDistanceToTrackingObject)
            + _trackingObjectRigidBody2D.velocity;
        _rigidBody2D.velocity = _moveCamera;
    }
        public void SetAutoDistanceToTrackingObject()
    {
        _maxDistanceToTrackingObject = GetComponent<Camera>().orthographicSize * 1.7f;
    }
    public void SetTrackingObject(GameObject trackingObject)
    {
        try
        {
            _trackingObject = trackingObject;
            _trackingObjectMove = _trackingObject.GetComponent<Moved>();
            _trackingObjectRigidBody2D = _trackingObject.GetComponent<Rigidbody2D>();
            transform.position = _positionCorrect + _trackingObject.transform.position;
        }
        catch 
        {
            Debug.LogError("NoTargetForCamera");
        }
    }
}

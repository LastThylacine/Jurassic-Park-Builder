using UnityEngine;

public class CameraObjectFollowing : MonoBehaviour
{
    public static CameraObjectFollowing Current;

    public Transform Target { get; private set; }

    private Camera _camera;
    private Bounds _targetBounds;
    private Vector3 _previousPosition;

    private Vector3 _velocity = Vector3.zero;

    private void Awake() => _camera = FindObjectOfType<Camera>();

    private void Start() => Current = this;

    private void Update()
    {
        if (Target)
        {
            Vector2 targetPosition = _camera.WorldToViewportPoint(Target.position + _targetBounds.extents);

            if (targetPosition.x >= 0.7f || targetPosition.x <= 0.3f || targetPosition.y >= 0.7f || targetPosition.y <= 0.3f)
            {
                Vector3 position = _camera.ScreenToWorldPoint(Target.position);
                Vector3 direction = position - _previousPosition;

                _camera.transform.position += direction;

                _previousPosition = position;
            }
            else
            {
                Vector3 position = _camera.ScreenToWorldPoint(Target.position);
                _previousPosition = position;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        Target = target;

        if (target != null)
        {
            _targetBounds = Target.GetComponent<PlaceableObject>().Display.GetComponent<PolygonCollider2D>().bounds;
            _previousPosition = target.position;
        }
    }
}

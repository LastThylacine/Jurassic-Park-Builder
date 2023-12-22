using UnityEngine;

public class CameraObjectFollowing : MonoBehaviour
{
    public static CameraObjectFollowing Current;

    public Transform Target { get; private set; }

    private Camera _camera;
    private Bounds _targetBounds;
    private Vector3 _previousPosition;
    private PlaceableObjectMovementListener _placeableObjectMovementListener;

    private void Awake() => _camera = FindObjectOfType<Camera>();

    private void Start() => Current = this;

    private void Update()
    {
        if (Target && _placeableObjectMovementListener.IsMoving)
        {
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x > 0.75f * Screen.width || mousePosition.x < 0.25f * Screen.width
                || mousePosition.y > 0.75f * Screen.height || mousePosition.y < 0.25f * Screen.height)
            {
                Vector3 position = _camera.ScreenToWorldPoint(Target.position);
                Vector3 direction = position - _previousPosition;

                _camera.transform.position += direction.normalized * 0.0025f;

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
            _placeableObjectMovementListener = Target.GetComponentInChildren<PlaceableObjectMovementListener>();
        }
    }
}

using UnityEngine;

public class CameraObjectFollowing : MonoBehaviour
{
    public static CameraObjectFollowing Current;

    [SerializeField] private float _smoothDamp;

    public Transform Target { get; private set; }

    private Camera _camera;
    private Vector3 _velocity = Vector3.zero;
    private PlaceableObjectMovementListener _placeableObjectMovementListener;

    private Bounds _mapBounds;
    private float _objectWidth;
    private float _objectHeight;

    private void Awake() => _camera = FindObjectOfType<Camera>();

    private void Start()
    {
        Current = this;

        _mapBounds = Globals.WorldBounds;
    }

    private void Update()
    {
        if (!Target)
            return;

        if (_placeableObjectMovementListener.IsMoving)
        {
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x > 0.75f * Screen.width || mousePosition.x < 0.25f * Screen.width
                || mousePosition.y > 0.75f * Screen.height || mousePosition.y < 0.25f * Screen.height)
            {
                Vector2 targetPosition = new Vector2(Target.position.x, Target.position.y);

                _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, targetPosition, ref _velocity, _smoothDamp);
            }
        }
        else
        {
            Vector2 targetPosition = new Vector2(Target.position.x, Target.position.y);

            _camera.transform.position = Vector3.Lerp(_camera.transform.position, targetPosition, 2f * Time.deltaTime);
        }
    }

    //private void LateUpdate()
    //{
    //    if (!Target)
    //        return;

    //    Target.transform.localPosition = new Vector3
    //        (
    //        Mathf.Clamp(Target.transform.localPosition.x, _mapBounds.min.x, _mapBounds.max.x),
    //        Mathf.Clamp(Target.transform.localPosition.y, _mapBounds.min.y, _mapBounds.max.y),
    //        Target.transform.localPosition.z
    //        );
    //}

    public void SetTarget(Transform target)
    {
        Target = target;

        if (target != null)
        {
            _placeableObjectMovementListener = Target.GetComponentInChildren<PlaceableObjectMovementListener>();
        }
    }
}
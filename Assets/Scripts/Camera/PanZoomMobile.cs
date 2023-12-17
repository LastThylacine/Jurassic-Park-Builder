using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoomMobile : MonoBehaviour
{
    [SerializeField] private float _zoomMinimum;
    [SerializeField] private float _zoomMaximum;

    private Camera _camera;

    private bool _moveAllowed;
    private Vector3 _touchPosition;

    private Bounds _cameraBounds;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        RecalculateBounds();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                _moveAllowed = false;

                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                if (EventSystem.current.IsPointerOverGameObject(touchZero.fingerId)
                    || EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                {
                    return;
                }

                Vector2 touchZeroLastPosition = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPosition = touchOne.position - touchOne.deltaPosition;

                float touchDistance = (touchZeroLastPosition - touchOneLastPosition).magnitude;
                float currentTouchDistance = (touchZero.position - touchOne.position).magnitude;

                float difference = currentTouchDistance - touchDistance;

                Zoom(difference * 0.001f);
            }
            else
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        {
                            _moveAllowed = false;
                        }
                        else
                        {
                            _moveAllowed = true;
                        }

                        _touchPosition = _camera.ScreenToWorldPoint(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if (_moveAllowed)
                        {
                            Vector3 direction = _touchPosition - _camera.ScreenToWorldPoint(touch.position);
                            transform.position += direction;
                        }
                        break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(transform.position.y, _cameraBounds.min.y, _cameraBounds.max.y),
            transform.position.z
            );
    }

    private void Zoom(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, _zoomMinimum, _zoomMaximum);

        RecalculateBounds();
    }

    private void RecalculateBounds()
    {
        var height = _camera.orthographicSize;
        var width = height * _camera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.extents.x - width;

        var minY = Globals.WorldBounds.min.y + height;
        var maxY = Globals.WorldBounds.extents.y - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector2(minX, minY),
            new Vector2(maxX + Globals.WorldBounds.center.x, maxY + Globals.WorldBounds.center.y)
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3
            (((_cameraBounds.max.x - Mathf.Abs(_cameraBounds.min.x)) / 2f),
            ((_cameraBounds.max.y - Mathf.Abs(_cameraBounds.min.y)) / 2f)),
            new Vector3(_cameraBounds.max.x - _cameraBounds.min.x, _cameraBounds.max.y - _cameraBounds.min.y));
    }
}

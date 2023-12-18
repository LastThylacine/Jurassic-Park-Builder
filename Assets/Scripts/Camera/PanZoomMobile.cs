using UnityEngine;
using UnityEngine.EventSystems;

public class PanZoomMobile : MonoBehaviour
{
    [SerializeField] private float _zoomMinimum;
    [SerializeField] private float _zoomMaximum;

    private Camera _camera;
    private CameraWorldBounds _cameraWorldBounds;

    private bool _moveAllowed;
    private Vector3 _touchPosition;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _cameraWorldBounds = FindObjectOfType<CameraWorldBounds>();
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

    private void Zoom(float increment)
    {
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - increment, _zoomMinimum, _zoomMaximum);

        _cameraWorldBounds.RecalculateBounds();
    }
}

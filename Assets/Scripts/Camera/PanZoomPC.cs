using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanZoomPC : MonoBehaviour
{
    [SerializeField] private float _timeToZoom = 0.5f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveLerpRate;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 7f;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    private Camera _camera;
    private CameraWorldBounds _cameraWorldBounds;
    private Controls _controls;
    private Vector2 _startPoint;
    private Vector2 _startCameraPosition;
    private float _zoomDelta;
    private bool _isCanZoomed = true;

    private void Awake()
    {
        _controls = new Controls();

        _camera = Camera.main;

        _cameraWorldBounds = FindObjectOfType<CameraWorldBounds>();
    }

    private void Start()
    {
        _zoomDelta = (_maxZoom - _minZoom) / 2f;
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        if (_startPoint == Vector2.zero)
        {
            if (_isCanZoomed)
            {
                float scrollDelta = context.ReadValue<float>();

                StartCoroutine(Zoom(_timeToZoom, scrollDelta));
            }
        }
    }

    private void OnScrollButtonClick(InputAction.CallbackContext context)
    {
        Vector2 point = _camera.ScreenToViewportPoint(Input.mousePosition);

        _startPoint = point;
        _startCameraPosition = _camera.transform.position;
    }

    private void Update()
    {
        if (_startPoint == Vector2.zero) return;

        Vector2 point = _camera.ScreenToViewportPoint(Input.mousePosition);

        Vector2 offset = point - _startPoint;
        Vector2 newPosition = new Vector2((_startCameraPosition - (offset * _moveSpeed * (_camera.orthographicSize / 10))).x,
            (_startCameraPosition - (offset * _moveSpeed * (_camera.orthographicSize / 10))).y);

        transform.position = Vector2.Lerp(transform.position, newPosition, _moveLerpRate * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        if (_controls.Main.MouseScrollButon.ReadValue<float>() == 0)
        {
            _startPoint = Vector2.zero;
            _startCameraPosition = Vector2.zero;
        }
    }

    private IEnumerator Zoom(float time, float scrollDelta)
    {
        _isCanZoomed = false;

        float elapsedTime = 0f;
        float startZoom = _camera.orthographicSize;
        float endZoom;

        if (scrollDelta > 0)
        {
            endZoom = _camera.orthographicSize - _zoomDelta;
        }
        else if (scrollDelta < 0)
        {
            endZoom = _camera.orthographicSize + _zoomDelta;
        }
        else
        {
            endZoom = startZoom;
        }

        endZoom = Mathf.Clamp(endZoom, _minZoom, _maxZoom);

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(startZoom, endZoom, elapsedTime / time);

            _cameraWorldBounds.RecalculateBounds();

            yield return null;
        }

        _camera.orthographicSize = endZoom;

        _isCanZoomed = true;
    }

    private void OnEnable()
    {
        _controls.Enable();

        _controls.Main.MouseScroll.performed += OnZoom;
        _controls.Main.MouseScrollButon.started += OnScrollButtonClick;
        _controls.Main.MouseScrollButon.performed += OnScrollButtonClick;
    }

    private void OnDisable()
    {
        _controls.Disable();

        _controls.Main.MouseScroll.performed -= OnZoom;
        _controls.Main.MouseScrollButon.started += OnScrollButtonClick;
        _controls.Main.MouseScrollButon.performed += OnScrollButtonClick;
    }
}

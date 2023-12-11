using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLevelEditorMovement : MonoBehaviour
{
    [SerializeField] private float _timeToZoom = 0.5f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveLerpRate;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 7f;
    [SerializeField] private Vector2 _minPosition;
    [SerializeField] private Vector2 _maxPosition;

    private Camera _camera;
    private Controls _controls;
    private Vector2 _startPoint;
    private Vector2 _startCameraPosition;
    private float _zoomDelta;
    private bool _isCanZoomed = true;

    private void Awake()
    {
        _controls = new Controls();

        _camera = Camera.main;
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
                float zoomDelta = context.ReadValue<float>();

                if (zoomDelta > 0)
                {
                    StartCoroutine(ZoomDown(_timeToZoom));
                }
                else if (zoomDelta < 0)
                {
                    StartCoroutine(ZoomUp(_timeToZoom));
                }
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
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, _minZoom, _maxZoom);

        if (_startPoint == Vector2.zero) return;

        Vector2 point = _camera.ScreenToViewportPoint(Input.mousePosition);

        Vector2 offset = point - _startPoint;
        Vector2 newPosition = new Vector2((_startCameraPosition - (offset * _moveSpeed * (_camera.orthographicSize / 10))).x,
            (_startCameraPosition - (offset * _moveSpeed * (_camera.orthographicSize / 10))).y);


        newPosition.x = Mathf.Clamp(newPosition.x, _minPosition.x, _maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, _minPosition.y, _maxPosition.y);

        transform.position = Vector2.Lerp(transform.position, newPosition, _moveLerpRate * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);

        if (_controls.Main.MouseScrollButon.ReadValue<float>() == 0)
        {
            _startPoint = Vector2.zero;
            _startCameraPosition = Vector2.zero;
        }
    }

    private IEnumerator ZoomUp(float time)
    {
        _isCanZoomed = false;

        float elapsedTime = 0f;
        float startZoom = _camera.orthographicSize;
        float endZoom;

        if ((_camera.orthographicSize + _zoomDelta) <= _maxZoom)
        {
            endZoom = _camera.orthographicSize + _zoomDelta;
        }
        else
        {
            _isCanZoomed = true;
            yield break;
        }

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(startZoom, endZoom, elapsedTime / time);
            yield return null;
        }

        _camera.orthographicSize = endZoom;
        _isCanZoomed = true;
    }

    private IEnumerator ZoomDown(float time)
    {
        _isCanZoomed = false;

        float elapsedTime = 0f;
        float startZoom = _camera.orthographicSize;
        float endZoom;

        if ((_camera.orthographicSize - _zoomDelta) >= _minZoom)
        {
            endZoom = _camera.orthographicSize - _zoomDelta;
        }
        else
        {
            _isCanZoomed = true;
            yield break;
        }

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(startZoom, endZoom, elapsedTime / time);
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

using UnityEngine;

public class CameraWorldBounds : MonoBehaviour
{
    private Camera _camera;
    private Bounds _cameraBounds;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private float _height;
    private float _width;

    private void Awake()
    {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        Globals.WorldBounds = bounds;

        _camera = FindObjectOfType<Camera>();
    }

    private void Start()
    {
        RecalculateBounds();
    }

    private void LateUpdate()
    {
        _camera.transform.position = new Vector3
            (
            Mathf.Clamp(_camera.transform.position.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(_camera.transform.position.y, _cameraBounds.min.y, _cameraBounds.max.y),
            _camera.transform.position.z
            );
    }

    public void RecalculateBounds()
    {
        _height = _camera.orthographicSize;
        _width = _height * _camera.aspect;

        _minX = Globals.WorldBounds.min.x + _width;
        _maxX = Globals.WorldBounds.extents.x - _width;

        _minY = Globals.WorldBounds.min.y + _height;
        _maxY = Globals.WorldBounds.extents.y - _height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector2(_minX, _minY),
            new Vector2(_maxX + Globals.WorldBounds.center.x, _maxY + Globals.WorldBounds.center.y)
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



using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrashItem2D : MonoBehaviour
{
    [Header("Data")]
    public TrashCategory category;

    [Header("Drag Settings")]
    public float followSpeed = 20f;
    public bool isDraggable = true;

    private bool _dragging;
    private Vector3 _offset;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (!isDraggable) return;
        _dragging = true;
        var mouseWorld = _cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;
        _offset = transform.position - mouseWorld;
    }

    private void OnMouseDrag()
    {
        if (!_dragging) return;
        var target = _cam.ScreenToWorldPoint(Input.mousePosition) + _offset;
        target.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }

    private void OnMouseUp()
    {
        _dragging = false;
        // Let SortingManager decide if this was dropped correctly.
        //SortingManager.Instance.TryResolveDrop(this);
    }
}

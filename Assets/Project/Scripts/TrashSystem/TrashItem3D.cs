
// Assets/Project/Scripts/SortingSystem3D/TrashItem3D.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class TrashItem3D : MonoBehaviour
{
    [Header("Data")]
    public TrashCategory category;

    [Header("Drag Settings")]
    public bool isDraggable = true;
    public float dragSmooth = 15f;
    public float dragDepth = 0f; // Z depth or distance from camera plane

    private Camera _cam;
    private bool _dragging;
    private Vector3 _offset;
    private Rigidbody _rb;

    void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true; // manual movement
    }

    void OnMouseDown()
    {
        if (!isDraggable) return;

        // Raycast from mouse to pick point in world
        Plane plane = new Plane(_cam.transform.forward, transform.position);
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            _offset = transform.position - hitPoint;
            _dragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (!_dragging) return;

        Plane plane = new Plane(_cam.transform.forward, transform.position);
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 target = ray.GetPoint(enter) + _offset;
            // Maintain a fixed depth if you use front-view camera
            // Keep item on a fixed depth plane (for front-view orthographic camera)
            target.z = /* your 2D plane z, e.g. */ 0f;  // or the item’s starting z
            transform.position = Vector3.Lerp(transform.position, target, dragSmooth * Time.deltaTime);

        }
    }

    void OnMouseUp()
    {
        _dragging = false;
        SortingManager3D.Instance.TryResolveDrop(this);
    }
}

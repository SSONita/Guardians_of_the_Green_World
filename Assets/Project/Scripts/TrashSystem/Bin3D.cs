using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bin3D : MonoBehaviour
{
    [Header("Bin")]
    public TrashCategory accepts;

    [Header("Visuals")]
    public Renderer binRenderer;

    private readonly HashSet<TrashItem3D> _itemsInside = new HashSet<TrashItem3D>();

    void Awake()
    {
        if (!binRenderer) binRenderer = GetComponentInChildren<Renderer>();

        // Ensure this collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<TrashItem3D>();
        if (item != null)
        {
            _itemsInside.Add(item);
            Debug.Log($"[Bin3D] {name}: ENTER {item.name} (Item:{item.category}, Bin:{accepts})");
        }
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<TrashItem3D>();
        if (item != null)
        {
            _itemsInside.Remove(item);
            Debug.Log($"[Bin3D] {name}: EXIT {item.name} (Item:{item.category}, Bin:{accepts})");
        }
    }

    public bool IsItemOver(TrashItem3D item) => _itemsInside.Contains(item);
}

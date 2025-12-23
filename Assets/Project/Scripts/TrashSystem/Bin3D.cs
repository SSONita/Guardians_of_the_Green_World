using System.Collections;
using System.Collections.Generic;

// Assets/Project/Scripts/SortingSystem3D/Bin3D.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bin3D : MonoBehaviour
{
    [Header("Bin")]
    public TrashCategory accepts;

    [Header("Visuals")]
    public Color idleColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Renderer binRenderer;

    private readonly HashSet<TrashItem3D> _itemsInside = new HashSet<TrashItem3D>();

    void Awake()
    {
        if (!binRenderer) binRenderer = GetComponentInChildren<Renderer>();
        SetHover(false);

        // Ensure this collider is a trigger
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    var item = other.GetComponent<TrashItem3D>();
    //    if (item != null)
    //    {
    //        _itemsInside.Add(item);
    //        SetHover(true);
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    var item = other.GetComponent<TrashItem3D>();
    //    if (item != null)
    //    {
    //        _itemsInside.Remove(item);
    //        if (_itemsInside.Count == 0) SetHover(false);
    //    }
    //}

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<TrashItem3D>();
        if (item != null)
        {
            _itemsInside.Add(item);
            SetHover(true);
            SetHoverFor(item);
            Debug.Log($"[Bin3D] {name}: ENTER {item.name} (Item:{item.category}, Bin:{accepts})");
        }
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<TrashItem3D>();
        if (item != null)
        {
            _itemsInside.Remove(item);
            if (_itemsInside.Count == 0) SetHover(false);
            if (_itemsInside.Count == 0) binRenderer.material.color = idleColor;
            Debug.Log($"[Bin3D] {name}: EXIT {item.name} (Item:{item.category}, Bin:{accepts})");
        }
    }


    public bool IsItemOver(TrashItem3D item) => _itemsInside.Contains(item);

    private void SetHover(bool on)
    {
        if (binRenderer && binRenderer.material)
            binRenderer.material.color = on ? hoverColor : idleColor;
    }

    private void SetHoverFor(TrashItem3D item)
    {
        if (!binRenderer || !binRenderer.material) return;
        bool match = (item != null && item.category == accepts);
        binRenderer.material.color = match ? Color.green : hoverColor; // hoverColor = yellow
    }


}


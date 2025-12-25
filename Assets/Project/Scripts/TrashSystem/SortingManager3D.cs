using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SortingManager3D : MonoBehaviour
{
    public static SortingManager3D Instance { get; private set; }

    [Header("Scene")]
    public Bin3D[] bins;
    public Transform collectedContainer;  // optional: where sorted items go
    public Transform resetArea;           // optional: snap-back position for wrong drops

    [Header("Scoring")]
    public int totalItems;
    public int correctlySorted;
    public int incorrectlySorted;

    [Header("Audio (optional)")]
    public AudioSource sfxSource;
    public AudioClip sfxCorrect;
    public AudioClip sfxWrong;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void TryResolveDrop(TrashItem3D item)
    {

        Bin3D target = null;

        // In SortingManager3D.TryResolveDrop(TrashItem3D item) BEFORE the foreach(bins):
        // Try physics overlap to find any bin colliders near the item center
        Collider[] hits = Physics.OverlapSphere(item.transform.position, 0.25f);
        foreach (var h in hits)
        {
            var binHit = h.GetComponent<Bin3D>();
            if (binHit != null)
            {
                target = binHit;
                break;
            }
        }

        foreach (var bin in bins)
        {
            if (bin != null && bin.IsItemOver(item))
            {
                target = bin;
                break;
            }
        }

        if (target == null)
        {
            HandleWrong(item, "No bin under item.");
            return;
        }

        if (target.accepts == item.category)
            HandleCorrect(item, target);
        else
            HandleWrong(item, $"Wrong bin ({item.category} ? {target.accepts}).");
    }

    private void HandleCorrect(TrashItem3D item, Bin3D bin)
    {
        correctlySorted++;
        PlaySfx(sfxCorrect);

        // Lock item; move to collected container or keep inside bin
        item.isDraggable = false;
        var col = item.GetComponent<Collider>(); if (col) col.enabled = false;

        if (collectedContainer)
        {
            item.transform.SetParent(collectedContainer);
            item.transform.localPosition = Vector3.zero + Random.insideUnitSphere * 0.2f;
        }
        else
        {
            // Optional: snap into bin center
            item.transform.position = bin.transform.position + Vector3.up * 0.1f;
        }

        //Optional: destroy after short delay
        Destroy(item.gameObject, 0.25f);

        //UpdateUI();

        if (correctlySorted >= totalItems)
        {
            Debug.Log("[Sorting] All items sorted! Trigger success.");
            // Hook your EndingManager or PhaseController here
        }
    }


    private void HandleWrong(TrashItem3D item, string reason)
    {
        incorrectlySorted++;
        PlaySfx(sfxWrong);
        Debug.Log($"[Sorting] Incorrect drop: {reason}");

        item.transform.SetParent(null);
        // Snap back to reset area (or original spawn)
        if (resetArea)
        {
            item.transform.SetParent(null);
            item.transform.position = resetArea.position + new Vector3(
                Random.Range(-0.25f, 0.25f),
                Random.Range(-0.25f, 0.25f),
                0f
            );
        }
        else { item.transform.position = item.originalPosition; }

        //UpdateUI();
    }

    private void PlaySfx(AudioClip clip)
    {
        if (clip && sfxSource) sfxSource.PlayOneShot(clip);
    }

    //private void UpdateUI()
    //{
    //    var ui = FindObjectOfType<SortingUI>();
    //    if (ui) ui.Refresh(correctlySorted, incorrectlySorted, totalItems);
    //}
}


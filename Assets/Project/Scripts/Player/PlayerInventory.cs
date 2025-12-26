using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<GameObject> collectedTrash = new List<GameObject>();

    public void AddTrash(GameObject trash)
    {
        // Add to inventory list
        collectedTrash.Add(trash);
        Debug.Log("Collected trash: " + trash.name);

        // Mark collected so HUD doesn't subtract on destroy
        TrashCounterHook hook = trash.GetComponent<TrashCounterHook>();
        if (hook != null) hook.MarkCollected();

        // Notify HUD
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.AddTrash();
        }

        // Destroy trash object
        Destroy(trash);
    }

    public void SortTrash()
    {
        Dictionary<string, int> sorted = new Dictionary<string, int>();

        foreach (GameObject t in collectedTrash)
        {
            string key = t.name.Replace("(Clone)", "");
            if (!sorted.ContainsKey(key)) sorted[key] = 0;
            sorted[key]++;
        }

        foreach (var kvp in sorted)
        {
            Debug.Log("Trash type: " + kvp.Key + " count: " + kvp.Value);
        }
    }
}

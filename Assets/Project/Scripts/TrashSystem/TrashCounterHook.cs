using UnityEngine;

public class TrashCounterHook : MonoBehaviour
{
    private bool collectedSignalSent = false;

    // Called when the player collects this trash
    public void MarkCollected()
    {
        collectedSignalSent = true;
    }

    // Called automatically when the trash object is destroyed
    void OnDestroy()
    {
        // If destroyed without being collected, subtract from HUD total
        if (!collectedSignalSent && GameHUD.Instance != null)
        {
            GameHUD.Instance.RemoveTrash();
        }
    }
}

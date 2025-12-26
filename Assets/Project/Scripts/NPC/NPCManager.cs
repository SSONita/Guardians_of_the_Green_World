using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [Header("Assign all NPCs here")]
    public NPCMovement[] allNPCs;

    [Header("Trash settings")]
    public GameObject[] trashPrefabs; // assign trash prefabs once here

    void Start()
    {
        if (allNPCs == null || allNPCs.Length == 0)
        {
            Debug.LogWarning("NPCManager: No NPCs assigned!");
            return;
        }

        // Pick one random NPC
        int chosenIndex = Random.Range(0, allNPCs.Length);

        for (int i = 0; i < allNPCs.Length; i++)
        {
            NPCMovement npc = allNPCs[i];

            if (i == chosenIndex)
            {
                // This NPC becomes the polluter
                npc.isPolluter = true;
                npc.trashPrefabs = trashPrefabs;

                Debug.Log("Polluter chosen: " + npc.name);
            }
            else
            {
                // Others are normal walkers
                npc.isPolluter = false;
            }
        }
    }
}

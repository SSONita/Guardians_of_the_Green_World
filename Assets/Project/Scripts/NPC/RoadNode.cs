using UnityEngine;
using System.Collections.Generic;

public class RoadNode : MonoBehaviour
{
    public List<RoadNode> connectedNodes; // next possible nodes
    void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawSphere(transform.position, 0.2f);

    if (connectedNodes == null) return;

    Gizmos.color = Color.green;
    foreach (var node in connectedNodes)
    {
        Gizmos.DrawLine(transform.position, node.transform.position);
    }
}
}
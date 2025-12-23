using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NPCMovement : MonoBehaviour
{
    [Header("Road following")]
    public RoadNode currentNode;   // Assign starting node in Inspector
    public float speed = 1f;       // NPC walking speed

    [Header("Gravity settings")]
    public float gravity = -9.81f; // Same as Player
    private Vector3 velocity;      // Track vertical velocity

    [Header("Interaction settings")]
    public Transform player;          // Assign your Player in Inspector
    public float interactRange = 5f;  // How close player must be
    public KeyCode interactKey = KeyCode.E;
    public float faceTurnSpeed = 5f;  // Rotation speed when facing player
    public float resumeBuffer = 0.5f; // Extra distance to resume after leaving range

    private bool isInteracting = false;

    private RoadNode targetNode;
    private CharacterController controller;
    private Animator animator;  

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ChooseNextNode();
    }

    void Update()
    {
        // --- Gravity handling (ALWAYS run) ---
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            // Small offset to avoid hover; tune as needed
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.05f,
                transform.position.z
            );
        }
        velocity.y += gravity * Time.deltaTime;
        
        bool isWalking = (speed > 0);
        animator.SetBool("isWalking", isWalking);

        // --- Interaction check ---
        if (player != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            if (Input.GetKeyDown(interactKey) && distToPlayer <= interactRange)
            {
                isInteracting = true;
            }

            if (isInteracting && distToPlayer > interactRange + resumeBuffer)
            {
                isInteracting = false;
            }
        }

        // --- Behavior when interacting ---
        if (isInteracting)
        {
            Vector3 lookDir = (player.position - transform.position);
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, faceTurnSpeed * Time.deltaTime);
            }

            // Apply gravity only while interacting
            controller.Move(velocity * Time.deltaTime);
            return; // skip walking
        }


        // --- Normal node-following movement ---
        if (targetNode == null) return;

        Vector3 targetPos = targetNode.transform.position;
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;
        dir = dir.normalized;

        Vector3 horizontalMove = dir * speed;

        // Apply movement + gravity
        controller.Move((horizontalMove + velocity) * Time.deltaTime);

        // Face the direction of travel
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        // Arrival check
        float distXZ = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(targetPos.x, targetPos.z)
        );

        if (distXZ < 0.3f)
        {
            currentNode = targetNode;
            ChooseNextNode();
        }
    }

    void ChooseNextNode()
    {
        if (currentNode == null || currentNode.connectedNodes == null || currentNode.connectedNodes.Count == 0)
        {
            targetNode = null;
            return;
        }

        int index = Random.Range(0, currentNode.connectedNodes.Count);
        targetNode = currentNode.connectedNodes[index];
    }

    public void Interact(Transform playerTransform)
    {
        isInteracting = true;
        player = playerTransform;
    }
}

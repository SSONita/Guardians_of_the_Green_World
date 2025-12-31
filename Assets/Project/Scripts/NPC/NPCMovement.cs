using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class NPCMovement : MonoBehaviour
{
    [Header("Road following")]
    public RoadNode currentNode;
    public float speed = 1f;

    [Header("Gravity settings")]
    public float gravity = -9.81f;
    private Vector3 velocity;

    [Header("Interaction settings")]
    public Transform player;
    public float interactRange = 5f;
    public KeyCode interactKey = KeyCode.E;
    public float faceTurnSpeed = 5f;
    public float resumeBuffer = 0.5f;

    [Header("Polluter settings")]
    public bool isPolluter = false;               // Manager sets this
    [HideInInspector] public GameObject[] trashPrefabs; // Manager injects shared list
    public Image exclamationMark;
    public float trashInterval = 15f;

    private bool hasBeenConfronted = false;
    private bool isInteracting = false;
    private RoadNode targetNode;
    private CharacterController controller;
    private Animator animator;
    private ConversationManager conversationManager;
    private Coroutine polluteRoutine;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        conversationManager = FindObjectOfType<ConversationManager>();
        ChooseNextNode();

        if (isPolluter && trashPrefabs != null && trashPrefabs.Length > 0)
        {
            polluteRoutine = StartCoroutine(PolluteLoop());
        }
    }

    void Update()
    {
        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y - 0.05f,
                transform.position.z
            );
        }
        velocity.y += gravity * Time.deltaTime;

        animator.SetBool("isWalking", !isInteracting && speed > 0);

        // Interaction check (optional if you want NPC to self-check)
        if (isPolluter && !hasBeenConfronted && player != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.position);
            if (Input.GetKeyDown(interactKey) && distToPlayer <= interactRange)
            {
                StartCoroutine(ConversationSequence());
            }
        }

        // If interacting, face player and pause walking
        if (isInteracting)
        {
            Vector3 lookDir = (player.position - transform.position);
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, faceTurnSpeed * Time.deltaTime);
            }

            controller.Move(velocity * Time.deltaTime);
            return;
        }

        // Normal road-node walking
        if (targetNode == null) return;

        Vector3 targetPos = targetNode.transform.position;
        Vector3 dir = (targetPos - transform.position);
        dir.y = 0;
        dir = dir.normalized;

        Vector3 horizontalMove = dir * speed;
        controller.Move((horizontalMove + velocity) * Time.deltaTime);

        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

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

    // Trash throwing loop
    private IEnumerator PolluteLoop()
    {
        while (isPolluter && !hasBeenConfronted)
        {
            yield return new WaitForSeconds(trashInterval);

            Debug.Log("Polluter " + name + " spawning trash...");

            if (trashPrefabs != null && trashPrefabs.Length > 0)
            {
                GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
                Vector3 spawnPos = transform.position - transform.forward * 1.5f + Vector3.up * 0.2f;
                //Instantiate(prefab, spawnPos, Quaternion.identity);
                GameObject spawned = Instantiate(prefab, spawnPos, Quaternion.identity);

                // Notify HUD
                if (GameHUD.Instance != null)
                {
                    GameHUD.Instance.RegisterNewTrash();
                }

            }

            if (exclamationMark != null)
            {
                exclamationMark.gameObject.SetActive(true);
                StartCoroutine(HideExclamation());
            }

        }
    }
    private IEnumerator HideExclamation()
    {
        yield return new WaitForSeconds(2f); // show for 2 seconds
        exclamationMark.gameObject.SetActive(false);
    }

    private IEnumerator ConversationSequence()
    {
        isInteracting = true;
        hasBeenConfronted = true;

        if (conversationManager != null)
        {
            conversationManager.ShowConversation(
                "Player: Throwing trash harms our world.\n" +
                "Polluter: I didn’t realize… I’ll stop now."
            );
        }

        yield return new WaitForSeconds(3f);

        if (conversationManager != null)
            conversationManager.HideConversation();

        // Stop polluting permanently
        isPolluter = false;
        if (polluteRoutine != null) StopCoroutine(polluteRoutine);

        // ✅ Tell HUD the polluter was confronted
        if (GameHUD.Instance != null)
        {
            GameHUD.Instance.ConfrontPolluter();
        }

        // Resume walking
        isInteracting = false;

        // Broadcast event (optional if you want other systems to listen)
        GameEvents.OnPolluterStopped?.Invoke();
    }


    public void Interact(Transform playerTransform)
    {
        isInteracting = true;
        player = playerTransform;

        if (isPolluter && !hasBeenConfronted)
        {
            StartCoroutine(ConversationSequence());
        }
        else
        {
            isInteracting = false;
        }
    }
}

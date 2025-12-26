using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public Transform cameraTransform;
    public float gravity = -9.81f;   // Gravity strength
    public float jumpHeight = 1.5f; // Optional jump
    private Animator animator;  

    private CharacterController controller;
    private float verticalRotation = 0f;
    private Vector3 velocity;       // Track vertical velocity
    private PlayerInventory inventory;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 
        inventory = GetComponent<PlayerInventory>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            PlayerInventory inventory = GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddTrash(other.gameObject);

                // Safely destroy prefab root
                if (other.transform.parent != null)
                {
                    Destroy(other.transform.parent.gameObject);
                }
                else
                {
                    Destroy(other.gameObject);
                }

                Debug.Log("Picked up trash via trigger: " + other.name);
            }
            else
            {
                Debug.LogError("PlayerInventory component missing on Player!");
            }
        }
    }


    void Update()
    {
        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Update Animator
        bool isWalking = (x != 0 || z != 0);
        animator.SetBool("isWalking", isWalking);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Camera look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Inside Update() of PlayerMovement
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed, casting ray...");
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
            RaycastHit hit;

            int npcLayerMask = LayerMask.GetMask("NPC");
            if (Physics.Raycast(ray, out hit, 20f, npcLayerMask)) // 5 units interact range
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                NPCMovement npc = hit.collider.GetComponent<NPCMovement>();
                if (npc != null)
                {
                    Debug.Log("Interacting with NPC: " + npc.name);
                    npc.Interact(transform); // tell NPC to face player
                }
            }
        }else { Debug.Log("Raycast did not hit anything."); }

    }
}

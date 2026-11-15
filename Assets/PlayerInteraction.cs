using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    [Header("Input Settings")]
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private string actionMapName = "Player";
    [SerializeField] private string attackActionName = "Attack";
    private InputAction attackAction;

    // Reference to the Animator
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (inputActions != null)
        {
            // Uses the name set in the Inspector (defaults to "Player")
            var actionMap = inputActions.FindActionMap(actionMapName, false);
            if (actionMap != null)
            {
                attackAction = actionMap.FindAction(attackActionName, false);
            }
            else
            {
                Debug.LogWarning($"[PlayerInteraction] Could not find Action Map '{actionMapName}'. Falling back to Left Mouse Click.");
            }
        }
    }

    void OnEnable()
    {
        attackAction?.Enable();
    }

    void OnDisable()
    {
        attackAction?.Disable();
    }

    void Update()
    {
        // Triggers on either your Input Action OR a standard Left Mouse Click
        bool attackPressed = (attackAction != null && attackAction.WasPressedThisFrame()) || Input.GetMouseButtonDown(0);

        if (attackPressed)
        {
            // 1. Trigger the punching animation immediately
            if (anim != null)
            {
                anim.SetTrigger("Attack");
            }

            // 2. Check for hits in front of the player
            PerformRaycastInteraction();
        }
    }

    private void PerformRaycastInteraction()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        Vector3 rayDirection = transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionRange, targetLayerMask))
        {
            Debug.Log($"[Interaction] Successfully hit target: {hit.collider.gameObject.name}", hit.collider.gameObject);

            // Play hit sound effect
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            // Trigger visual hit flash on target
            HitFlashController flashController = hit.collider.GetComponent<HitFlashController>();
            if (flashController != null)
            {
                flashController.TriggerFlash();
            }

            // Increment combo counter
            ComboManager comboManager = GetComponent<ComboManager>();
            if (comboManager != null)
            {
                comboManager.AddHit();
            }
        }
        else
        {
            Debug.Log("[Interaction] Raycast fired, but hit nothing within range.");
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOrigin, transform.forward * interactionRange);
    }
}
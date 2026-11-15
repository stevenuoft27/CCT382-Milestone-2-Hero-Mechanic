using UnityEngine;
using UnityEngine.Events;

public class DamageTrigger : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The Health ScriptableObject asset to modify.")]
    [SerializeField] private Health health;

    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 20f;

    [Header("Events")]
    [Tooltip("Invoked when the player triggers the volume (wired in Inspector to UI, screen flash, etc.).")]
    public UnityEvent onDamageTaken;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the Player
        if (other.CompareTag("Player"))
        {
            if (health != null)
            {
                // Subtract health and clamp at zero
                health.Subtract(damageAmount);
            }

            // Fire the UnityEvent for simple visual/audio effects
            onDamageTaken?.Invoke();
        }
    }
}
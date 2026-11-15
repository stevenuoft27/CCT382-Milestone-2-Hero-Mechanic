using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LAB 8 - GOAL 2 & GOAL 3
///
/// What this script does, in order, every time the game runs:
///   1. Register itself with a shared GameEventSO, so it gets called
///      automatically whenever health changes - without the script that
///      changed it ever needing to know this one exists (Goal 2).
///   2. Whenever that happens, read two shared numbers (current health and
///      max health) and turn them into a 0-1 ratio the Slider understands
///      (Goal 2).
///   3. Also play a short sound at that exact moment, so the player hears
///      the hit as well as seeing the bar move (Goal 3).
/// </summary>
public class HealthBarUI : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The current value to display, as a fraction of Max Health.")]
    public FloatVariableSO currentHealth;
    public FloatVariableSO maxHealth;

    [Tooltip("The event this script listens for. Raised by whatever changes currentHealth.")]
    public GameEventSO onHealthChanged;

    [Header("UI")]
    public Slider slider;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip changeSound;

    private void OnEnable()
    {
        // Register the event listener when active
        onHealthChanged.RegisterListener(OnHealthChangedEvent);

        // Sync once immediately at startup so the slider doesn't show a stale value
        SyncSlider();
    }

    private void OnDisable()
    {
        // Unregister the event listener when disabled to prevent errors
        onHealthChanged.UnregisterListener(OnHealthChangedEvent);
    }

    // Called automatically by onHealthChanged whenever it's Raised()
    private void OnHealthChangedEvent()
    {
        SyncSlider();

        // Play the hit/change sound effect once
        if (audioSource != null && changeSound != null)
        {
            audioSource.PlayOneShot(changeSound);
        }
    }

    private void SyncSlider()
    {
        if (slider == null || maxHealth == null || maxHealth.value <= 0f) return;

        // Calculate current health as a 0-1 fraction ratio for the Slider
        slider.value = currentHealth.value / maxHealth.value;
    }
}
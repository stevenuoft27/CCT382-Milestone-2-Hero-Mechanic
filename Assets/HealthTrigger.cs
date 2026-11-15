using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UPDATED FOR LAB 8 - this version drops HealthSO entirely. From Lab 8
/// onward, healthValue (FloatVariableSO) is the single source of truth
/// for this trigger's target - not a mirror running alongside HealthSO.
///
/// If you're looking at this file for Part 4 (Lab 5) specifically: the
/// Health field you may remember from that lab has been replaced. See
/// the Master Setup Guide's Part 7 for why - in short, HealthSO's own
/// job was to teach the "data + method on one ScriptableObject" pattern
/// once, during Lab 5 itself. That lesson doesn't need a second, parallel
/// copy of the same number running forever afterward, so Lab 8 retires it
/// rather than keeping both in sync.
///
/// Sits on a trigger volume in the scene. When the player enters it, this
/// script lowers a shared value - it never touches the player's own
/// script, and the value holder never knows this trigger exists.
///
/// The optional UnityEvent (onTriggered) is Lab 5's stretch goal: a public
/// slot other objects can wire a reaction into from the Inspector, no
/// extra script required - the same mechanism as a UI Button's OnClick().
///
/// onHealthChanged (GameEventSO) is Lab 8's addition: it announces the
/// change so listeners like HealthBarUI can react without ever
/// referencing this script directly.
/// </summary>
public class HealthTrigger : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("The shared value this trigger lowers (e.g. PlayerHealthValue).")]
    public FloatVariableSO healthValue;

    [Tooltip("How much to subtract from healthValue when the player enters.")]
    public float damageAmount = 10f;

    [Header("Player Detection")]
    [Tooltip("Must match the Tag on your player GameObject exactly.")]
    public string playerTag = "Player";

    [Header("Stretch Goal: Reaction (optional)")]
    [Tooltip("Wire a response here in the Inspector - e.g. a UI element's SetActive(true), or an AudioSource's Play(). Leave empty to skip. Note: the dropdown only lists methods on components attached to the object you drag in, so static calls like Debug.Log won't appear unless you add a small script exposing them.")]
    public UnityEvent onTriggered;

    [Header("Lab 8: Broadcasting the Change")]
    [Tooltip("Raised after healthValue changes, so listeners (e.g. HealthBarUI) can react without a direct reference to this trigger.")]
    public GameEventSO onHealthChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (healthValue == null)
        {
            Debug.LogWarning($"{name}: No FloatVariableSO assigned in the Health Value field - nothing to subtract from.");
            return;
        }

        healthValue.value = Mathf.Max(healthValue.value - damageAmount, 0f);
        onTriggered?.Invoke();
        onHealthChanged?.Raise();
    }
}

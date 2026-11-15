using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    [Header("Combo Settings")]
    [Tooltip("Time window allowed before the combo resets.")]
    [SerializeField] private float comboResetTime = 1.5f;
    private int currentCombo = 0;
    private float comboTimer = 0f;

    [Header("UI Reference")]
    [Tooltip("Reference to the TextMeshPro UI element displaying the combo.")]
    [SerializeField] private TextMeshProUGUI comboText;

    public int CurrentCombo => currentCombo;

    void Update()
    {
        // Countdown the combo window if a combo is active
        if (currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                ResetCombo();
            }
        }
    }

    public void AddHit()
    {
        currentCombo++;
        comboTimer = comboResetTime; // Reset the window timer on every successful hit
        UpdateUI();
    }

    private void ResetCombo()
    {
        currentCombo = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (comboText != null)
        {
            if (currentCombo > 1)
            {
                comboText.gameObject.SetActive(true);
                comboText.text = $"{currentCombo} HITS!";
            }
            else
            {
                // Hide the counter if there's no active combo (0 or 1 hit)
                comboText.gameObject.SetActive(false);
            }
        }
    }
}
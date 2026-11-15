using UnityEngine;
using TMPro;

public class UITextFlash : MonoBehaviour
{
    [SerializeField] private Health health;
    private TextMeshProUGUI healthText;

    private void Awake()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (health != null && healthText != null)
        {
            healthText.text = $"Health: {health.CurrentHealth}";
        }
    }
}
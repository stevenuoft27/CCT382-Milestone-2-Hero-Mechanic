using UnityEngine;

[CreateAssetMenu(fileName = "HealthSO", menuName = "ScriptableObjects/HealthSO")]
public class Health : ScriptableObject
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    private void OnEnable()
    {
        // Reset health when the scriptable object loads or game starts
        currentHealth = maxHealth;
    }

    public void Subtract(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f); // Lowers and clamps at zero

        Debug.Log($"Health reduced by {amount}. Current Health: {currentHealth}");
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
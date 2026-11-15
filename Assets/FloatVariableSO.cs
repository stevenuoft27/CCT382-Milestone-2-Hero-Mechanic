using UnityEngine;

[CreateAssetMenu(fileName = "NewFloatVariable", menuName = "Game Data/Float Variable")]
public class FloatVariableSO : ScriptableObject
{
    [Header("Reset Settings")]
    [Tooltip("The value this variable starts with when entering Play mode.")]
    [SerializeField] private float initialValue = 100f;

    [Tooltip("The current value. Any script can read or write this directly.")]
    public float value;

    // This runs automatically when the game starts or the asset loads
    private void OnEnable()
    {
        value = initialValue;
    }
}
using UnityEngine;
using System.Collections;

public class HitFlashController : MonoBehaviour
{
    private Material targetMaterial;
    private Coroutine flashCoroutine;

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            // Instantiates a unique material so flashing one object doesn't affect others
            targetMaterial = renderer.material;
        }
    }

    public void TriggerFlash()
    {
        if (targetMaterial == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine(0.2f));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        float elapsedTime = 0f;
        targetMaterial.SetFloat("_FlashAmount", 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            targetMaterial.SetFloat("_FlashAmount", Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        targetMaterial.SetFloat("_FlashAmount", 0f);
        flashCoroutine = null;
    }
}
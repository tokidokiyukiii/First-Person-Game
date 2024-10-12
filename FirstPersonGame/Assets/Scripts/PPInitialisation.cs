using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Volume globalVolume; // Reference to your Global Volume
    private Vignette vignette;

    void Start()
    {
        // Check if the global volume is assigned and initialize the vignette
        if (globalVolume.profile.TryGet<Vignette>(out vignette))
        {
            // Set the initial intensity if needed
            vignette.intensity.value = 0.4f; // Example value
        }
        else
        {
            Debug.LogError("Vignette component not found in the volume profile!");
        }
    }

    public void TransitionToGameScene()
    {
        // Example transition logic
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        // Example to increase vignette intensity over time
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(0.4f, 1f, t); // Increase intensity
            yield return null;
        }

        // Load the new scene here
        // SceneManager.LoadScene("YourGameSceneName");

        // Example to decrease vignette intensity after scene load
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            vignette.intensity.value = Mathf.Lerp(1f, 0.4f, t); // Decrease intensity
            yield return null;
        }
    }
}
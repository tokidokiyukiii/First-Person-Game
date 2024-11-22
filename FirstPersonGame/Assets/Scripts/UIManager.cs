using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera (assign in the Inspector)
    public float rotationSpeed = 1f; // Speed at which the camera rotates
    private bool isRotating = false; // Track if the camera is currently rotating
    public GameObject[] UIElements;
    public GameObject initialButton;
    
    public bool isGame = false;

    public FirstPersonControls firstPersonControls;
    public LevelLoader levelLoader;
    //public int levelToLoad = 1;

    void Start()
    {
        if (isGame)
            Cursor.visible = false;
    }
    
    // Method to rotate the camera left by 90 degrees
    public void RotateCameraLeftBy90Degrees()
    {
        if (!isRotating) // Prevent triggering multiple rotations simultaneously
        {
            StartCoroutine(RotateCameraCoroutine(90f));
        }
    }

    public void LoadScene(int levelToLoad)
    {
        levelLoader.LoadNextLevel(levelToLoad);
        //SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    // Coroutine to smoothly rotate the camera
    private IEnumerator RotateCameraCoroutine(float angle)
    {
        isRotating = true;
        Quaternion startRotation = mainCamera.transform.rotation; // Initial rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -angle, 0); //Target rotation
        float rotationProgress = 0f;
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / angle); //Normalize the rotation speed based on angle
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation,
                endRotation, rotationProgress); // Smoothly interpolate rotation
            yield return null;
        }
        mainCamera.transform.rotation = endRotation; // Ensure exact final rotation
        isRotating = false;
        initialButton.SetActive(false);
        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
    }

    public void ClosePause()
    {
        firstPersonControls.TogglePause();
    }
}


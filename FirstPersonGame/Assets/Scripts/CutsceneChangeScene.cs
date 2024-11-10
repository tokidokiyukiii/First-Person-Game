using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneChangeScene : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public string sceneName; // Name of the scene to load when video finishes

    void Start()
    {
        // Check if the VideoPlayer is assigned
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Subscribe to the loopPointReached event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // Load the specified scene when the video ends
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}

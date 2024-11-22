using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 5f;
    public Transform player;
    public FirstPersonControls firstPersonControls;
    public CharacterController controller;
    public GameObject imgTransition;

    public bool playsVideo = false;
    public VideoPlayer videoPlayer;
    public EnemyAI enemyAI;
    public GameObject cutscene;
    public GameObject canvas;
    public GameObject audioManager;
    public GameObject finalPhase;
    public bool isGame = false;
    public bool isPlaying;
    public bool isCredits = false;
    
    private void Start()
    {
        if (controller != null)
            controller = player.GetComponent<CharacterController>();
        
        if (playsVideo)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            Debug.Log("Subscribed to loopPointReached");
            videoPlayer.prepareCompleted += OnVideoPrepared;
            
            videoPlayer.isLooping = false;

            // Start preparing the video
        
            videoPlayer.Prepare();
            Debug.Log("Preparing video...");
        }
    }

    private void Update()
    {
        //Debug.Log($"Video time: {videoPlayer.time}/{videoPlayer.length}");
        if (playsVideo)
        {
            if (videoPlayer.isPlaying && videoPlayer.time >= videoPlayer.length - 0.03f) // Allow a 0.1s threshold
            {
                Debug.Log("Video close to end. Manually invoking end behavior.");
                OnVideoEnd(videoPlayer);
            }
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video is prepared, starting playback...");
        videoPlayer.Play();
    }
    
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished!");
        //firstPersonControlls.enabled = true;
        if (isGame)
        {
            //firstPersonControls.enabled = true;
            controller.enabled = true;
            firstPersonControls.isInputEnabled = true;
            enemyAI.canEnemyMove = true;
            Debug.Log("Enemy can move");
        }

        if (isCredits)
            Cursor.visible = true;
        //levelLoaderObject.SetActive(true);
        Debug.Log("Video end");
        StopTransition(player);

        if (isGame)
        {
            cutscene.SetActive(false);
            audioManager.SetActive(true);
            canvas.SetActive(true);
            finalPhase.SetActive(true);
            //playsVideo = false;
        }

        isPlaying = false;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
        //videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    public void LoadNextLevel(int buildNum)
    {
        //SceneManager.LoadScene(buildNum);
        StartCoroutine(LoadLevel(buildNum));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

    public void InGameTransition(Transform waypoint)
    {
        //player.position = waypoint.position;
        StartCoroutine(MovePlayer(waypoint));
    }

    IEnumerator MovePlayer(Transform waypoint)
    {
        transition.SetTrigger("Start");
        
        //CharacterController controller = player.GetComponent<CharacterController>();
        //firstPersonControls.isInputEnabled = false;
        //controller.enabled = false;

        yield return new WaitForSeconds(transitionTime);

        if (isGame)
        {
            firstPersonControls.isInputEnabled = false;
            controller.enabled = false;
            enemyAI.canEnemyMove = false;
            Debug.Log("Enemy can't move");
        }
        
        if (playsVideo)
        {
            audioManager.SetActive(false);
            cutscene.SetActive(true);
            canvas.SetActive(false);
            
            Debug.Log("Video is about to play!");
            videoPlayer.time = 0f;
            videoPlayer.Play();
            imgTransition.SetActive(false);
            isPlaying = true;
            Debug.Log("Video state: " + videoPlayer.isPlaying);
        }
        else
        {
            StopTransition(waypoint);
        }

        //player.position = waypoint.position;
        //controller.enabled = true;
        //firstPersonControls.isInputEnabled = true;
    }

    public void StopTransition(Transform waypoint)
    {
        StartCoroutine(EndTransition(waypoint));
    }

    IEnumerator EndTransition(Transform waypoint)
    {
        if (playsVideo)
        {
            imgTransition.SetActive(true);
            playsVideo = false;
        }

        transition.SetTrigger("End");
        if (isGame)
            player.position = waypoint.position;
        
        yield return new WaitForSeconds(transitionTime);

        if (isGame)
        {
            controller.enabled = true;
            firstPersonControls.isInputEnabled = true;
            enemyAI.canEnemyMove = true;
        }
    }
}

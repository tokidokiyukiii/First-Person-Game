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
    
    private void Start()
    {
        controller = player.GetComponent<CharacterController>();
        
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // Start preparing the video
        videoPlayer.Prepare();
        Debug.Log("Preparing video...");
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
        firstPersonControls.isInputEnabled = true;
        enemyAI.canEnemyMove = true;
        
        //levelLoaderObject.SetActive(true);
        StopTransition(player);
        
        cutscene.SetActive(false);
        canvas.SetActive(true);
        playsVideo = false;
    }
    
    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.prepareCompleted -= OnVideoPrepared;
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
        
        CharacterController controller = player.GetComponent<CharacterController>();
        firstPersonControls.isInputEnabled = false;
        controller.enabled = false;

        yield return new WaitForSeconds(transitionTime);

        if (playsVideo)
        {
            firstPersonControls.isInputEnabled = false;
            enemyAI.canEnemyMove = false;
            
            cutscene.SetActive(true);
            canvas.SetActive(false);
            
            Debug.Log("Video is about to play!");
            videoPlayer.time = 0;
            videoPlayer.Play();
            Debug.Log("Video state: " + videoPlayer.isPlaying);
        }
        else
        {
            StopTransition(waypoint);
        }
        
        imgTransition.SetActive(false);

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
        imgTransition.SetActive(true);
        player.position = waypoint.position;
        transition.SetTrigger("End");
        
        yield return new WaitForSeconds(transitionTime);
        
        firstPersonControls.isInputEnabled = true;
        controller.enabled = true;
    }
}

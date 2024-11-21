using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Video;

public class ThoughtCount : MonoBehaviour
{
    public int thoughtCount = 0;
    public int secondPhaseThoughtCount = 0;
    public int finalPhaseThoughtCount = 0;
    public int thoughtTotal = 5;

    public TextMeshProUGUI phaseText;
    private int phaseCount = 1;
    public TextMeshProUGUI thoughtCountText;
    
    public TextMeshProUGUI keyText;

    public GameObject key;
    public GameObject glowingKey;

    public GameObject Thoughts2;
    public GameObject Thoughts3;
    public GameObject Thoughts4;
    public GameObject Thoughts5;

    public GameObject GlowingThoughts2;
    public GameObject GlowingThoughts3;
    public GameObject GlowingThoughts4;
    public GameObject GlowingThoughts5;

    public EnemyAI enemyAI;
    public Door towerDoor;

    public FirstPersonControls firstPersonControlls;

    public GameObject normalViewVolume;
    public GameObject secondViewVolume;
    public GameObject finalNormalViewVolume;
    public GameObject FinalSecondViewVolume;

    public bool collectedThought1;
    public bool collectedThought2;

    public TextMeshProUGUI sprintText;

    public GameObject SoundThree;
    public GameObject SoundFour;

    public GameObject cutscene;
    public VideoPlayer videoPlayer;
    public GameObject canvas;

    private void Start()
    {
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
        firstPersonControlls.enabled = true;
        enemyAI.canEnemyMove = true;
        
        cutscene.SetActive(false);
        canvas.SetActive(true);
    }
    
    void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPlayer.prepareCompleted -= OnVideoPrepared;
    }

    public void AddThought()
    {
        if (phaseCount == 2)
            ++secondPhaseThoughtCount;
        if (phaseCount == 3)
            ++finalPhaseThoughtCount;
        thoughtCount++;

        if (thoughtCount >= thoughtTotal)
        {
            //firstPersonControlls.enabled = false;
            enemyAI.canEnemyMove = false;
            
            cutscene.SetActive(true);
            canvas.SetActive(false);
            
            Debug.Log("Video is about to play!");
            videoPlayer.time = 0;
            videoPlayer.Play();
            Debug.Log("Video state: " + videoPlayer.isPlaying);
            
            key.SetActive(true);
            glowingKey.SetActive(true);
            ShowMessage("The key is in the attic. Be quick...");
            
            enemyAI.isKeyActive = true;
            firstPersonControlls.canSprint = true;
            
            //StartCoroutine(ActivateSprintText());
        }

        switch (thoughtCount)
        {
            case 1:
                if (!collectedThought1 && !collectedThought2)
                {
                    Thoughts2.SetActive(true);
                    GlowingThoughts2.SetActive(true);
                }
                break;
            case 2:
                if (!collectedThought1 || !collectedThought2)
                {
                    Thoughts2.SetActive(true);
                    GlowingThoughts2.SetActive(true);
                }
                break;
            case 3:
                Thoughts2.SetActive(true);
                GlowingThoughts2.SetActive(true);

                SoundThree.SetActive(true);
                SoundFour.SetActive(true);
                
                if (!collectedThought1 && !collectedThought2)
                {
                    towerDoor.needsKey = false;
                    towerDoor.ToggleDoor();
                }
                break;
            case 4:
                if (!collectedThought1 || !collectedThought2)
                {
                    towerDoor.needsKey = false;
                    towerDoor.ToggleDoor();
                }
                break;
            case 5:
                phaseText.text = "PHASE 2";
                phaseCount = 2;

                towerDoor.needsKey = false;
                towerDoor.ToggleDoor();
                
                enemyAI.gameObject.SetActive(true);
                enemyAI.canEnemyMove = true;
                enemyAI.animator.enabled = true;
                
                enemyAI.audioSource.PlayOneShot(enemyAI.laughingSpawn);
                enemyAI.PlayHumming();
                break;
            case 6:
                Thoughts3.SetActive(true);
                GlowingThoughts3.SetActive(true);
                break;
            case 9:
                Thoughts4.SetActive(true);
                GlowingThoughts4.SetActive(true);
                break;
            case 10:
                phaseText.text = "FINAL PHASE";
                phaseCount = 3;
                
                if (firstPersonControlls.isNormalView)
                {
                    normalViewVolume.SetActive(false);
                    finalNormalViewVolume.SetActive(true);
                }
                else
                {
                    secondViewVolume.SetActive(false);
                    FinalSecondViewVolume.SetActive(true);
                }
                firstPersonControlls.finalPhase = true;
                break;
            case 12:
                Thoughts5.SetActive(true);
                GlowingThoughts5.SetActive(true);
                break;
        }

        UpdateUI(); 
    }


    public void ShowMessage(string message)
    {
        thoughtCountText.gameObject.SetActive(false);
        phaseText.gameObject.SetActive(false);
        keyText.gameObject.SetActive(true);
        //countPanel.SetActive(false);
        keyText.text = message; 

        // StartCoroutine(HideMessageAfterDelay(2f));
    }

    private void UpdateUI()
    {
        if (thoughtCountText != null)
        {
            if (phaseCount == 1)
            {
                thoughtCountText.text = "Thoughts: " + thoughtCount + " / 5";
            }
            else if (phaseCount == 2)
            {
                thoughtCountText.text = "Thoughts: " + secondPhaseThoughtCount + " / 5";
            }
            else if (phaseCount == 3)
            {
                thoughtCountText.text = "Thoughts: " + finalPhaseThoughtCount + " / 5";
            }
        }
    }

    private IEnumerator ActivateSprintText()
    {
        firstPersonControlls.isShowingMessage = true;
        
        sprintText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);

        firstPersonControlls.isShowingMessage = false;
    }

    /*private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        messagePanel.SetActive(false); // Hide the message panel
    }*/

}

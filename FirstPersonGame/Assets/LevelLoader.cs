using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 5f;
    public Transform player;
    public FirstPersonControls firstPersonControls;

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
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        player.position = waypoint.position;
        controller.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{

    public void LoadSceneByName() 
    {
        SceneManager.LoadScene("Scenes/Cutscene1");
    }

    public void LoadNextInBuild() 
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }


}

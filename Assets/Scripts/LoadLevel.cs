using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadLevel : MonoBehaviour
{
    public string sceneToLoad;

    public void loadScene()
    {
        PlayerPrefs.SetString("stageToLoad", sceneToLoad);
        SceneManager.LoadScene("loading");
    }


}
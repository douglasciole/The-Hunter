using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour {
    
    int changeSceneDelay = 1;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.GetString("stageToLoad") != "")
        {
            loadScene(PlayerPrefs.GetString("stageToLoad"));
        }else
        {
            loadScene("titulo");
        }
    }

    public void loadScene(string sceneToLoad)
    {
        PlayerPrefs.SetString("stageToLoad", "");
        StartCoroutine(WaitForLoad(sceneToLoad)); // the time we will wait before to load the scene.
    }
    public IEnumerator WaitForLoad(string sceneToLoad)
    {
        yield return new WaitForSeconds(changeSceneDelay);
        SceneManager.LoadSceneAsync(sceneToLoad); // From here we load the scene that we have previously indicated in the editor.

    }

}

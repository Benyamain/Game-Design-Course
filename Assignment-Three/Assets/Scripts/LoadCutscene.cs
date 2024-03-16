using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCutscene : MonoBehaviour
{
    private void Start()
    {
       if (!GameManager.WasCutsceneLoaded) {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(GameManager.LoadCutscene);
            GameManager.WasCutsceneLoaded = true;
       }
    }
}
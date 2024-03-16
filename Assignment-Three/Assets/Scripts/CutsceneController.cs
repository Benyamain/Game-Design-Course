using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEditor;

public class CutsceneController : MonoBehaviour
{
    // Put this on main camera
    
    public PlayableDirector director;
    public GameObject blackScreen;
    // private float timer = 0f;

    private void Awake() {
        blackScreen = GameObject.FindGameObjectWithTag("Black Screen");
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        // blackScreen.SetActive(false);
        director.played += OnTimelineStarted;
        director.stopped += OnTimelineStopped;
    }

    // Update is called once per frame
    private void Update()
    {
        director.Play();

        // timer += Time.deltaTime;
        
        // if (timer > 0.70f) {
        //     blackScreen.SetActive(!blackScreen.activeSelf);
        //     timer = 0f;
        // }
    }

    private void OnTimelineStarted(PlayableDirector director)
    {
        EditorApplication.isPlaying = true;
        
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        // blackScreen.SetActive(false);
        // Load another scene only if it's not already loaded
        GameManager.WasCutsceneLoaded = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(GameManager.LoadMenu);
    }
}
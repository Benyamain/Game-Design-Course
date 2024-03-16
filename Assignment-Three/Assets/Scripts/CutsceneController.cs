using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    // Put this on main camera
    
    private PlayableDirector director;
    
    // Start is called before the first frame update
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.played += OnTimelineStarted;
        director.stopped += OnTimelineStopped;
    }

    // Update is called once per frame
    private void Update()
    {
        director.Play();
    }

    private void OnTimelineStarted(PlayableDirector director)
    {
        // TODO: Auto-generated method stub
    }

    private void OnTimelineStopped(PlayableDirector director)
    {
        // Load another scene only if it's not already loaded
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(GameManager.LoadMenu);
    }
}
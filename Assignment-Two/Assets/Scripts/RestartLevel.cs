using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Wait for key to be pressed
        // https://forum.unity.com/threads/restart-scene-key.812355/
        if (Input.GetKey(KeyCode.Space)) {
            // Load the scene again
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

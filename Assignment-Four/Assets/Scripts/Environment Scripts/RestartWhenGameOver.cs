using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RestartWhenGameOver : MonoBehaviour
{
    /* The purpose for this script is to account for the situation when the Player is dead, which
    indicates that their movement, nav mesh agent, and character controller script is turned off.
    Therfore, we cannot access the key to restart the game, and have to use this method instead. */
    private void Update()
    {
        // https://forum.unity.com/threads/restart-scene-key.812355/
        if (Keyboard.current.rKey.wasPressedThisFrame) {
            // Load the scene again
            GameManager.RestartGame();
            GameManager.ResetInstances();
        }

        if (Keyboard.current.mKey.wasPressedThisFrame) {
            SceneManager.LoadScene(GameManager.LoadMenu);
        }
    }
}

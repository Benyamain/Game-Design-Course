using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    public static int HealthPickupCount = 0;
    public static int MaxHealthPickupCount = 5;
    public static float CurrentScore = 0f;
    public static float HighScore = 0f;
    public static int LoadMenu = 0;
    public static int LoadGame = 1;
    public static Camera MainCamera;
    public static GameObject Player;
    public static GameObject Enemy;
    public static CharacterController PlayerCharacterController;
    public static bool IsPlayerDead;
    public static bool IsEnemyDead;
    public static bool WasMenuLoaded = false;

    private void Awake() {
        if (!WasMenuLoaded) {
            SceneManager.LoadScene(GameManager.LoadMenu);
            WasMenuLoaded = true;
        }
    }

    private void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        PlayerCharacterController = Player.GetComponent<CharacterController>();
    }

    // public static void PlayerDied() {
    //     IsPlayerDead = true;
    //     Destroy(Player);
    // }

    public static void EnemyDied() {
        IsEnemyDead = true;
        Destroy(Enemy);
    }

    public static void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void DisablePlayerCharacterController() {
        if (PlayerCharacterController != null)
        {
            PlayerCharacterController.enabled = false;
        }
    }

    public static void EnablePlayerCharacterController() {
        if (PlayerCharacterController != null)
        {
            PlayerCharacterController.enabled = true;
        }
    }

    // https://gist.github.com/kurtdekker/50faa0d78cd978375b2fe465d55b282b
    public static void AddHealthPickup() {
        ++HealthPickupCount;
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        HealthPickupCount = 0;
        MaxHealthPickupCount = 5;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    public static int HealthPickupCount = 0;
    public static int MaxHealthPickupCount = 5;
    public static int CurrentScore = 0;
    public static int HighScore = 0;
    public static int LoadMenu = 0;
    public static int LoadGame = 1;
    public static Camera MainCamera;
    public static GameObject Player;
    public static GameObject Enemy;
    public static GameObject EnemyHealthSlider;
    public static CharacterController PlayerCharacterController;
    public static bool IsPlayerDead;
    public static bool IsEnemyDead;
    public static bool WasMenuLoaded = false;
    public static float PlayerHealth = 100f;
    public static float EnemyHealth = 100f;

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
        EnemyHealthSlider = GameObject.FindGameObjectWithTag("EnemyHealthSlider");
        PlayerCharacterController = Player.GetComponent<CharacterController>();
    }

    // public static void PlayerDied() {
    //     IsPlayerDead = true;
    //     Destroy(Player);
    // }

    public static void EnemyDied() {
        CurrentScore++;
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

    public static void PlayerTakeDamage(float damageAmount) {
        if (PlayerHealth <= 0f) {
            return;
        }

        PlayerHealth -= damageAmount;
        if (PlayerHealth <= 0) {
            DisablePlayerCharacterController();
            RestartGame();
            ResetInstances();
            EnablePlayerCharacterController();
        }
    }

    public static void EnemyTakeDamage(float damageAmount) {
        if (EnemyHealth <= 0) return;
        EnemyHealth -= damageAmount;

        if (EnemyHealth <= 0f)
        {
            EnemyHealth = 0;
            EnemyDied();
        }

        // EnemyHealthSlider.value = EnemyHealth;
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        HealthPickupCount = 0;
        MaxHealthPickupCount = 5;
        CurrentScore = 0;
        PlayerHealth = 100f;
        EnemyHealth = 100f;
    }
}
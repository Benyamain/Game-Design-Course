using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using Grpc.Core;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    public static int CurrentScore = 0;
    public static int HighScore = 0;
    public static int LoadMenu = 0;
    public static int LoadGame = 1;
    public static Camera MainCamera;
    public static GameObject Player;
    public static GameObject Enemy;
    public static GameObject EnemyHealthSliderGameObject;
    public static CharacterController PlayerCharacterController;
    public static CharacterController EnemyCharacterController;
    public static bool IsPlayerDead;
    public static bool WasMenuLoaded = false;
    public static float PlayerHealth = 100f;
    public static float HealthPickupAmount = 100f;
    public static float MaxHealth = 100f;
    private static PlayerController PlayerControllerScript;
    public static NavMeshAgent PlayerNavMeshAgent;

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
        PlayerCharacterController = Player.GetComponent<CharacterController>();
        PlayerControllerScript = Player.GetComponent<PlayerController>();
        PlayerNavMeshAgent = Player.GetComponent<NavMeshAgent>();
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
    public static void AddHealthPickup(float healAmount) {
        // Clamping the value also works as well
        PlayerHealth = Mathf.Min(PlayerHealth + healAmount, MaxHealth);
    }

    public static void PlayerTakeDamage(float damageAmount) {
        if (PlayerHealth <= 0f) {
            return;
        }

        PlayerHealth -= damageAmount;

        if (PlayerHealth <= 0) {
            IsPlayerDead = true;
        }
    }

    public static void GameOver() {
        // Need to disable this to prevent clipping to ground with NavMeshSurface when Player cannot move.
        PlayerNavMeshAgent.enabled = false;
        PlayerControllerScript.enabled = false;
        DisablePlayerCharacterController();
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        CurrentScore = 0;
        PlayerHealth = 100f;
        IsPlayerDead = false;
    }
}
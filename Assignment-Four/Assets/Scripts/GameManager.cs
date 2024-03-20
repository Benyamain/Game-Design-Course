using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public static GameObject[] EnemySpawnPointsGameObject;
    public static CharacterController PlayerCharacterController;
    public static CharacterController EnemyCharacterController;
    public static bool IsPlayerDead;
    public static bool IsEnemyDead;
    public static bool WasMenuLoaded = false;
    public static float PlayerHealth = 100f;
    public static float EnemyHealth = 5f;
    public static Slider EnemyHealthSlider;
    public static List<Transform> EnemySpawnPoints = new List<Transform>();
    public static float HealthPickupAmount = 100f;
    public static float MaxHealth = 100f;

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
        EnemyCharacterController = Enemy.GetComponent<CharacterController>();
        EnemyHealthSliderGameObject = GameObject.FindGameObjectWithTag("EnemyHealthSlider");
        EnemySpawnPointsGameObject = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");

        GameObject[] spawnPoints = EnemySpawnPointsGameObject;
        foreach (GameObject spawnPoint in spawnPoints)
        {
            EnemySpawnPoints.Add(spawnPoint.transform);
        }
        

        SetEnemyHealthSlider();
    }

    public static void SetEnemyHealthSlider() {
        if (EnemyHealthSliderGameObject != null) {
            EnemyHealthSlider = EnemyHealthSliderGameObject.GetComponent<Slider>();
        }
    }

    public static void EnemyDied() {
        CurrentScore++;
        IsEnemyDead = true;
        Destroy(Enemy);
        RespawnEnemy();
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

    public static void DisableEnemyCharacterController() {
        if (EnemyCharacterController != null)
        {
            EnemyCharacterController.enabled = false;
        }
    }

    public static void EnableEnemyCharacterController() {
        if (EnemyCharacterController != null)
        {
            EnemyCharacterController.enabled = true;
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
            DisablePlayerCharacterController();
            DisableEnemyCharacterController();
        }
    }

    public static void EnemyTakeDamage(float damageAmount) {
        if (EnemyHealth <= 0f) return;
        
        EnemyHealth -= damageAmount;

        if (EnemyHealth <= 0)
        {
            EnemyHealth = 0f;
            EnemyDied();
        }

        if (EnemyHealthSlider != null) {
            EnemyHealthSlider.value = EnemyHealth;
        }
    }

    public static void RespawnEnemy()
    {
        Transform spawnPoint = EnemySpawnPoints[Random.Range(0, EnemySpawnPoints.Count)];
        Instantiate(Enemy, spawnPoint.position, spawnPoint.rotation);
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        CurrentScore = 0;
        PlayerHealth = 100f;
        EnemyHealth = 100f;
    }
}
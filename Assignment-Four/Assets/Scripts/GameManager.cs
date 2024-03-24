using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    // https://gist.github.com/kurtdekker/50faa0d78cd978375b2fe465d55b282b
    public static int CurrentScore = 0;
    public static int HighScore = 0;
    public static int LoadMenu = 0;
    public static int LoadGame = 1;
    public static Camera MainCamera;
    public static GameObject Player;
    public static CharacterController PlayerCharacterController;
    public static PlayerShooting PlayerShootingScript;
    public static bool IsPlayerDead;
    public static bool WasMenuLoaded = false;
    public static float PlayerHealth = 100f;
    public static float HealthPickupAmount = 100f;
    public static float MaxHealth = 100f;
    private static PlayerController PlayerControllerScript;
    private static Animator PlayerAnimator;
    public static bool IsTeleportedToTop = false;

    private void Awake() {
        // Be sure the menu is the first thing being loaded when the game starts
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
        PlayerAnimator = Player.GetComponent<Animator>();
        PlayerShootingScript = Player.GetComponent<PlayerShooting>();
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

    public static void EnablePlayerAnimator() {
        if (PlayerAnimator != null)
        {
            PlayerAnimator.enabled = true;
        }
    }

    public static void DisablePlayerAnimator() {
        if (PlayerAnimator != null)
        {
            PlayerAnimator.enabled = false;
        }
    }

    public static void EnablePlayerControllerScript() {
        if (PlayerAnimator != null)
        {
            PlayerControllerScript.enabled = true;
        }
    }

    public static void DisablePlayerControllerScript() {
        if (PlayerControllerScript != null)
        {
            PlayerControllerScript.enabled = false;
        }
    }

    public static void EnablePlayerCharacterController() {
        if (PlayerCharacterController != null)
        {
            PlayerCharacterController.enabled = true;
        }
    }

    public static void DisablePlayerShootingScript() {
        if (PlayerShootingScript != null)
        {
            PlayerShootingScript.enabled = false;
        }
    }

    public static void EnablePlayerShootingScript() {
        if (PlayerShootingScript != null)
        {
            PlayerShootingScript.enabled = true;
        }
    }

    public static void AddHealthPickup(float healAmount) {
        // Clamping the value also works
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
        DisablePlayerControllerScript();
        DisablePlayerCharacterController();
        DisablePlayerShootingScript();
        DisablePlayerAnimator();
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        CurrentScore = 0;
        PlayerHealth = 100f;
        IsPlayerDead = false;
        IsTeleportedToTop = false;
    }
}
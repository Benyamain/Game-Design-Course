using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Camera MainCamera;
    public static GameObject Player;
    public static CharacterController PlayerCharacterController;
    public static GameObject Enemy;
    public static bool IsEnemyDead;

    private void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        PlayerCharacterController = Player.GetComponent<CharacterController>();

        DisableCursorMode();
    }

    public static void EnemyDied() {
        IsEnemyDead = true;
        Destroy(Enemy);
    }

    public static void DisablePlayerCharacterController() {
        if (PlayerCharacterController != null) {
            PlayerCharacterController.enabled = false;
        }
    }

    public static void EnablePlayerCharacterController() {
        if (PlayerCharacterController != null)
        {
            PlayerCharacterController.enabled = true;
        }
    }

    public static void EnableCursorMode() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void DisableCursorMode() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
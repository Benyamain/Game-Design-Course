using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    public static int SkullCount = 0;
    public static int MaxSkulls = 5;
    public static float CurrentTime = 0f;
    public static float BestTime = 0f;
    public static int LoadMenu = 1;
    public static int LoadGame = 0;
    public static bool ReachedEndzone = false;
    public static bool IsLocalLayer = false;
    public static Camera GunCamera;
    public static Camera MainCamera;
    public static GameObject Player;
    public static GameObject Enemy;
    public static GameObject PlayerShadows;
    public static GameObject PlayerCameraCrosshair;
    public static GameObject WeaponCameraCrosshair;
    public static CharacterController PlayerCharacterController;
    public static Vector3 CheckpointPosition = new(-35.96f, 44.36f, -63.42f);
    public static Vector3 CheckpointRotation = new(0f, 110.43f, 0f);
    public static Vector3 CheckpointScale = new(2f, 2f, 2f);
    public static bool IsPlayerDead;
    public static bool IsEnemyDead;

    private void Start()
    {
        GunCamera = GameObject.FindGameObjectWithTag("GunCamera").GetComponent<Camera>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Enemy = GameObject.FindGameObjectWithTag("Enemy");
        PlayerShadows = GameObject.FindGameObjectWithTag("PlayerShadows");
        PlayerCameraCrosshair = GameObject.FindGameObjectWithTag("PlayerCameraCrosshair");
        WeaponCameraCrosshair = GameObject.FindGameObjectWithTag("WeaponCameraCrosshair");
        PlayerCharacterController = Player.GetComponent<CharacterController>();

        SetLayerRecursively(Player.transform, IsLocalLayer ? 6 : 0);
        PlayerCameraCrosshair.SetActive(true);
        WeaponCameraCrosshair.SetActive(false);

        Cursor.lockState  = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void PlayerDied() {
        IsPlayerDead = true;
        Destroy(Player);
    }

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

            Cursor.lockState  = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public static void EnablePlayerCharacterController() {
        if (PlayerCharacterController != null)
        {
            PlayerCharacterController.enabled = true;
            Cursor.lockState  = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    
    // https://gist.github.com/kurtdekker/50faa0d78cd978375b2fe465d55b282b
    public static void AddSkull() {
        ++SkullCount;
    }

    public static void ChangeLayer(bool changeLayer) {
        if (changeLayer)
        {
            ActivateGunCamera();
        } else {
            ActivateMainCamera();
        }
    }

    private static void ActivateMainCamera() {
        // Remove the gun camera from the stack
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Remove(GunCamera);
            
        // Set the rendering settings for the main camera
        UniversalAdditionalCameraData MainCameraData = MainCamera.GetUniversalAdditionalCameraData();
        MainCameraData.renderShadows = true;
        MainCameraData.renderPostProcessing = false;

        // Exclude the "Local" layer from the culling mask ; think of it as the complement property
        MainCamera.cullingMask = ~(1 << LayerMask.NameToLayer("Local"));

        SetLayerRecursively(Player.transform, 0);
        SetShadowsEnabled(false);
        PlayerCameraCrosshair.SetActive(true);
        WeaponCameraCrosshair.SetActive(false);
    }

    private static void ActivateGunCamera()
    {
        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(GunCamera);

        // Set the rendering settings for the gun camera
        UniversalAdditionalCameraData GunCameraData = GunCamera.GetUniversalAdditionalCameraData();
        GunCameraData.renderShadows = true;
        GunCameraData.renderPostProcessing = false;

        // Set the culling mask to show only the "Local" layer
        GunCamera.cullingMask = 1 << LayerMask.NameToLayer("Local");

        SetLayerRecursively(Player.transform, 6);
        SetShadowsEnabled(true);
        WeaponCameraCrosshair.SetActive(true);
        PlayerCameraCrosshair.SetActive(false);
    }

    // Because player has nested game objects, we need to recursively set everything to Local space
    /* Note: The functionality of this might be rendered pointless as only Shadows are rendered in the Local space.
       Thus, I could just set the Player and Weapon game object to be rendered in local space and toggle between
       to be more efficient with code usage.
    */
    private static void SetLayerRecursively(Transform parentTransform, int layer)
    {
        parentTransform.gameObject.layer = layer;

        foreach (Transform child in parentTransform)
        {
            SetLayerRecursively(child, layer);
        }
    }

    // Switching FOV requires for the player to not render shadows in specific state
    private static void SetShadowsEnabled(bool enable)
    {
        Renderer renderer = PlayerShadows.GetComponent<Renderer>();
        
        if (renderer != null)
        {
            renderer.shadowCastingMode = enable ? UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = enable;
        }
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        SkullCount = 0;
        MaxSkulls = 5;
        ReachedEndzone = false;
        IsLocalLayer = false;
    }
}
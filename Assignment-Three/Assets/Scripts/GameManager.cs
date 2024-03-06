using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public static bool CanDance = false;

    // https://gist.github.com/kurtdekker/50faa0d78cd978375b2fe465d55b282b
    public static void AddSkull() {
        ++SkullCount;
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        SkullCount = 0;
        MaxSkulls = 5;
        ReachedEndzone = false;
        CanDance = false;
    }
}
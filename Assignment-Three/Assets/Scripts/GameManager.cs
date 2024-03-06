using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // https://forum.unity.com/threads/help-how-do-you-set-up-a-gamemanager.131170/
    public static int CoinCount = 0;
    public static int MaxCoins = 5;
    public static float CurrentTime = 0f;
    public static float BestTime = 0f;
    public static int LoadMenu = 1;
    public static int LoadGame = 0;
    public static bool ReachedEndzone = false;
    public static bool CanDance = false;

    // https://gist.github.com/kurtdekker/50faa0d78cd978375b2fe465d55b282b
    public static void AddCoin() {
        ++CoinCount;
    }

    // Reset values just to be safe
    public static void ResetInstances() {
        CoinCount = 0;
        MaxCoins = 5;
        ReachedEndzone = false;
        CanDance = false;
    }
}
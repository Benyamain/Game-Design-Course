using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : BaseGameUI
{    
    private float _highScore;
    private PlayerHealth _playerHealth;
    
    // https://forum.unity.com/threads/how-do-i-make-my-code-only-display-1-or-2-numbers-after-the-decimal.370059/
    
    // Start is called before the first frame update
    protected override void Start()
    {   
        // Call the base start method so the label and button are set up first.
        base.Start();

        if (PlayerPrefs.GetFloat("NEW KEY") == 0f)
        {
            HighScoreLabel.text = $"High Score: None";
        }
        else {
            HighScoreLabel.text = "High Score: " + PlayerPrefs.GetFloat("NEW KEY").ToString("F0");
        }

        CurrentScoreLabel.text = "Demons Killed: " + GameManager.CurrentScore.ToString();

        //HealthLabel.text = "Health: " + _playerHealth.ToString();

        JumpLabel.text = "Jump (Spacebar)";

        SprintLabel.text = "Sprint (LShift)";
        
        CrouchLabel.text = "Crouch (C)";

        MenuLabel.text = "Menu (M)";
        
        RestartButton.text = "Restart (R)";
    }

    // Update is called once per frame
    private void Update()
    {
        CurrentScoreLabel.text = "Demons Killed: " + GameManager.CurrentScore.ToString("F0");
        //HealthLabel.text = "Health: " + _playerHealth.ToString();

        if ((GameManager.HealthPickupCount == GameManager.MaxHealthPickupCount) && GameManager.IsEnemyDead) {
            // Check if the current time is less than the existing best time once player is in endzone and has all coins.
            if ((GameManager.CurrentScore < PlayerPrefs.GetFloat("NEW KEY", float.MaxValue))) {
                GameManager.HighScore = GameManager.CurrentScore;
                // Store in temporary as there is bug fix that resets and does not show the best time
                _highScore = GameManager.HighScore;

                PlayerPrefs.SetFloat("NEW KEY", _highScore);
            }

            GameManager.RestartGame();
            GameManager.ResetInstances();
        }
    }
}
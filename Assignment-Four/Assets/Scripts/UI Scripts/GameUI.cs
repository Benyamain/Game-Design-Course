using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : BaseGameUI
{
    private int _highScore;

    // https://forum.unity.com/threads/how-do-i-make-my-code-only-display-1-or-2-numbers-after-the-decimal.370059/

    // Start is called before the first frame update
    protected override void Start()
    {
        // Call the base start method so the label and button are set up first.
        base.Start();

        if (PlayerPrefs.GetInt("NEW KEY") == 0f)
        {
            HighScoreLabel.text = $"High Score: None";
        }
        else
        {
            HighScoreLabel.text = "High Score: " + PlayerPrefs.GetInt("NEW KEY").ToString();
        }

        CurrentScoreLabel.text = "Demons Killed: " + GameManager.CurrentScore.ToString();

        HealthLabel.text = "Health: " + GameManager.PlayerHealth.ToString();

        JumpLabel.text = "Jump (Spacebar)";

        MenuLabel.text = "Menu (M)";

        RestartButton.text = "Restart (R)";
    }

    // Update is called once per frame
    private void Update()
    {
        CurrentScoreLabel.text = "Demons Killed: " + GameManager.CurrentScore.ToString();
        HealthLabel.text = "Health: " + GameManager.PlayerHealth.ToString();

        if ((GameManager.CurrentScore > PlayerPrefs.GetInt("NEW KEY", int.MinValue)))
        {
            GameManager.HighScore = GameManager.CurrentScore;
            // Store in temporary as there is bug fix that resets and does not show the best score
            _highScore = GameManager.HighScore;

            PlayerPrefs.SetInt("NEW KEY", _highScore);
        }
    }
}
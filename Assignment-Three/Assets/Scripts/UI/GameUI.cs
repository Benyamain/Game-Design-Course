using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : BaseGameUI
{    
    private float _bestTime;
    
    // https://forum.unity.com/threads/how-do-i-make-my-code-only-display-1-or-2-numbers-after-the-decimal.370059/
    
    // Start is called before the first frame update
    protected override void Start()
    {   
        // Call the base start method so the label and button are set up first.
        base.Start();

        // Check if the current time is greater than the existing best time.
        if (PlayerPrefs.GetFloat("NEW KEY") == 0f)
        {
            // First time playing the game
            BestTimeLabel.text = $"Best: None";
        }
        else {
            // Get new best time
            BestTimeLabel.text = "Best: " + PlayerPrefs.GetFloat("NEW KEY").ToString("F2") + " s";
        }

        // Display no score at the start of the game.
        ScoreLabel.text = "Skulls: " + GameManager.SkullCount.ToString() + " of " + GameManager.MaxSkulls.ToString();

        // Display no time at the start of the game.
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString() + " s";

        JumpLabel.text = "Jump (Spacebar)";

        SprintLabel.text = "Sprint (LShift)";
        
        CrouchLabel.text = "Crouch (C)";

        FOVLabel.text = "FOV (F)";

        RestartLabel.text = "Restart (R)";
        
        // This button is for taking us to the menu so make it say so.
        GameLevelButton.text = "Menu (M)";
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the label to show the most up to date score
        ScoreLabel.text = "Skulls: " + GameManager.SkullCount.ToString() + " of " + GameManager.MaxSkulls.ToString();
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString("F2") + " s";

        // Check if the current time is less than the existing best time once player is in endzone and has all coins.
        if ((GameManager.CurrentTime < PlayerPrefs.GetFloat("NEW KEY", float.MaxValue)) && GameManager.ReachedEndzone && (GameManager.SkullCount == GameManager.MaxSkulls) && GameManager.IsEnemyDead)
        {
            GameManager.BestTime = GameManager.CurrentTime;
            // Store in temporary as there is bug fix that resets and does not show the best time
            _bestTime = GameManager.BestTime;

            // Set new best time
            PlayerPrefs.SetFloat("NEW KEY", _bestTime);

            // Load the scene again
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.ResetInstances();
        }
    }
}
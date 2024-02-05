using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (PlayerPrefs.GetFloat("TIME BEST WR") == 0f)
        {
            // First time playing the game
            BestTimeLabel.text = $"Best: None";
        }
        else {
            // Get new best time
            BestTimeLabel.text = "Best: " + PlayerPrefs.GetFloat("TIME BEST WR").ToString("F2") + " s";
        }

        // Display no score at the start of the game.
        ScoreLabel.text = "Coins: " + GameManager.CoinCount.ToString() + " of " + GameManager.MaxCoins.ToString();

        // Display no time at the start of the game.
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString() + " s";
        
        // This button is for taking us to the menu so make it say so.
        GameLevelButton.text = "Menu";
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the label to show the most up to date score
        ScoreLabel.text = "Coins: " + GameManager.CoinCount.ToString() + " of " + GameManager.MaxCoins.ToString();
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString("F2") + " s";

        // Check if the current time is less than the existing best time once player is in endzone and has all coins.
        if ((GameManager.CurrentTime < PlayerPrefs.GetFloat("TIME BEST WR", float.MaxValue)) && GameManager.ReachedEndzone && Input.GetKey(KeyCode.R) && (GameManager.CoinCount == GameManager.MaxCoins))
        {
            GameManager.BestTime = GameManager.CurrentTime;
            // Store in temporary as there is bug fix that resets and does not show the best time
            _bestTime = GameManager.BestTime;

            // Set new best time
            PlayerPrefs.SetFloat("TIME BEST WR", _bestTime);
        }
    }
}
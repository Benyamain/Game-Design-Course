using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : BaseGameUI
{    
    // https://forum.unity.com/threads/how-do-i-make-my-code-only-display-1-or-2-numbers-after-the-decimal.370059/
    
    // Start is called before the first frame update
    protected override void Start()
    {
        // Call the base start method so the label and button are set up first.
        base.Start();

        // Check if the current time is greater than the existing best time.
        if (PlayerPrefs.GetFloat("BEST TIME") == 0f)
        {
            // First time playing the game
            BestTimeLabel.text = $"Best: None";
        }
        else {
            // Get new best time
            BestTimeLabel.text = "Best: " + PlayerPrefs.GetFloat("BEST TIME").ToString("F2") + " s";
        }

        // Display no score at the start of the game.
        ScoreLabel.text = "Coins: " + GameManager.CoinCount.ToString() + " of " + GameManager.MaxCoins.ToString();

        // Display no time at the start of the game.
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString() + " s";
        
        // This button is for taking us to the menu so make it say so.
        LevelButton.text = "Menu";
    }

    // Update is called once per frame
    private void Update()
    {
        // Update the label to show the most up to date score
        ScoreLabel.text = "Coins: " + GameManager.CoinCount.ToString() + " of " + GameManager.MaxCoins.ToString();
        TimeLabel.text = "Time: " + GameManager.CurrentTime.ToString("F2") + " s";

        // Check if the current time is greater than the existing best time.
        if (GameManager.CurrentTime > PlayerPrefs.GetFloat("BEST TIME"))
        {
            GameManager.BestTime = GameManager.CurrentTime;

            // Set new best time
            PlayerPrefs.SetFloat("BEST TIME", GameManager.BestTime);
        }
    }
}
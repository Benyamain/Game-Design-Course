using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseGameUI : MonoBehaviour
{
    /// <summary>
    /// The label to display the score in the game and the high score in the menu.
    /// </summary>
    protected Label ScoreLabel;

    /// <summary>
    /// The label to display the time in the game.
    /// </summary>
    protected Label TimeLabel;

    /// <summary>
    /// The label to display the best time in the game.
    /// </summary>
    protected Label BestTimeLabel;

    /// <summary>
    /// The label to display how to jump.
    /// </summary>
    protected Label JumpLabel;
    
    /// <summary>
    /// The label to display how to sprint.
    /// </summary>
    protected Label SprintLabel;

    /// <summary>
    /// The label to display how to crouch.
    /// </summary>
    protected Label CrouchLabel;

    /// <summary>
    /// The label to display how to switch FOV.
    /// </summary>
    protected Label FOVLabel;

    /// <summary>
    /// The label to display how to restart.
    /// </summary>
    protected Label RestartLabel;

    /// <summary>
    /// The button to switch between the game and menu.
    /// </summary>
    protected Button GameLevelButton;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Get the component with our UI so we can they query it for the parts we want.
        UIDocument document = GetComponent<UIDocument>();

        // Start from the root of the UI and we can search for the other parts from there.
        VisualElement root = document.rootVisualElement;

        // Get the label for the score and high score.
        ScoreLabel = root.Q<Label>("ScoreLabel");

        // Get the label for the time.
        TimeLabel = root.Q<Label>("TimeLabel");

        // Get the label for the best time.
        BestTimeLabel = root.Q<Label>("BestTimeLabel");

        JumpLabel = root.Q<Label>("JumpLabel");

        SprintLabel = root.Q<Label>("SprintLabel");

        CrouchLabel = root.Q<Label>("CrouchLabel");

        FOVLabel = root.Q<Label>("FOVLabel");

        RestartLabel = root.Q<Label>("RestartLabel");
        
        // Get the button for switching levels.
        GameLevelButton = root.Q<Button>("GameLevelButton");

        GameLevelButton.clicked += LoadLevel;
    }

    private void LoadLevel()
    {
        // Load the level based on the index set in the inspector.
        SceneManager.LoadScene(GameManager.LoadMenu);
    }

    private void OnDestroy()
    {
        GameLevelButton.clicked -= LoadLevel;
    }
}
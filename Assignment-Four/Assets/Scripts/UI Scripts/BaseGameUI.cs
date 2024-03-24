using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseGameUI : MonoBehaviour
{
    protected Label HighScoreLabel;

    protected Label CurrentScoreLabel;

    protected Label HealthLabel;

    /// <summary>
    /// The label to display how to jump.
    /// </summary>
    protected Label JumpLabel;
    
    protected Label MenuLabel;

    protected Button RestartButton;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Get the component with our UI so we can they query it for the parts we want.
        UIDocument document = GetComponent<UIDocument>();

        // Start from the root of the UI and we can search for the other parts from there.
        VisualElement root = document.rootVisualElement;

        HighScoreLabel = root.Q<Label>("HighScoreLabel");

        CurrentScoreLabel = root.Q<Label>("CurrentScoreLabel");

        HealthLabel = root.Q<Label>("HealthLabel");

        JumpLabel = root.Q<Label>("JumpLabel");

        MenuLabel = root.Q<Label>("MenuLabel");
        
        RestartButton = root.Q<Button>("RestartButton");

        RestartButton.clicked += LoadLevel;
    }

    private void LoadLevel()
    {
        // Load the level based on the index set in the inspector.
        SceneManager.LoadScene(GameManager.LoadMenu);
    }

    private void OnDestroy()
    {
        if (RestartButton != null)
        {
            RestartButton.clicked -= LoadLevel;
        }
    }
}
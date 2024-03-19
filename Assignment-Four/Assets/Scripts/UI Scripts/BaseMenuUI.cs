using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseMenuUI : MonoBehaviour
{
    /// <summary>
    /// The button to switch between the game and menu.
    /// </summary>
    protected Button MenuLevelButton;

    // Start is called before the first frame update
    protected virtual void Start()
    {        
        // Get the component with our UI so we can they query it for the parts we want.
        UIDocument document = GetComponent<UIDocument>();

        // Start from the root of the UI and we can search for the other parts from there.
        VisualElement root = document.rootVisualElement;
        
        // Get the button for switching levels.
        MenuLevelButton = root.Q<Button>("MenuLevelButton");

        MenuLevelButton.clicked += LoadLevel;
    }

    private void LoadLevel()
    {
        // Load the level based on the index set in the inspector.
        SceneManager.LoadScene(GameManager.LoadGame);
    }

    private void OnDestroy()
    {
        if (MenuLevelButton != null)
        {
            MenuLevelButton.clicked -= LoadLevel;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseMenuUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The scene number to load when the button is clicked.")]
    private int _load;

    /// <summary>
    /// The button to switch between the game and menu.
    /// </summary>
    protected Button LevelButton;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Get the component with our UI so we can they query it for the parts we want.
        UIDocument document = GetComponent<UIDocument>();

        // Start from the root of the UI and we can search for the other parts from there.
        VisualElement root = document.rootVisualElement;
        
        // Get the button for switching levels.
        LevelButton = root.Q<Button>("LevelButton");

        LevelButton.clicked += LoadLevel;
    }

    private void LoadLevel()
    {
        // Load the level based on the index set in the inspector.
        SceneManager.LoadScene(_load);
    }

    private void Update()
    {
        // Switch levels
        if (Input.GetKey(KeyCode.Space))
        {
            LoadLevel();
        }
    }

    private void OnDestroy()
    {
        LevelButton.clicked -= LoadLevel;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UIDocument))]
public abstract class BaseCutsceneUI : MonoBehaviour
{
    protected Button CutsceneLevelButton;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Get the component with our UI so we can they query it for the parts we want.
        UIDocument document = GetComponent<UIDocument>();

        // Start from the root of the UI and we can search for the other parts from there.
        VisualElement root = document.rootVisualElement;

        CutsceneLevelButton = root.Q<Button>("CutsceneLevelButton");

        CutsceneLevelButton.clicked += LoadLevel;
    }

    private void LoadLevel()
    {
        SceneManager.LoadScene(GameManager.LoadMenu);
    }

    private void OnDestroy()
    {
        CutsceneLevelButton.clicked -= LoadLevel;
    }
}
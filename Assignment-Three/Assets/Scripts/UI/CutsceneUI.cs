using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneUI : BaseCutsceneUI
{    
    protected override void Start()
    {   
        base.Start();

        CutsceneLevelButton.text = "Skip";
    }
}
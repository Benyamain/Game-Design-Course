using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : BaseMenuUI
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        LevelButton.text = "'Space' to Play";
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRepeater : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Called once every frame
    private void Update()
    {
        // Rotate on the y-axis but do it with respect to time
        // https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        transform.Rotate(0f, 30f * Time.deltaTime, 0f);
    }
}

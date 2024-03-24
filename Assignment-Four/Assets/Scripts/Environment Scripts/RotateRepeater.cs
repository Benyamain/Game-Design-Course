using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRepeater : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Object rotation speed")]
    private float _speed = 100f;
    
    private void Update()
    {
        // Rotate on the y-axis but do it with respect to time
        // https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        transform.Rotate(0f, _speed * Time.deltaTime, 0f);
    }
}
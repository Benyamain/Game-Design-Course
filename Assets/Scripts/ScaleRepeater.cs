using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRepeater : MonoBehaviour
{
    // Declaration of variables
    [SerializeField]
    [Tooltip("The original state of the object scale")]
    private Vector3 _startScale = new Vector3(1f, 1f, 1f);
    [SerializeField]
    [Tooltip("The target state of the object scale")]
    private Vector3 _endScale = new Vector3(2f, 2f, 2f);
    private bool scaleUp = true;
    [SerializeField]
    [Tooltip("The rate of movement for the game object")]
    [Range(0, 10)]
    private float _speed = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Determine the target scale based on the current scale state
        // https://discussions.unity.com/t/resizing-an-object/6145/3
        Vector3 targetScale = scaleUp ? _endScale : _startScale;
        
        // Scale towards the target scale
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, _speed * Time.deltaTime);

        // Discrepancy of difference accounted for
        // https://forum.unity.com/threads/i-want-to-move-an-object-toward-a-target-and-scale-it-from-1-toward-zero.215055/
        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            // Change scaling 
            scaleUp = !scaleUp;
        }
    }
}

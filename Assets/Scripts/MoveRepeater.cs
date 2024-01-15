using UnityEngine;

public class Repeatable : MonoBehaviour
{
    // https://stackoverflow.com/questions/40489102/moving-an-object-with-vector3-movetowards
    public Vector3 targetPosition = new Vector3(-4f, 0.5f, 2f);
    public float speed = 1f;

    private Vector3 startPosition;
    private bool movingForward = true;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the start position
        startPosition = transform.localPosition;
    }

    // Called once every frame
    private void Update()
    {
        // Change target based on if moving forward or not
        Vector3 target = movingForward ? targetPosition : startPosition;

        // Move towards the target position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (transform.localPosition == target)
        {
            // Change direction
            movingForward = !movingForward;
        }
    }
}
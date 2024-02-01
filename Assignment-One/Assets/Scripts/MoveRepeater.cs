using UnityEngine;

public class Repeatable : MonoBehaviour
{
    // https://stackoverflow.com/questions/40489102/moving-an-object-with-vector3-movetowards
    [SerializeField]
    [Tooltip("The target position of where the game object shall be moving towards in the scene")]
    private Vector3 _targetPosition = new Vector3(-4f, 0.5f, 2f);
    [SerializeField]
    [Tooltip("The rate of movement for the game object")]
    [Range(0, 10)]
    private float _speed = 1f;

    [SerializeField]
    [Tooltip("The start position of where the game object currently lies in the scene")]
    private Vector3 _startPosition;
    private bool _movingForward = true;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the start position
        _startPosition = transform.localPosition;
    }

    // Called once every frame
    private void Update()
    {
        // Change target based on if moving forward or not
        Vector3 target = _movingForward ? _targetPosition : _startPosition;

        // Move towards the target position
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, _speed * Time.deltaTime);

        // Check if the object has reached the target position
        if (transform.localPosition == target)
        {
            // Change direction
            _movingForward = !_movingForward;
        }
    }
}
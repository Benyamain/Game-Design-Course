using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class HitScan : MonoBehaviour
{
    [Tooltip("Where to start the ray from.")]
    [SerializeField]
    private Transform rayStart;

    private LineRenderer _lr;
    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.isPressed)
        {
            return;
        }

        // Cache the start position.
        Vector3 p = rayStart.position;
        
        // Perform the ray cast/hit scan, returning true if something is hit.
        if (Physics.Raycast(p, rayStart.forward, out RaycastHit hit))
        {
            // If something is hit, set the positions to it.
            _lr.SetPositions(new []{p, hit.point});
        }
        else
        {
            // Otherwise, find a point in the distance to draw the ray.
            _lr.SetPositions(new []{p, p + rayStart.TransformDirection(Vector3.forward) * 1000});
        }
    }
}
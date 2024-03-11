using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class HitScan : MonoBehaviour
{
    // https://forum.unity.com/threads/linerenderer-fade-out-solved.542988/
    // https://forum.unity.com/threads/how-to-add-camera-to-stack.926573/
    
    [Tooltip("Where to start the ray from.")]
    [SerializeField]
    private Transform rayStart;

    [SerializeField]
    private float fadeOutSpeed = 10000f;

    [SerializeField]
    private float lineWidth = 0.015f;

    private LineRenderer _lr;
    private float _alpha = 1f;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {        
        if (!Mouse.current.leftButton.isPressed)
        {
            ClearLineRenderer();
            return;
        }

        Vector3 p = rayStart.position;

        if (Physics.Raycast(p, rayStart.TransformDirection(Vector3.forward), out RaycastHit hit))
        {
            SetLineRendererPositions(p, hit.point);

            // Check if object hit has a collider
            if (hit.collider != null && hit.collider.CompareTag("Destroyable")) {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            SetLineRendererPositions(p, p + rayStart.TransformDirection(Vector3.forward) * 1000);
        }

        SetLineRendererWidth();
        FadeOutLineRenderer();
    }

    private void ClearLineRenderer()
    {
        _lr.positionCount = 0;
        // Reset when not drawing
        _alpha = 1f;
    }

    private void SetLineRendererPositions(Vector3 start, Vector3 end)
    {
        _lr.positionCount = 2;
        _lr.SetPositions(new[] { start, end });
    }

    private void SetLineRendererWidth()
    {
        _lr.startWidth = lineWidth;
        _lr.endWidth = lineWidth;
    }

    private void FadeOutLineRenderer()
    {
        if (_alpha > 0f)
        {
            _alpha -= Time.deltaTime * fadeOutSpeed;
            Color lineColor = _lr.startColor;
            lineColor.a = _alpha;
            _lr.startColor = lineColor;
            _lr.endColor = lineColor;
        } else {
            ClearLineRenderer();
        }
    }
}
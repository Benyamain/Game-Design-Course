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
    private float lineWidth = 0.045f;
    private CharacterMover characterMover;

    private LineRenderer _lr;
    private float _alpha = 1f;

    [SerializeField]
    private float damageAmount = 1f;
    

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
        characterMover = GetComponent<CharacterMover>();
    }

    private void Update()
    {        
        if (!Mouse.current.leftButton.isPressed)
        {
            ClearLineRenderer();
            return;
        }

        // Idea of implementation inspired by Ethan Scheys
        Ray ray = GameManager.MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (characterMover.canShoot && !characterMover.isRunning && !characterMover.isRunningBackwards) {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {                
                SetLineRendererPositions(rayStart.position, hit.point);
                
                // Render the line renderer from the MainCamera's perspective
                // SetLineRendererPositions(GameManager.MainCamera.WorldToViewportPoint(rayStart.position), GameManager.MainCamera.WorldToViewportPoint(hit.point));

                // Check if object hit has a collider
                if (hit.collider != null && hit.collider.CompareTag("Destroyable")) {
                    Destroy(hit.collider.gameObject);
                }

                // Check if bullet hits enemy, then enemy takes damage
                if (hit.collider != null && hit.collider.CompareTag("Enemy")) {
                    hit.collider.gameObject.GetComponent<EnemyHealth>().TakeDamage(damageAmount);                    
                }
            }
            else
            {
                SetLineRendererPositions(rayStart.position, rayStart.position + ray.direction * 1000);
                
                // Render the line renderer from the MainCamera's perspective
                // SetLineRendererPositions(GameManager.MainCamera.WorldToViewportPoint(rayStart.position), GameManager.MainCamera.WorldToViewportPoint(rayStart.position + ray.direction * 1000));
            }

            SetLineRendererWidth();
            FadeOutLineRenderer();
        }
    }

    private void ClearLineRenderer()
    {
        _lr.positionCount = 0;
        // Reset when not drawing
        _alpha = 1f;
    }

    private void SetLineRendererPositions(Vector3 start, Vector3 end)
    {
        // if (GameManager.IsLocalLayer)
        // {
        //     // Transform the start and end points to the GunCamera's view space
        //     start = GameManager.GunCamera.WorldToViewportPoint(start);
        //     end = GameManager.GunCamera.WorldToViewportPoint(end);
        // }

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
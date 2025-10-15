using UnityEngine;

public class WheelTrailRenderHandler : MonoBehaviour
{
    CarController carController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBraking))
        {
            trailRenderer.emitting = true;
        }
        else 
        {
            trailRenderer.emitting = false;
        }
    }
}

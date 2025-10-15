using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car moviment")]
    public float driftFactor = 0.5f;
    public float accelerationFactor = 20.0f;
    public float turnFactor = 2.0f;
    public float maxSpeed = 12;

    public float accelerationInput = 0;
    public float turnFactorInput = 0;

    public float minTurnSpeedFactor = 0.5f;
    public float brakingForce = 12f;

    public float rotationAngle = 0;

    public float velocityVsUp = 0;

    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        EngineForce();
        RemoveOrthogonalVelocity();
        Steering();
    }

    void EngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, rb.linearVelocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (velocityVsUp < -maxSpeed * 0.2f && accelerationInput < 0)
        {
            return;
        }

        if (rb.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        rb.AddForce(engineForceVector, ForceMode2D.Force);

        if (Mathf.Abs(accelerationInput) < 0.1f)
        {
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 3.0f, Time.fixedDeltaTime * 5);
        }
        else
        {
            rb.linearDamping = 0.2f;
        }
    }

    void Steering()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        float speedFactor = Mathf.Clamp01(currentSpeed / maxSpeed);
        float turnSpeedMultiplier = Mathf.Lerp(0.3f, 1.0f, speedFactor);

        rotationAngle -= turnFactorInput * turnFactor * turnSpeedMultiplier * Time.fixedDeltaTime * 50f;

        rb.MoveRotation(rotationAngle);
    }

    void RemoveOrthogonalVelocity()
    {
        Vector2 fowardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rigthVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);


        rb.linearVelocity = fowardVelocity + rigthVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        turnFactorInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    public void ApplySpeedPenalty(float penaltyMultiplier)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity *= penaltyMultiplier;
        }
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, rb.linearVelocity);
    }

    public bool IsTireScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        isBraking = false;

        if (accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }

        if (Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;
        }

        return false;
    }
}
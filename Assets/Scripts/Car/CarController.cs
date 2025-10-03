using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car moviment")]
    public float driftFactor = 0.2f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 2.5f;
    public float maxSpeed = 15;

    public float accelerationInput = 0;
    public float turnFactorInput = 0;

    public float minTurnSpeedFactor = 0.3f;
    public float brakingForce = 8f;

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

        if (velocityVsUp < -maxSpeed * 0.3f && accelerationInput < 0)
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
            rb.linearDamping = Mathf.Lerp(rb.linearDamping, 2.0f, Time.fixedDeltaTime * 3);
        }
        else
        {
            rb.linearDamping = 0.1f;
        }
    }

    void Steering()
    {
        float minSpeedForTurning = (rb.linearVelocity.magnitude / 8);
        minSpeedForTurning = Mathf.Clamp(minSpeedForTurning, minTurnSpeedFactor, 1f);

        rotationAngle -= turnFactorInput * turnFactor * minSpeedForTurning;

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
}
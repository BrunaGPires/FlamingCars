using UnityEngine;
using System.Collections;

public class CarCollisionHandler : MonoBehaviour
{
    [Header("Collision Settings")]
    public float speedPenaltyMultiplier = 0.3f;
    public float penaltyDuration = 1.5f;

    [Header("Push Back Settings")]
    public float pushBackForce = 1f;
    public float pushBackDuration = 0.5f;

    [Header("Effects Settings")]
    public float shakeIntensity = 0.05f;
    public float shakeDuration = 0.3f;
    
    private CarController carController;
    private Rigidbody2D rb;
    private bool isColliding = false;
    
    void Start()
    {
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.linearDamping = 2f;
            rb.angularDamping = 2f;
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isColliding)
        {
            Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
            StartCoroutine(HandleCollision(pushDirection));
        }
    }
    
    private IEnumerator HandleCollision(Vector2 pushDirection)
    {
        isColliding = true;
        
        if (carController != null)
        {
            carController.ApplySpeedPenalty(speedPenaltyMultiplier);
        }
        
        StartCoroutine(PushBackEffect(pushDirection));
        StartCoroutine(ShakeEffect());
        StartCoroutine(BlinkEffect());
        
        yield return new WaitForSeconds(penaltyDuration);
        
        isColliding = false;
    }

    private IEnumerator PushBackEffect(Vector2 direction)
    {
        float elapsed = 0f;

        while (elapsed < pushBackDuration)
        {
            if (rb != null)
            {
                float forceMultiplier = 1f - (elapsed / pushBackDuration);
                rb.AddForce(direction * pushBackForce * forceMultiplier, ForceMode2D.Impulse);

            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator ShakeEffect()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            
            transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPos;
    }
    
    private IEnumerator BlinkEffect()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite == null) yield break;
        
        Color originalColor = sprite.color;
        
        for (int i = 0; i < 6; i++)
        {
            sprite.color = i % 2 == 0 ? Color.red : originalColor;
            yield return new WaitForSeconds(0.1f);
        }
        
        sprite.color = originalColor;
    }
}
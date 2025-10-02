using UnityEngine;
using System.Collections;

public class TrackBarrier : MonoBehaviour
{
    [Header("Barrier Settings")]
    public float speedPenalty = 0.5f;
    public float shakeIntensity = 0.03f;
    public float shakeDuration = 0.1f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log($"COLISÃO DETECTADA com {gameObject.name}!");

            CarController car = collision.gameObject.GetComponent<CarController>();
            if (car != null)
            {
                ApplySpeedPenalty(car);
            }
        }
    }

    void ApplySpeedPenalty(CarController car)
    {
        Rigidbody2D rb = car.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity *= speedPenalty;
            StartCoroutine(ShakeCar(car));
        }
    }

    IEnumerator ShakeCar(CarController car)
    {
        Vector3 originalPos = car.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            car.transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        car.transform.position = originalPos;
    }
}
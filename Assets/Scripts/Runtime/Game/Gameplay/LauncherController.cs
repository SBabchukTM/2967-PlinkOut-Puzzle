using UnityEngine;

public class LauncherController : MonoBehaviour
{
    [Header("Launcher Settings")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float launchForce = 5f;

    [Header("Rotation Settings")]
    [SerializeField] private float minAngle = -60f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private float rotationSpeed = 90f;

    private float currentAngle = 0f;

    public void RotateLeft()
    {
        currentAngle -= rotationSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        ApplyRotation();
    }

    public void RotateRight()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void Launch()
    {
        if (ballPrefab == null || firePoint == null)
        {
            Debug.LogWarning("Launcher is missing references.");
            return;
        }

        GameObject ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = -firePoint.up;
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
        }
    }
}

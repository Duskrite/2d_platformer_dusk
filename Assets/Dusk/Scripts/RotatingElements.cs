using UnityEngine;

public class RotatingElements : MonoBehaviour
{

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float minAngle = -70f;
    public float maxAngle = 70f;

    [Header("Visual Effects")]
    public bool useSmoothing = true;
    public float smoothFactor = 2f;

    private float currentAngle = 0f;
    private int direction = 1;
    void Update()
    {
        currentAngle += direction * rotationSpeed * Time.deltaTime;

        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
            direction = -1;
        }
        else if (currentAngle <= minAngle)
        {
            currentAngle = minAngle;
            direction = 1;
        }

        if (useSmoothing)
        {
            float targetZRotation = currentAngle;
            float currentZRotation = transform.localEulerAngles.z;

            if (currentZRotation > 180) currentZRotation -= 360f;

            float smoothedZRotation = Mathf.Lerp(currentZRotation, targetZRotation, smoothFactor * Time.deltaTime);
            transform.localRotation = Quaternion.Euler(0f, 0f, smoothedZRotation);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }
}

using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public bool rotate = false;
    float rotationDirection = 1; // direction of rotation 1 right, -1 left
    float rotationSpeed = 15.0f; // rotational speed

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (rotate)
        {
            // Rotate the camera around its Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * rotationDirection * Time.deltaTime);
        }
    }

    public void SetCamera(float dir)
    {
        rotationDirection = dir;
        rotate = true; // Ensure the camera starts rotating when direction is set
    }

    public void ResetCamera()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rotate = false; // Stop rotation
    }
}

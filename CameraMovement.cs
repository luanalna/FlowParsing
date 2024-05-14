using UnityEngine;

public class AutoRotateCamera : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public int block_number = 1;
    float rotationSpeedRight = ExperimentManager.cameraRotationSpeedRight;
    float rotationSpeedLeft = ExperimentManager.cameraRotationSpeedLeft;

    void Start()
    
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

void Update()
{
    if (ExperimentManager.startCameraRotating)
    {
        if (block_number == 1)
        {
            // Rotate the camera around its Y-axis to the right
            transform.Rotate(Vector3.up, rotationSpeedRight * Time.deltaTime);
        }
        else if (block_number == 2)
        {
            // Rotate the camera around its Y-axis to the left
            transform.Rotate(Vector3.up, rotationSpeedLeft * Time.deltaTime);
        }
    }
}

        // Function to reset the camera to its original position and rotation
    public void ResetCamera()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }

    // Method to update block number from ExperimentGenerator
    public void UpdateBlockNumber(int newBlockNumber)
    {
        block_number = newBlockNumber;
    }

}
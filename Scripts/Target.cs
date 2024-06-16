using UnityEngine;

public class Target : MonoBehaviour
{
    // Initial Target's positions
    public Vector3 startingTargetPosition = new Vector3(0f, 10f, 20f);

    // Initial Target's fall velocity
    public Vector3 fallVelocity = new Vector3(.0f, .0f, .0f);
    public bool fall = false;

    void Start()
    {
        fall = false;
        transform.position = startingTargetPosition;
    }

    void Update()
    {
        if (fall) // Let the target fall only if fall is true
        {
            // Update Target's velocity
            //fallVelocity.y -= Physics.gravity.magnitude * Time.deltaTime;

            // Update the Target's position
            /* with physics:   x{i+1} = x{i} + v{i} * t */  /* x{i+1}  x{i} + v{i}*t + 1/2 a{i} * t*t */
            transform.position -= fallVelocity * Time.deltaTime; // + Physics.gravity * Time.deltaTime * Time.deltaTime * 0.5f;
        }
    }

   public void SetTarget(float angle, float velocity, float distance)
    {
        /* This function I suppose is called from the Experiment Manager or whatever
        and sets the target's initial conditions based on the input provided from 
        experiment file reading */

        /* Reset target's position */
        startingTargetPosition.z = distance; // Set the distance from subject. (Z axis in unity, at starting orientation)
        transform.position = startingTargetPosition; // Reset the X and Y variables

        // Calculate the initial velocity based on the angle and speed
        float angleInRadians = angle * Mathf.Deg2Rad; // input angle from degrees to radians
        float initialVelocityX = Mathf.Sin(angleInRadians) * velocity; // Velocity along X-axis
        float initialVelocityY = Mathf.Cos(angleInRadians) * velocity; // Velocity along Y-axis

        // Set the fall velocity
        fallVelocity.x = initialVelocityX;
        fallVelocity.y = initialVelocityY;
    }

    public void startFalling()
    {
        fall = true;
    }

    public void stopFalling()
    {
        fall = false;
    }
}

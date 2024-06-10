using UnityEngine;

public class Target : MonoBehaviour
{
    // Initial Target's positions
    public Vector3 startingTargetPosition = new Vector3(2f, 26f, 20f);
    // Initial Target's fall velocity
    Vector3 startingFallVlocity = new Vector3(0f, 0f, 0f);
    Vector3 fallVelocity = new Vector3(0f, 0f, 0f);

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
            fallVelocity.y -= Physics.gravity.magnitude * Time.deltaTime;
            // Update the Target's position
            transform.position += fallVelocity * Time.deltaTime + Physics.gravity * Time.deltaTime * Time.deltaTime * 0.5f;
        }
    }

    public void SetTarget(float dir)
    {
        // Reset target's position
        transform.position = startingTargetPosition;
        // Reset target's velocity
        fallVelocity = startingFallVlocity;
        fallVelocity.x = dir * 2; // Ensure velocity direction is set correctly
    }

    public void SetDepth(float depth)
    {
        startingTargetPosition.z = depth;
        transform.position = startingTargetPosition;
    }

    public void startFalling()
    {
        fall = true;
    }

    public void stopFalling()
    {
        fall = false;
    }

    public void CalculateAngularSpeed(float dir, float radius)
    {
        float linearVelocityX = dir * 2;
        float angularSpeedRad = linearVelocityX / radius;
        float angularSpeedDeg = angularSpeedRad * Mathf.Rad2Deg;
        Debug.Log("Angular Speed: " + angularSpeedDeg + " degrees/second");
    }
}

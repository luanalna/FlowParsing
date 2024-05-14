using UnityEngine;


public class Target : MonoBehaviour
{

    // initiali Targeet's positions
    public Vector3 startingTargetPosition = new Vector3(0f, 0f, 0f);
    // initial Target's fall velocity
    Vector3 fallVelocity = new Vector3(0f, 0f, 0f);

    void Start()
    {
        transform.position = ExperimentManager.targetPositionStartTrial;
    }
    void Update()
    {
        if(ExperimentManager.startTargetFalling == true) // let the target fall only if startTargetFalling is true
        {
            // Update Target's velocity
            fallVelocity.y -= Physics.gravity.magnitude * Time.deltaTime;
            // Update the Target's position
            transform.position += fallVelocity * Time.deltaTime + Physics.gravity * Time.deltaTime * Time.deltaTime * 0.5f;
        }


        // DEBUGGING STAFF
        //Debug.Log("Current Target Position: " + transform.position);
    }

    public void ResetTarget(Vector3 resetPosition, Vector3 resetVelocity)
    {
        transform.position = resetPosition; // reset target's position
        fallVelocity = resetVelocity; // reset ttarget's velocity
    }
}

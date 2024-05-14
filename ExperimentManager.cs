using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Enumeration to define possible directions of velocity
enum velocityDirection
{
    Straight = 0,
    Right = 1,
    Left = 2
}

public static class ExperimentManager
{
    // Flag to control particle creation during the experiment
    public static bool createParticles = false;

    // Flag to check if the experiment is currently running
    public static bool experimentIsRunning = false;

    // Flag to track if the subject's answer has been recorded
    public static bool answerFromSubjectRecorded = true;

    // StreamWriter used to log subject answers to a file
    public static StreamWriter writer = new StreamWriter("Record_Answers.txt", true);

    // Target settings
    // Flag to control when the target starts falling
    public static bool startTargetFalling = true;

    // Camera settings
    // Flag to control camera rotation
    public static bool startCameraRotating = false;

    // Camera rotation speeds
    public static float cameraRotationSpeedLeft = 0.0f; // Left rotational speed of the camera
    public static float cameraRotationSpeedRight = 0.0f; // Right rotational speed of the camera
    public static float cameraRotationSpeed = 0.0f; // General rotation speed of the camera

    // Timing settings
    // Time recorded when the subject presses the spacebar to indicate readiness
    public static float timeAtBeginningSpacebarPress = 0.0f;

    // Time after which the target should fall following the spacebar press
    public static float timeTargetToFall = 3.0f;

    // Trial settings
    // Number of trials and blocks in the experiment
    public static int numberOfTrials = 3;
    public static int numberOfBlocks = 2;

    // Velocity vectors for the target falling in different directions
    public static Vector3 fallVelocityStraight = new Vector3(4f, 0f, 0f);
    public static Vector3 fallVelocityRight = new Vector3(0f, 0f, 0f);
    public static Vector3 fallVelocityLeft = new Vector3(-0.4f, 0f, 0f);

    // Initial positions for the target at the start of each trial
    public static Vector3 targetPositionStartTrial = new Vector3(0f, 9f, 20f);
    public static Vector3 line2PositionStartTrial = new Vector3(0f, -0.2f, 3.77f);

    // Information about the subject and related files (placeholder for actual implementation)
}


public class AnswerManager
{

    // this class is intended to be used as an answer manager

    public static void ListenAndRecordAnswer()
    {

        string line_to_write = "" + Time.time + "\n"; // reaction time

        // to check tthe Target's falling direction
        // if Target falling straight
        Debug.Log((int)ExperimentGenerator.directionsIndex[ExperimentGenerator.trial_number]);
        Debug.Log((int)velocityDirection.Straight);

        if ((int)ExperimentGenerator.directionsIndex[ExperimentGenerator.trial_number] == (int)velocityDirection.Straight)
        {
            line_to_write += "Target Drection: Straight\n";
        }
        // if Target falling right
        if (ExperimentGenerator.directionsIndex[ExperimentGenerator.trial_number] == (int)velocityDirection.Right)
        {
            line_to_write += "Target Drection: Right\t\n";
        }
        // if Target falling left
        if (ExperimentGenerator.directionsIndex[ExperimentGenerator.trial_number] == (int)velocityDirection.Left)
        {
            line_to_write += "Target Drection: Left\t\n";
        }

        // for checking which of the arrows was pressed and write and record the message
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log(" DOWN ");
            line_to_write += "Subject Ans: Straight \n";
            ExperimentManager.writer.WriteLine(line_to_write); // send data to buffer to be writen on the file
            ExperimentManager.writer.Flush(); // endure the data go from buffer to the file
            ExperimentManager.answerFromSubjectRecorded = true; // true answer check
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log(" LEFT ");
            line_to_write += "Subject Ans: Left \n";
            ExperimentManager.writer.WriteLine(line_to_write); // send data to buffer to be writen on the file
            ExperimentManager.writer.Flush(); // endure the data go from buffer to the file
            ExperimentManager.answerFromSubjectRecorded = true; // true answer check
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log(" RIGHT ");
            line_to_write += "Subject Ans: Right \n";
            ExperimentManager.writer.WriteLine(line_to_write); // send data to buffer to be writen on the file
            ExperimentManager.writer.Flush(); // endure the data go from buffer to the file
            ExperimentManager.answerFromSubjectRecorded = true; // true answer check
            return;
        }
        else
        {
            ExperimentManager.answerFromSubjectRecorded = false; // subject did not answer
        }
    }
}


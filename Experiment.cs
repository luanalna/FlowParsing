using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

// wtf
public class ExperimentGenerator : MonoBehaviour
{
    // Canvas elements for different phases of the experiment
    public Canvas instructionsPanel;
    public Canvas questionPanel;
    public Canvas endPanel;

    // UI Button to proceed to the next block of trials
    public Button continueButton;

    // Reference to the draggable stick GameObject to be shown during the question phase
    public GameObject Stick;

    // Text UI for displaying the angle of the draggable stick
    public Text angleDisplay;

    // Other experimental components
    public Target target;
    public AutoRotateCamera viewCamera;

    // Experiment timing and control variables
    public float startTime = ExperimentManager.timeAtBeginningSpacebarPress;
    public float deltaTime = 0.0f;
    public static int trial_number = 0;
    public static int block_number = 1;
    float rotationSpeedRight = ExperimentManager.cameraRotationSpeedRight;
    float rotationSpeedLeft = ExperimentManager.cameraRotationSpeedLeft;

    // Arrays for holding the velocities and directions of the target object
    public static Vector3[] targetStartingVelocities;
    public static int[] directionsIndex = Enumerable.Repeat(0, ExperimentManager.numberOfTrials).ToArray();

    void Start()
    {
        // Set up button listener to proceed to next block when clicked
        continueButton.onClick.AddListener(ProceedToNextBlock);

        // Initially set the visibility of panels
        questionPanel.gameObject.SetActive(false);
        instructionsPanel.gameObject.SetActive(true);
        endPanel.gameObject.SetActive(false);

        // Initialize other experiment settings
        ExperimentManager.createParticles = true;
        ExperimentManager.startTargetFalling = false;
        targetStartingVelocities = GenerateRandomVectors(ExperimentManager.numberOfTrials);
        ExperimentManager.startCameraRotating = false;

        // Initially disable the draggable stick
        Stick.SetActive(false);
    }

    void Update()
    {
        // Update the time since the start of the experiment
        deltaTime = Time.time - startTime;

        // Start the experiment when spacebar is pressed and the experiment is not currently running
        if (Input.GetKeyDown(KeyCode.Space) && !ExperimentManager.experimentIsRunning)
        {
            StartExperiment();
        }

        // Handle the ongoing trial
        if (trial_number >= 0 && trial_number < ExperimentManager.numberOfTrials && ExperimentManager.experimentIsRunning)
        {
            RunTrial(trial_number);
        }

        // End the experiment after the last trial
        if (trial_number == ExperimentManager.numberOfTrials && ExperimentManager.experimentIsRunning)
        {
            endPanel.gameObject.SetActive(true);
            questionPanel.gameObject.SetActive(false);
        }
    }

    // Method to start the experiment
    void StartExperiment()
    {
        instructionsPanel.gameObject.SetActive(false);
        ExperimentManager.experimentIsRunning = true;
        startTime = Time.time;
        StartBlock();
    }

    // Method to start a new block of trials
    void StartBlock()
    {
        trial_number = 0; // Reset trial counter for this block

        // Determine camera rotation settings based on the current block number
        if (block_number == 1) // First block
        {
            ExperimentManager.startCameraRotating = true;
            viewCamera.UpdateBlockNumber(block_number);
        }
        else if (block_number == 2) // Second block
        {
            ExperimentManager.startCameraRotating = true;
            viewCamera.UpdateBlockNumber(block_number);
        }

        // Begin the current trial
        RunTrial(trial_number);
    }

    // Method to generate random vectors for target movement
    Vector3[] GenerateRandomVectors(int count)
    {
        Vector3[] vectors = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, 3);
            directionsIndex[i] = randomIndex;
            switch (randomIndex)
            {
                case 0:
                    vectors[i] = ExperimentManager.fallVelocityStraight;
                    break;
                case 1:
                    vectors[i] = ExperimentManager.fallVelocityRight;
                    break;
                case 2:
                    vectors[i] = ExperimentManager.fallVelocityLeft;
                    break;
            }
        }
        return vectors;
    }

    // Method to handle each trial
    void RunTrial(int trial_number)
    {
        deltaTime = Time.time - startTime;

        // Display the draggable stick when a trial is completed
        if (ExperimentManager.answerFromSubjectRecorded == true)
        {
            ShowAnimation();
        }

        // Record answer if the time to fall has passed and no answer was recorded
        if (deltaTime > ExperimentManager.timeTargetToFall && ExperimentManager.answerFromSubjectRecorded == false)
        {
            RecordAnswer();
        }
    }

    // Method to proceed to the next block of trials
    void ProceedToNextBlock()
    {
        block_number++;
        // Restart the experiment block
        endPanel.gameObject.SetActive(false);
        StartBlock();
    }

    // Method to show the animation and reinitialize the experiment state
    void ShowAnimation()
    {
        ExperimentManager.answerFromSubjectRecorded = false;
        ExperimentManager.experimentIsRunning = true;
        ExperimentManager.timeAtBeginningSpacebarPress = Time.time;
        endPanel.gameObject.SetActive(false);
        questionPanel.gameObject.SetActive(false);
        ExperimentManager.startCameraRotating = true;
        ExperimentManager.startTargetFalling = true;
        target.ResetTarget(ExperimentManager.targetPositionStartTrial, targetStartingVelocities[trial_number]);
        viewCamera.ResetCamera();
        startTime = Time.time;
        return;
    }

    // Method to activate the question panel and wait for an answer
    void RecordAnswer()
    {
        questionPanel.gameObject.SetActive(true);
        Stick.SetActive(true); // Activate the draggable stick during the question phase
        if (Input.anyKeyDown)
        {
            AnswerManager.ListenAndRecordAnswer();
            if (ExperimentManager.answerFromSubjectRecorded)
            {
                startTime = Time.time;
                trial_number += 1;
            }
        }
    }
}

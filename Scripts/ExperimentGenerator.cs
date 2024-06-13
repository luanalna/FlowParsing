using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExperimentGenerator : MonoBehaviour
{
    // Canvas
    public Canvas startPanel;
    public Canvas paddlePanel;
    public Canvas endPanel;
    public Canvas endBlockPanel;

    // Buttons
    public Button startButton;
    public Button continueButton;

    // Others
    public Target target;
    public CameraRotation viewCamera;
    public CSVManager CSVfile;
    public AdjustablePaddle adjustablePaddle;
    public Particles particles;

    // Timing Settings
    public static float timeTargetToFall = 3.0f; // time for the ball to fall after spacebar is pressed

    private bool isAnimationPlaying = false;
    private bool isWaitingForSubjectAnswer = false;
    private bool continueButtonPressed = false;
    private bool startButtonPressed = false;

    void Start()
    {
        // Initial Setup
        paddlePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        endPanel.gameObject.SetActive(false);
        particles.create = true;
        viewCamera.rotate = false;

        // Add button listeners
        continueButton.onClick.AddListener(OnContinueClick);
        startButton.onClick.AddListener(OnStartClick);

        // Update Output file name with subject information
       // CSVfile.UpdateSubjectInformationFile(Name_subject, Surname_subject, Number_subject);
    }

    void Update()
    {
        // Step 1: Start Canvas
        if (startButtonPressed)
        {
            if (CSVfile.ReadCSV_row())
            {
                // Assume CSVfile.TargetVelocity is the speed and CSVfile.FallAngle is the angle from the CSV file
                target.SetTarget(CSVfile.FallAngle, CSVfile.TargetVelocity);
                viewCamera.SetCamera(CSVfile.CameraDir);
                startPanel.gameObject.SetActive(false);
                target.startFalling();
                isAnimationPlaying = true;
                startButtonPressed = false; // Reset flag
                StartCoroutine(WaitForAnimation());
            }
        }

        // Step 2: On "Continue" Button Click
        if (continueButtonPressed && !isAnimationPlaying && !isWaitingForSubjectAnswer)
        {
            target.startFalling();
            viewCamera.rotate = true;
            continueButtonPressed = false; // Reset flag
            StartCoroutine(WaitForAnimation());
        }
    }

    IEnumerator WaitForAnimation()
    {
        // Step 3: Wait for the ball to fall (animation duration)
        yield return new WaitForSeconds(timeTargetToFall);
        target.stopFalling();
        viewCamera.ResetCamera();
        isAnimationPlaying = false;
        isWaitingForSubjectAnswer = true;

        // Step 4: Show adjustable paddle canvas
        paddlePanel.gameObject.SetActive(true);
    }

    void OnContinueClick()
    {
        // Step 5: Continue Button Click for Next Trial
        if (isWaitingForSubjectAnswer)
        {
            float responseAngle = adjustablePaddle.angleSlider.value; // Get the angle from the slider
            CSVfile.UpdateCSVWithAngle(responseAngle); // Update CSV with the provided response angle

            paddlePanel.gameObject.SetActive(false); // Deactivate answer panel
            isWaitingForSubjectAnswer = false; // Reset flag

            // Reset the slider to 0 after the subject has answered
            adjustablePaddle.ResetSlider();

            if (CSVfile.ReadCSV_row())
            {
                target.SetTarget(CSVfile.FallAngle, CSVfile.TargetVelocity);
                viewCamera.SetCamera(CSVfile.CameraDir);
                target.startFalling();
                isAnimationPlaying = true;
                StartCoroutine(WaitForAnimation());
            }
            else
            {
                endPanel.gameObject.SetActive(true);
            }
        }
        else
        {
            continueButtonPressed = true;
        }
    }

    void OnStartClick()
    {
        startButtonPressed = true;
    }
}

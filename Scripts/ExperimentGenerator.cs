using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperimentGenerator : MonoBehaviour
{
    // Canvas
    public Canvas startPanel;
    public Canvas paddlePanel;
    public Canvas endPanel;
    public Canvas endBlockPanel;

    // Buttons
    public Button startButton;
    public Button endButton;
    public Button endBlockButton;
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
    private bool endButtonPressed = false;
    private bool nextBlockButtonPressed = false;

    void Start()
    {
        // Initial Setup
        paddlePanel.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
        endPanel.gameObject.SetActive(false);
        endBlockPanel.gameObject.SetActive(false);
        particles.create = true;

        // Add button listeners
        continueButton.onClick.AddListener(OnContinueClick);
        startButton.onClick.AddListener(OnStartClick);
        endButton.onClick.AddListener(OnEndClick);
        endBlockButton.onClick.AddListener(OnNextBlockClick);
    }

    void Update()
    {
        // Step 1: Start Canvas
        if (startButtonPressed)
        {
            if (CSVfile.ReadCSV_row())
            {
                target.SetTarget(CSVfile.TargetDir);
                target.SetDepth(CSVfile.DepthView); // Set the depth from CSV
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
            float angle = adjustablePaddle.angleSlider.value; // Get the angle from the slider
            CSVfile.UpdateCSVWithAngle(angle); // Update CSV with the provided angle

            paddlePanel.gameObject.SetActive(false);
            isWaitingForSubjectAnswer = false;
            if (CSVfile.ReadCSV_row())
            {
                target.SetTarget(CSVfile.TargetDir);
                target.SetDepth(CSVfile.DepthView); // Set the depth from CSV
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

    void OnNextBlockClick()
    {
        nextBlockButtonPressed = true;
    }

    void OnEndClick()
    {
        endButtonPressed = true;
    }
}

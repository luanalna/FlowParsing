using UnityEngine;
using UnityEngine.UI;

public class AdjustablePaddle : MonoBehaviour
{
    public RectTransform draggableStick; // Reference to the draggable stick
    public Slider angleSlider;           // Reference to the Slider

    void Start()
    {
        // Initialize the slider's value to match the current angle of the stick
        float initialAngle = draggableStick.localEulerAngles.z;
        angleSlider.value = initialAngle;

        // Add a listener to call the UpdateStickAngle method whenever the slider value changes
        angleSlider.onValueChanged.AddListener(UpdateStickAngle);
    }

    void UpdateStickAngle(float value)
    {
        // Rotate the stick based on the slider value
        draggableStick.localEulerAngles = new Vector3(0, 0, value);
    }
        public void ResetSlider()
    {
        // Reset the slider to 0 and update the stick angle accordingly
        angleSlider.value = 0;
        UpdateStickAngle(0);
    }
}
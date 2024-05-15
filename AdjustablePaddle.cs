using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdjustablePaddle : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform stableStick;  // Assign in the inspector
    public RectTransform draggableStick;  // Assign in the inspector
    public Slider sliderTop;  // Assign in the inspector
    public Slider sliderBottom;  // Assign in the inspector
    public Text angleDisplay;  // Assign in the inspector

    private bool isDraggingTop = false;
    private bool isDraggingBottom = false;

    void Start()
    {
        sliderTop?.onValueChanged.AddListener(value => OnSliderValueChanged(0, value));
        sliderBottom?.onValueChanged.AddListener(value => OnSliderValueChanged(1, value));
    }

    void OnDestroy()
    {
        sliderTop?.onValueChanged.RemoveAllListeners();
        sliderBottom?.onValueChanged.RemoveAllListeners();
    }

    private void OnSliderValueChanged(int index, float value)
    {
        var child = draggableStick.GetChild(index).GetComponent<RectTransform>();
        child.anchoredPosition = new Vector2(child.anchoredPosition.x, value);
        UpdateAngleDisplay();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraggingTop = RectTransformUtility.RectangleContainsScreenPoint(draggableStick.GetChild(0).GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera);
        isDraggingBottom = RectTransformUtility.RectangleContainsScreenPoint(draggableStick.GetChild(1).GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDraggingTop || isDraggingBottom)
        {
            int index = isDraggingTop ? 0 : 1;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(draggableStick, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            draggableStick.GetChild(index).GetComponent<RectTransform>().anchoredPosition = localPoint;
            UpdateAngleDisplay();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDraggingTop = isDraggingBottom = false;
    }

    private void UpdateAngleDisplay()
    {
        Vector3 topPosition = draggableStick.GetChild(0).GetComponent<RectTransform>().anchoredPosition;
        Vector3 bottomPosition = draggableStick.GetChild(1).GetComponent<RectTransform>().anchoredPosition;
        Vector3 stickDirection = (topPosition - bottomPosition).normalized;
        float angle = Vector3.Angle(stableStick.up, stickDirection);
        angleDisplay.text = $"Angle: {angle:F2}°";
    }
}

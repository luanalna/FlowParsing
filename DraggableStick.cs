using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Stick : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float mouseZCoord;

    void OnMouseDown()
    {
        if (!IsPointerOverUIObject())
        {
            mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mouseOffset = gameObject.transform.position - GetMouseWorldPos();
        }
    }

    void OnMouseDrag()
    {
        if (!IsPointerOverUIObject())
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GetMouseWorldPos().z + mouseOffset.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

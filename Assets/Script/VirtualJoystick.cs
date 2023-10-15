using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour
{
    public static UnityEvent onInput = new UnityEvent();
    public static Vector3 input { get; set; }

    [SerializeField]
    private float maxMagnitude = 100f;
    [SerializeField]
    private GameObject virtualJoystick;
    [SerializeField]
    private GameObject virtualJoystickHandle;
    [SerializeField]
    private GameObject virtualJoystickArrowPivot;

    [SerializeField]
    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    private Vector3 downPos;
    private Vector3 dragPos;
    private Vector3 dir;
    private float magnitude;
    private bool isMouseButtonDownOnUI;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseButtonDownOnUI = IsMouseButtonDownOnUI();
            if (isMouseButtonDownOnUI)
                return;
            downPos = Input.mousePosition;
            virtualJoystick.transform.position = downPos;
            virtualJoystick.SetActive(true);
        }
        if (Input.GetMouseButton(0))
        {
            if (isMouseButtonDownOnUI)
                return;
            dragPos = Input.mousePosition;
            dir = Vector3.Normalize(dragPos - downPos);
            magnitude = Vector3.Magnitude(dragPos - downPos);
            if (magnitude > maxMagnitude)
            {
                downPos += dir * (magnitude - maxMagnitude);
                virtualJoystick.transform.position = downPos;
            }
            input = dir * magnitude / maxMagnitude;
            input = new Vector3(input.x, 0f, input.y);
            onInput.Invoke();
            virtualJoystickHandle.transform.position = downPos + dir * magnitude;

            float angle = Mathf.Atan2(dragPos.y - virtualJoystickArrowPivot.transform.position.y, dragPos.x - virtualJoystickArrowPivot.transform.position.x) * Mathf.Rad2Deg;
            virtualJoystickArrowPivot.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
        if (Input.GetMouseButtonUp(0))
        {
            input = Vector3.zero;
            virtualJoystick.SetActive(false);
            onInput.Invoke();
        }
    }

    private bool IsMouseButtonDownOnUI()
    {
        _pointerEventData = new PointerEventData(_eventSystem);
        _pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(_pointerEventData, results);
        if (results.Count > 0)
            return true;
        return false;
    }
}
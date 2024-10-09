using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform joystickHandle; // Cần điều khiển
    [SerializeField] private RectTransform joystickBackground; // Nền joystick

    private Vector2 inputVector;

    public Vector2 GetInputVector()
    {
        return inputVector;
    }

    // Khi người chơi nhấn vào joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Kích hoạt Drag ngay khi nhấn xuống
    }

    // Khi người chơi nhả joystick
    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero; // Đặt lại input về 0 khi nhả
        joystickHandle.anchoredPosition = Vector2.zero; // Đưa cần điều khiển về giữa
    }

    // Khi người chơi kéo joystick
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 joystickPosition;
        // Chuyển đổi tọa độ điểm nhấn chuột sang hệ tọa độ cục bộ của joystick background
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out joystickPosition);

        float maxDistance = joystickBackground.sizeDelta.x / 2;

        // Giới hạn khoảng cách của cần điều khiển
        if (joystickPosition.magnitude > maxDistance)
        {
            joystickPosition = joystickPosition.normalized * maxDistance;
        }

        inputVector = joystickPosition / maxDistance;

        // Di chuyển cần điều khiển trong giới hạn
        joystickHandle.anchoredPosition = joystickPosition;
    }
}

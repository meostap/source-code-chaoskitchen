using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayeJoystickmm : MonoBehaviour
{
    Vector2 movevector;
    public float movespeed;

    public void InputPlayer(InputAction.CallbackContext context)
    {
        movevector = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(movevector.x, 0, movevector.y);
        movement.Normalize();
        transform.Translate(movespeed * movement * Time.deltaTime);
    }
}

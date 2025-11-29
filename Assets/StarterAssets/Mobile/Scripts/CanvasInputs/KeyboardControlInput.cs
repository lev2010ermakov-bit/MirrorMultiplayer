using System;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardControlInput : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [System.Serializable]
    public class KeyEvent : UnityEvent<Boolean> { }

    [Header("Output")]
    public Event WASDOutputEvent;

    public KeyEvent JumpEvent;

    public KeyEvent SptintEvent;

    private void Update()
    {
        Vector2 InputVector = new Vector2(Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0), Input.GetKey(KeyCode.S) ? -1 : (Input.GetKey(KeyCode.W) ? 1 : 0));

        WASDOutputEvent.Invoke(InputVector);
        JumpEvent.Invoke(Input.GetKey(KeyCode.Space));
        SptintEvent.Invoke(Input.GetKey(KeyCode.LeftShift));
    }
}

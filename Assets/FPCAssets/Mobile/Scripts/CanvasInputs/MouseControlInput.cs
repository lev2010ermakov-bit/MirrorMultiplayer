using UnityEngine;
using UnityEngine.Events;

public class MouseControlInput : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [SerializeField] private float Multipler;

    [Header("Output")]
    public Event MouseOutputEvent;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector2 MouseInputVector = new Vector2(Input.mousePositionDelta.x,-Input.mousePositionDelta.y);
        MouseOutputEvent.Invoke(MouseInputVector * Multipler);
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = Cursor.visible == true ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }
    }
}

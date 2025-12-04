using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEventSystemHandler, IPointerClickHandler
{

    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    private const int MaxClickDelta = 20;

    [SerializeField] private AnimationCurve _viewCurve;

    private Vector2 _delta;
    private Vector2 _force = default(Vector2);
    private bool _isDrag = false;
    private bool _isActive = false;
    private float _xPosition;
    private float _yPosition;
    private float _clickDelta = 0;

    public float Drag = 0.02f;
    public float Multiplier = 1f;
    public float _direction;

    public UnityEvent OnTouch;
    public UnityEvent OnRelease;

    public static Vector2 Direction;
    public static bool IsClick = false;

    [Header("Output")]
    public Event touchZoneOutputEvent;

    private void OnEnable()
    {
        _isActive = false;
        _force = default(Vector2);
        IsClick = false;
        StartCoroutine(WaitActivate());
    }

    public void OnDrag(PointerEventData eventData)
    {
        _delta = eventData.delta;
        _force += _delta * Time.deltaTime * Multiplier;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _clickDelta = 0;
        _isDrag = false;
        if (OnTouch != null)
        {
            OnTouch.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _delta = Vector2.zero;
        _isDrag = true;
        if (OnRelease != null)
        {
            OnRelease.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickDelta < MaxClickDelta)
        {
            IsClick = true;
        }
        _clickDelta = 0;
    }

    public static void DeactivateClick()
    {
        IsClick = false;
    }

    private void Update()
    {

        touchZoneOutputEvent.Invoke(new Vector2(_delta.x, -_delta.y)/Multiplier);
        if (_isActive == false)
        {
            Direction = Vector2.zero;
            return;
        }
        _clickDelta += _delta.sqrMagnitude;
        float smoothMultiplier = 2f;
        _isDrag = _delta.sqrMagnitude > 0.15f;
        if (_isDrag == false)
            smoothMultiplier = 16f;
        if (_isDrag == true)
            _force = Vector2.Lerp(_force, Vector2.zero, Time.deltaTime * smoothMultiplier);
        else
            _force = Vector2.zero;
        UpdateAcceleration(_force);
        CalculateView();
        _delta = Vector2.zero;
        _direction = Direction.sqrMagnitude;
    }

    private void CalculateView()
    {
        float inputX = _xPosition;
        float directionX = Mathf.Sign(inputX);
        float accelerationX = _viewCurve.Evaluate(inputX * directionX);
        float horizontalSpeed = 3f;
        float deltaX = accelerationX * directionX * horizontalSpeed;
        float inputY = _yPosition;
        float directionY = Mathf.Sign(inputY);
        float accelerationY = 0f - _viewCurve.Evaluate(inputY * directionY);
        float deltaY = accelerationY * directionY * 1f;
        Direction = new Vector2(deltaX, deltaY);
    }

    private void UpdateAcceleration(Vector2 force)
    {
        _xPosition = Mathf.Round(force.x * 100f) / 100f;
        _yPosition = Mathf.Round(force.y * 100f) / 100f;
    }

    private IEnumerator WaitActivate()
    {
        yield return new WaitForSeconds(0.3f);
        _isActive = true;
    }
}

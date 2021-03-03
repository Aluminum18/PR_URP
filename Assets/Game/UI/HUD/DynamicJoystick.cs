using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DynamicJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Reference")]
    [SerializeField]
    private Vector3Variable _rawInputJoystick;
    [SerializeField]
    private Vector3Variable _joystickDirection;

    [Header("Events out")]
    [SerializeField]
    private UnityEvent _onPointerDown;
    [SerializeField]
    private UnityEvent _onDrag;
    [SerializeField]
    private UnityEvent _onPointerUp;

    [Header("Config")]
    [SerializeField]
    private float _fadeInTime;
    [SerializeField]
    private float _fadeOutTime;
    [Tooltip("Joystick up directs to forward")]
    [SerializeField]
    private Transform _controller;
    [SerializeField]
    private Transform _joystick;
    [SerializeField]
    private bool _mapUpToForward;
    [SerializeField]
    private float _triggerDistance;
    [SerializeField]
    private float _litmitDistance;
    [SerializeField]
    private Camera _uiCam;
    [SerializeField]
    private Transform _worldCamTransform;
    [SerializeField]
    private Canvas _parentCanvas;

    private float _directionX = 0;
    private float _directionY = 0;

    private Vector3 _centerPos;
    private Vector3 _positionVector;
    private float A;
    private float B;

    private float Ai;
    private float Bi;
    private float Ci;
    private float _distanceSqr;

    private void Start()
    {
        _centerPos = _joystick.position;
        _distanceSqr = _litmitDistance * _litmitDistance;
    }

    public void SetDirectionOutHolder(Vector3Variable directionHolder)
    {
        _joystickDirection = directionHolder;
    }

    public void SetRawJoyStickInputHolder(Vector3Variable rawInputHolder)
    {
        _rawInputJoystick = rawInputHolder;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPointToWorld = _uiCam.ScreenToWorldPoint(eventData.position);
        _positionVector = touchPointToWorld;
        _positionVector.z = touchPointToWorld.z + _parentCanvas.planeDistance;

        if (Vector3.Distance(_centerPos, _positionVector) < _triggerDistance)
        {
            _rawInputJoystick.Value = Vector3.zero;
            _joystickDirection.Value = Vector3.zero;
            _joystick.position = _positionVector;
            return;
        }

        if (Vector3.Distance(_centerPos, _positionVector) >= _litmitDistance)
        {
            CalculateOverFlow(touchPointToWorld);
        }

        _joystick.position = _positionVector;
        _directionX = (_joystick.position.x - _centerPos.x) / _litmitDistance;
        _directionY = (_joystick.position.y - _centerPos.y) / _litmitDistance;

        _rawInputJoystick.Value = (_positionVector - _centerPos) / _litmitDistance;

        _joystickDirection.Value = _worldCamTransform.right * _directionX +
           (_mapUpToForward ? _worldCamTransform.forward : _worldCamTransform.up) * _directionY;

        _onDrag.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FadeIn();

        Vector3 touchPointToWorld = _uiCam.ScreenToWorldPoint(eventData.position);
        _positionVector = touchPointToWorld;
        _positionVector.z = touchPointToWorld.z + _parentCanvas.planeDistance;

        _centerPos = _positionVector;
        _controller.position = _positionVector;
        _onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _onPointerUp.Invoke();
        BackToCenter();
        FadeOut();
    }

    private void CalculateOverFlow(Vector2 touchPoint)
    {
        float x = touchPoint.x - _centerPos.x;
        float y = touchPoint.y - _centerPos.y;

        if (Mathf.Abs(x) < 0.00000001f)
        {
            _positionVector.x = _centerPos.x;
            _positionVector.y = _centerPos.y + 1f * Mathf.Sign(y) * _litmitDistance;
            return;
        }

        if (Mathf.Abs(y) < 0.00000001f)
        {
            _positionVector.y = _centerPos.y;
            _positionVector.x = _centerPos.x + 1 * Mathf.Sign(x) * _litmitDistance;
            return;
        }

        float tan = y / x;

        float tanSqr = Mathf.Pow(tan, 2);

        float xSqr = 1 / (tanSqr + 1);
        float ySqr = tanSqr * xSqr;

        _positionVector.x = Mathf.Sqrt(xSqr) * Mathf.Sign(x) * _litmitDistance + _centerPos.x;
        _positionVector.y = Mathf.Sqrt(ySqr) * Mathf.Sign(y) * _litmitDistance + _centerPos.y;
    }

    private void BackToCenter()
    {
        LeanTween.move(_joystick.gameObject, _centerPos, 0.2f).setEase(LeanTweenType.linear);
        _joystickDirection.Value = Vector3.zero;
        _rawInputJoystick.Value = Vector3.zero;
    }

    private void FadeIn()
    {
        LeanTween.alphaCanvas(_controller.gameObject.GetComponent<CanvasGroup>(), 1f, _fadeOutTime).setEase(LeanTweenType.easeOutQuad);
    }

    private void FadeOut()
    {
        LeanTween.alphaCanvas(_controller.gameObject.GetComponent<CanvasGroup>(), 0f, _fadeOutTime).setEase(LeanTweenType.easeInQuad);
    }
}

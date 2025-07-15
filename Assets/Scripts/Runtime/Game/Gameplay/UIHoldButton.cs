using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public event Action OnHold;

    [SerializeField] private float _repeatRate = 0.05f;

    private bool _isHeld = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHeld = true;
        InvokeRepeating(nameof(InvokeHold), 0f, _repeatRate);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isHeld = false;
        CancelInvoke(nameof(InvokeHold));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHeld = false;
        CancelInvoke(nameof(InvokeHold));
    }

    private void InvokeHold()
    {
        if (_isHeld)
        {
            OnHold?.Invoke();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UnityEvent OnMouseHoverStart;
    [SerializeField] private UnityEvent OnMouseHoverEnd;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseHoverStart?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseHoverEnd?.Invoke();
    }
}
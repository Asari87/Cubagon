using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonEvent { Hover, Pressed}
public class UIButtonEventNotifier : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{    
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.HandleButtonEvent(ButtonEvent.Pressed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.HandleButtonEvent(ButtonEvent.Hover);
    }

}

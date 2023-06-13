using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileValidator : MonoBehaviour
{
    private void Awake()
    {
        bool isOnMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        gameObject.SetActive(isOnMobile);
    }
}

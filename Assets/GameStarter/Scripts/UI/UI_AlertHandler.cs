using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public enum AlertType { Confimation, Decision}
public class UI_AlertHandler : MonoBehaviour
{
    public static UI_AlertHandler instance;
    [SerializeField] private GameObject alertPanel;
    [SerializeField] private TMP_Text alertTitle;
    [SerializeField] private TMP_Text alertMessage;
    [SerializeField] private Button noButton;
    [SerializeField] private Button yesButton;

    public Action confirmCallback;
    public Action cancelCallback;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        yesButton.onClick.AddListener(HandleConfirmButton);
        noButton.onClick.AddListener(HandleCancelButton);
    }

    private void HandleConfirmButton()
    {
        if(confirmCallback != null)
            confirmCallback?.Invoke();
        HideWindow();
    }

    private void HandleCancelButton()
    {
        if(cancelCallback != null)
            cancelCallback?.Invoke();
        HideWindow();
    }

    public void ShowMessage(string title, string message)
    {
        alertTitle.text = title;
        alertMessage.text = message;
        yesButton.onClick.AddListener(() => HideWindow());
        noButton.gameObject.SetActive(false);
        alertPanel.SetActive(true);

    }
    public void ShowMessage(string title, string message, Action confirmCallback)
    {
        alertTitle.text = title;
        alertMessage.text = message;
        this.confirmCallback = confirmCallback;
        noButton.gameObject.SetActive(false);
        alertPanel.SetActive(true);

    }
    public void ShowMessage(string title, string message, Action confirmCallback, Action cancelCallback)
    {
        alertTitle.text = title;
        alertMessage.text = message;
        this.confirmCallback = confirmCallback;
        this.cancelCallback = cancelCallback;
        noButton.gameObject.SetActive(true);
        alertPanel.SetActive(true);

    }

    private void HideWindow()
    {
        alertPanel.SetActive(false);
        confirmCallback = null;
        cancelCallback = null;
    }
    private void OnDisable()
    {
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.RemoveAllListeners();
    }

}

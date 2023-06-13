using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecisionUI : MonoBehaviour
{
    public static DecisionUI Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text header;
    [SerializeField] TMP_Text description;
    [SerializeField] Button okBtn;
    [SerializeField] Button noBtn;
    private Action<bool> _callback;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        panel.SetActive(false);
        okBtn.onClick.AddListener(delegate { ClosePanel(true); });
        noBtn.onClick.AddListener(delegate { ClosePanel(false); });
    }
    public void ShowMessage(string head, string desc, Action<bool> callback)
    {
        panel.SetActive(true);
        header.text = head;
        description.text = desc;
        _callback = callback;
    }
    
    private void ClosePanel(bool decition)
    {
        panel.SetActive(false);
        _callback?.Invoke(decition);
    }


    private void OnDestroy()
    {
        okBtn.onClick.RemoveAllListeners();
    }
}

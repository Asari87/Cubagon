using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : MonoBehaviour
{
    public static WarningUI Instance;
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text header;
    [SerializeField] TMP_Text description;
    [SerializeField] Button okBtn;
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
        okBtn.onClick.AddListener(ClosePanel);
    }
    public void ShowMessage(string head, string desc)
    {
        panel.SetActive(true);
        header.text = head;
        description.text = desc;
    }
    private void ClosePanel()
    {
        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        okBtn.onClick.RemoveAllListeners();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    private void Awake()
    {
        playBtn.onClick.AddListener(() => SceneHandler.Instance.LoadScene(1));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button p1Button;
    [SerializeField] private Button p2Button;

    [Header("Sprites")]
    [SerializeField] private Sprite humanSprite;
    [SerializeField] private Sprite aiSprite;

    private GameSettingSO gameSettings;

    private void Awake()
    {
        gameSettings = Resources.Load<GameSettingSO>(Constants.GAME_SETTING);
        gameSettings.p1Type = PlayerType.Human;
        gameSettings.p2Type = PlayerType.Human;

        p1Button.onClick.AddListener(() => SwitchP1Type());
        p2Button.onClick.AddListener(() => SwitchP2Type());

        //Initialize
        SwitchP1Type();
        SwitchP2Type();
    }

    private void OnDestroy()
    {
        p1Button.onClick.RemoveAllListeners();
        p2Button.onClick.RemoveAllListeners();
    }

    private void SwitchP2Type()
    {
        gameSettings.p2Type = GetOpposite(gameSettings.p2Type);
        p2Button.image.sprite = GetSprite(gameSettings.p2Type);
    }

    private void SwitchP1Type()
    {
        gameSettings.p1Type = GetOpposite(gameSettings.p1Type);
        p1Button.image.sprite = GetSprite(gameSettings.p1Type);
    }

    private Sprite GetSprite(PlayerType type)
    {
        switch(type)
        {
            default:
            case PlayerType.Human:
                return humanSprite;
            case PlayerType.AI:
                return aiSprite;
        }
    }

    private PlayerType GetOpposite(PlayerType type)
    {
        switch (type)
        {
            default:
            case PlayerType.Human:
                return PlayerType.AI;
            case PlayerType.AI:
                return PlayerType.Human;
        }
    }
}

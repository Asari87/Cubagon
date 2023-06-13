using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private Button effectsBtn;
    [SerializeField] private Button musicBtn;

    [SerializeField] private Slider effectSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField] private Button backBtn;

    [Header("Button Sprites")]
    [SerializeField] private Sprite effectsOn;
    [SerializeField] private Sprite effectsOff;
    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;

    private Image effectsImage;
    private Image musicImage;

    private bool effectsMute = false;
    private bool musicMute = false;
    private void Awake()
    {
        effectsImage = effectsBtn.GetComponent<Image>();
        musicImage = musicBtn.GetComponent<Image>();
        if(PlayerPrefs.HasKey(SoundPrefsKeys.EffectMute.ToString()))
        {
            effectsMute = PlayerPrefs.GetInt(SoundPrefsKeys.EffectMute.ToString()) == 1;
            UpdateUI();
        }
        if(PlayerPrefs.HasKey(SoundPrefsKeys.MusicMute.ToString()))
        {
            musicMute = PlayerPrefs.GetInt(SoundPrefsKeys.MusicMute.ToString()) == 1;
            UpdateUI();
        }

        if(PlayerPrefs.HasKey(SoundPrefsKeys.EffectVolume.ToString()))
        {
            effectSlider.value = PlayerPrefs.GetFloat(SoundPrefsKeys.EffectVolume.ToString());
        }
        if(PlayerPrefs.HasKey(SoundPrefsKeys.MusicVolume.ToString()))
        {
            musicSlider.value = PlayerPrefs.GetFloat (SoundPrefsKeys.MusicVolume.ToString());
        }




        effectsBtn.onClick.AddListener(() => 
        { 
            SoundManager.Instance.ToggleEffects(); 
            effectsMute= !effectsMute;
            UpdateUI();
        });
        musicBtn.onClick.AddListener(() => 
        { 
            SoundManager.Instance.ToggleMusic();
            musicMute = !musicMute;
            UpdateUI();
        });

        effectSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetEffectsVolume(value));
        musicSlider.onValueChanged.AddListener((value) => SoundManager.Instance.SetMusicVolume(value));

        backBtn.onClick.AddListener(() => SoundManager.Instance.SaveChanges());
    }


    private void UpdateUI()
    {
        effectsImage.sprite = effectsMute ? effectsOff : effectsOn;
        musicImage.sprite = musicMute ? musicOff : musicOn;

    }



}

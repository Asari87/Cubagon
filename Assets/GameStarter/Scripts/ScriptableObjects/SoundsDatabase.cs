using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds all global sounds to be used in SoundManager
/// </summary>
/// <remarks>
/// Individual sounds may be kept elsewhere
/// </remarks>
[CreateAssetMenu()]
public class SoundsDatabase : ScriptableObject
{

    [Header("Ambiance")]
    public AudioClip[] menuAmbience;
    public LevelAudio[] levelsAmbiance;
    public AudioClip[] gameOverAmbience;

    [Header("UI Buttons")]
    public AudioClip buttonHover;
    public AudioClip buttonPress;

    [Header("Player")]
    public AudioClip playerDeath;

}

[Serializable]
public class LevelAudio
{
    public string sceneName;
    public AudioClip audio;
}

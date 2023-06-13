using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes { IntroScene, Level, GameOverScene }
/// <summary>
/// Swap scenes on requests.
/// </summary>
/// <remarks>
/// Makes use of Fader to fade in and out of scenes
/// </remarks>
public class SceneHandler : MonoBehaviour
{
    public static SceneHandler Instance;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float fadeInTime;
    private string[] sceneNames;
    private Fader fader;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        InitalizeSceneList();
        fader = GetComponentInChildren<Fader>();

        //local
        void InitalizeSceneList()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            sceneNames = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                sceneNames[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }
        }
    }

    #region Unload 
    public Coroutine UnloadScene(string sceneName)
    {
        return StartCoroutine(Unload(sceneName));
    }

    private IEnumerator Unload(string sceneName)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        while (unloadOp.progress < .9f)
        {
            yield return null;
        }
        Debug.Log($"{sceneName} unload: completed");
    }

    public Coroutine UnloadScene(int sceneIndex)
    {
        return StartCoroutine(Unload(sceneIndex));
    }
    private IEnumerator Unload(int sceneIndex)
    {
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneIndex);
        while (unloadOp.progress < .9f)
        {
            yield return null;
        }
        Debug.Log($"Scene {sceneIndex} unload: completed");
    }
    #endregion

    #region Load
    public void LoadScene(string nextSceneName, bool additive = false)
    {
        if (additive)
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
            return;
        }
        StartCoroutine(SwtichScene(nextSceneName.ToString()));
    }
    public void LoadScene(int nextSceneIndex, bool additive = false)
    {
        if (additive)
        {
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);
            return;
        }
        StartCoroutine(SwtichScene(nextSceneIndex));
    }
    private IEnumerator SwtichScene(string nextScene)
    {
        yield return fader.FadeOut(fadeOutTime);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        while (op.progress < .9f)
        {
            yield return null;
        }

        yield return fader.FadeIn(fadeInTime);
    }
    private IEnumerator SwtichScene(int nextScene)
    {
        yield return fader.FadeOut(fadeOutTime);
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        while (op.progress < .9f)
        {
            yield return null;
        }

        yield return fader.FadeIn(fadeInTime);
    }
    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion


    public void SetActiveScene(Scene scene)
    {
        SceneManager.SetActiveScene(scene);
    }
}
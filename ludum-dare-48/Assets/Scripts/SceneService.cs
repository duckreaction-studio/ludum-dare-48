using DG.Tweening;
using DuckReaction.Common.Container;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneService : MonoBehaviour
{
    [SerializeField]
    string[] _firstScenes = { "Scenes/Home" };

    [SerializeField]
    Image _imageEffect;

    [SerializeField]
    float _transitionDuration = 0.5f;

    List<string> _scenesToUnload;
    List<string> _scenesToLoad;

    void Start()
    {
        _imageEffect.color = Color.black;

        StartSceneTransition(new string[0], _firstScenes, false);
    }

    public void StartSceneTransition(string[] scenesToUnload, string[] scenesToLoad, bool fadeIn = true)
    {
        _scenesToUnload = new List<string>(scenesToUnload);
        _scenesToLoad = new List<string>(scenesToLoad);

        if (fadeIn)
            _imageEffect.DOFade(1, _transitionDuration).onComplete += ProcessNextScene;
        else
            ProcessNextScene();
    }

    private void OnSceneLoadedOrUnloaded(AsyncOperation op)
    {
        ProcessNextScene();
    }

    private void ProcessNextScene()
    {
        if (_scenesToUnload.Count == 0 && _scenesToLoad.Count == 0)
        {
            OnScenesLoaded();
        }
        else if (_scenesToUnload.Count == 0)
        {
            LoadNextScene();
        }
        else
        {
            UnloadNextScene();
        }
    }

    private void UnloadNextScene()
    {
        var scene = _scenesToUnload.Shift();
        SceneManager.UnloadSceneAsync(scene).completed += OnSceneLoadedOrUnloaded;
    }

    private void LoadNextScene()
    {
        var scene = _scenesToLoad.Shift();
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += OnSceneLoadedOrUnloaded;
    }

    private void OnScenesLoaded()
    {
        _imageEffect.DOFade(0, _transitionDuration).onComplete += OnFadeInComplete;
    }

    private void OnFadeInComplete()
    {
        Debug.Log("Fade in complete");
    }
}

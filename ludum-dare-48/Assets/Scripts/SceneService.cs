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
    float _targetY = 500;

    [SerializeField]
    float _transitionDuration = 0.5f;

    List<string> _scenesToUnload;
    List<string> _scenesToLoad;

    void Start()
    {
        _imageEffect.color = Color.black;
        Vector2 target = ((RectTransform)_imageEffect.transform).anchoredPosition;
        target.y = _targetY;

        StartSceneTransition(new string[0], _firstScenes, false);
    }

    public void StartSceneTransition(string[] scenesToUnload, string[] scenesToLoad, bool fadeIn = true)
    {
        _scenesToUnload = new List<string>(scenesToUnload);
        _scenesToLoad = new List<string>(scenesToLoad);

        if (fadeIn)
            FadeInAnimation();
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
            FadeOutAnimation();
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

    [ContextMenu("Test fade in")]
    private void FadeInAnimation()
    {
        //_imageEffect.DOFade(1, _transitionDuration).onComplete += ProcessNextScene;
        Vector2 target = ((RectTransform)_imageEffect.transform).anchoredPosition;
        target.y = 0;
        ((RectTransform)_imageEffect.transform).DOAnchorPos(target, _transitionDuration).onComplete += ProcessNextScene;
    }

    [ContextMenu("Test fade out")]
    private void FadeOutAnimation()
    {
        //_imageEffect.DOFade(0, _transitionDuration).onComplete += OnFadeInComplete;
        Vector2 target = ((RectTransform)_imageEffect.transform).anchoredPosition;
        target.y = _targetY;
        ((RectTransform)_imageEffect.transform).DOAnchorPos(target, _transitionDuration);
    }

    private void OnFadeInComplete()
    {
        Debug.Log("Fade in complete");
    }
}

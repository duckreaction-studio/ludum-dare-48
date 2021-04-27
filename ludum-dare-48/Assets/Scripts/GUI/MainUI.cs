using Core;
using DuckReaction.Common;
using GUI;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MainUI : GameBehaviour
{
    [SerializeField]
    TMP_Text _level;
    [SerializeField]
    TMP_Text _score;
    [SerializeField]
    AnimatedPanel _animatedPanel;
    [SerializeField]
    GameObject _gameOver;
    [SerializeField]
    TMP_Text _finalScore;

    NumberFormatInfo _numberFormat;

    protected void Awake()
    {
        _gameOver.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();

        _numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        _numberFormat.NumberGroupSeparator = " ";
    }

    protected override void Update()
    {
        base.Update();
        _level.text = "Level " + _gameState.level;
        _score.text = _gameState.totalScore.ToString(_numberFormat);
    }

    protected override void OnGameOver()
    {
        _finalScore.text = _gameState.totalScore.ToString(_numberFormat);
        _gameOver.SetActive(true);
    }

    public void Pause()
    {
        if (_gameState.isStartedOrRunning())
            PauseInner();
        else
            Resume();
    }

    public void Resume()
    {
        _animatedPanel.Hide();
        _gameState.Resume();
    }

    private void PauseInner()
    {
        _animatedPanel.Show();
        _gameState.Pause();
    }

    public void Restart()
    {
        _animatedPanel.Hide();
        _gameState.Restart();
        _gameOver.SetActive(false);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Screen.fullScreen = false;
    Application.OpenURL("about:blank");
#else
        Application.Quit();
#endif
    }
}

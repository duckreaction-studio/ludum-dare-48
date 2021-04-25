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

    NumberFormatInfo _numberFormat;

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
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

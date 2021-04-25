using Core;
using DuckReaction.Common;
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
        if (_gameState.isRunning())
            _gameState.Pause();
        else
            _gameState.Resume();
    }
}

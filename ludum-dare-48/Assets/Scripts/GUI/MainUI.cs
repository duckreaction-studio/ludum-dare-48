using Core;
using DuckReaction.Common;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MainUI : GameBehaviour
{
    [SerializeField]
    TMP_Text _score;

    NumberFormatInfo _numberFormat;

    protected void Start()
    {
        _numberFormat = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        _numberFormat.NumberGroupSeparator = " ";
    }

    protected override void Update()
    {
        base.Update();
        _score.text = _gameState.score.ToString(_numberFormat);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreSettings
{
    public int scorePerSecond = 100;
    public int scorePerCombo = 1000;
    public int scoreToChangeLevel = 1000;
    public float comboDuration = 8f;
}

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CatCategorySettings
{
    [TitleGroup("Aspect")]
    public Color color1 = Color.green;
    public Color color2 = Color.black;

    [TitleGroup("Speed modifier")]
    [Range(0, 2)]
    public float eatSpeedModifier = 1f;
    [Range(0, 2)]
    public float hungrySpeedModifier = 1f;
}

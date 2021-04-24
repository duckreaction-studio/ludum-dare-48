using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CatCommonSettings
{
    [TitleGroup("Speed")]
    [Range(0, 1)]
    public float eatSpeed = 0.3f;
    [Range(-1, 0)]
    public float hungrySpeed = -0.05f;

    [TitleGroup("Hungry steps")]
    [Range(0, 1)]
    public float notHungry = 0.66f;
    [Range(0, 1)]
    public float veryHungry = 0.33f;

    [TitleGroup("Delay")]
    public float happyDelay = 8f;
    public float afterEatDelay = 4f;
    public float dizzyDelay = 6f;

    [TitleGroup("Speed modifiers")]
    public float fast = 2f;
    public float normal = 1f;
    public float slow = 0.5f;
    public float verySlow = 0.1f;

    [TitleGroup("Other")]
    public float distanceFromBowl = 1f;
    public float initHungry = 0.6f;
}

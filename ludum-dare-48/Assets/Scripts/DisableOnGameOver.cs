using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameOver : GameBehaviour
{
    protected override void OnGameOver()
    {
        gameObject.SetActive(false);
    }

    protected override void OnGameReset()
    {
        gameObject.SetActive(true);
    }
}

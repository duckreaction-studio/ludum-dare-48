using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class CatUI : MonoBehaviour
    {
        [SerializeField]
        Image _image;

        CatAI _catAi;
        public CatAI catAi
        {
            get
            {
                if (_catAi == null)
                {
                    _catAi = GetComponentInParent<CatAI>();
                }
                return _catAi;
            }
        }
        void Update()
        {
            if (catAi)
            {
                _image.fillAmount = catAi.hungry;
                _image.color = GetColor();
            }
        }

        private Color GetColor()
        {
            if (_catAi.isVeryHungry())
            {
                return Color.red;
            }
            else if (_catAi.isNotHungry())
            {
                return Color.green;
            }
            else
            {
                return Color.yellow;
            }
        }
    }
}

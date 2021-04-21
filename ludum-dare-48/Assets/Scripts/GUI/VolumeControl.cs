using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GUI
{
    public class VolumeControl : MonoBehaviour
    {
        Slider _slider;
        Slider slider
        {
            get
            {
                if (_slider == null)
                {
                    _slider = GetComponent<Slider>();
                }
                return _slider;
            }
        }

        bool _isUpdatingInput = false;

        void Awake()
        {
            UpdateSlider();
        }

        void OnEnable()
        {
            UpdateSlider();
        }

        private void UpdateSlider()
        {
            _isUpdatingInput = true;
            slider.value = AudioListener.volume;
            _isUpdatingInput = false;
        }

        public void OnSliderValueChanged(float newValue)
        {
            if (!_isUpdatingInput)
                AudioListener.volume = newValue;
        }
    }
}
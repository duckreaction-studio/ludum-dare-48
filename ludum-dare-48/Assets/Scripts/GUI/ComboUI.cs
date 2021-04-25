using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuckReaction.Common;
using DG.Tweening;
using Core;
using System;
using TMPro;

namespace GUI
{
    public class ComboUI : GameBehaviour
    {
        [SerializeField]
        float _xOffet = 1000;
        [SerializeField]
        float _appearDuration = 0.3f;
        [SerializeField]
        float _shakeDuration = 0.6f;
        [SerializeField]
        float _shakeStrength = 100f;
        [SerializeField]
        int _shakeVibrato = 10;
        [SerializeField]
        float _delayBeforeDisappear = 1f;
        [SerializeField]
        string[] _comboMessages = new string[]
        {
            "Combo !",
            "Double Combo",
            "CRAZY combo !!!",
            "IIINSAAANE !!!",
            "COMBO KILLER !",
            "MAAASTER COMBO KIIING !!!",
            "Ok... We know you're cheating"
        };
        [SerializeField]
        TMP_Text _text;

        Vector2 _startAnchor;

        RectTransform _rect;

        void Awake()
        {
            _rect = this.GetRectTransform();
            _startAnchor = _rect.anchoredPosition;

            GoToStartPosition();
        }

        void Start()
        {
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
        }

        private void OnGameEventReceived(GameEvent ge)
        {
            if (ge.Is(CoreGameEventType.StartCombo))
            {
                UpdateComboMessage(ge.GetParam<int>());
                StartComboAnimation();
            }
        }

        private void UpdateComboMessage(int combo)
        {
            _text.text = _comboMessages[Mathf.Clamp(combo - 1, 0, _comboMessages.Length - 1)];
        }

        [ContextMenu("Start combo animation")]
        public void StartComboAnimation()
        {
            GoToStartPosition();

            var sequence = DOTween.Sequence();
            sequence.Append(_rect.DOAnchorPosX(_startAnchor.x, _appearDuration).SetEase(Ease.OutCubic));
            sequence.Append(_rect.DOShakeAnchorPos(_shakeDuration, _shakeStrength, _shakeVibrato));
            sequence.AppendInterval(_delayBeforeDisappear);
            sequence.Append(_rect.DOAnchorPosX(_startAnchor.x + _xOffet, _appearDuration).SetEase(Ease.OutCubic));
            sequence.Play();
        }

        private void GoToStartPosition()
        {
            var pos = _startAnchor;
            pos.x -= _xOffet;
            _rect.anchoredPosition = pos;
        }
    }
}
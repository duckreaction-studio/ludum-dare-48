using DG.Tweening;
using DuckReaction.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GUI
{
    public class AnimatedPanel : MonoBehaviour
    {
        [SerializeField]
        float _animationDuration = 0.5f;
        [SerializeField]
        bool _hideOnAwake = true;
        [ShowInInspector, ReadOnly]
        float _startY;
        [ShowInInspector, ReadOnly]
        float _endY;

        bool _isVisible = true;

        void Awake()
        {
            _startY = Screen.height;
            _endY = this.GetRectTransform().anchoredPosition.y;
            if (_hideOnAwake)
                Hide(true);
        }

        private void Update()
        {
            if (!_isVisible && _startY != Screen.height)
            {
                _startY = Screen.height;
                SetAnchorY(_startY);
            }
        }

        [ContextMenu("Show")]
        public void Show()
        {
            Show(false);
        }

        public void Show(bool instant)
        {
            _isVisible = true;
            Transition(_endY, instant);
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            Hide(false);
        }

        public void Hide(bool instant)
        {
            _isVisible = false;
            Transition(_startY, instant);
        }

        private void Transition(float targetY, bool instant = false)
        {
            if (instant)
            {
                SetAnchorY(targetY);
            }
            else
            {
                this.GetRectTransform().DOAnchorPosY(targetY, _animationDuration).SetEase(Ease.OutCubic);
            }
        }

        private void SetAnchorY(float targetY)
        {
            var pos = this.GetRectTransform().anchoredPosition;
            pos.y = targetY;
            this.GetRectTransform().anchoredPosition = pos;
        }
    }
}
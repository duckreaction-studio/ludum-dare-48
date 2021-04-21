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

        void Awake()
        {
            _startY = Screen.height;
            _endY = this.GetRectTransform().anchoredPosition.y;
            if (_hideOnAwake)
                Hide(true);
        }

        [ContextMenu("Show")]
        public void Show()
        {
            Show(false);
        }

        public void Show(bool instant)
        {
            Transition(_endY, instant);
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            Hide(false);
        }

        public void Hide(bool instant)
        {
            Transition(_startY, instant);
        }

        private void Transition(float targetY, bool instant = false)
        {
            if (instant)
            {
                var pos = this.GetRectTransform().anchoredPosition;
                pos.y = targetY;
                this.GetRectTransform().anchoredPosition = pos;
            }
            else
            {
                this.GetRectTransform().DOAnchorPosY(targetY, _animationDuration).SetEase(Ease.OutCubic);
            }
        }
    }
}
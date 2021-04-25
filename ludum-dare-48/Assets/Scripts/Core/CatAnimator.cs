using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class RandomRange
    {
        [MinMaxSlider(0.1f, 20f, true)]
        public Vector2 minMax;

        public RandomRange()
        {

        }
        public RandomRange(float min, float max)
        {
            minMax = new Vector2(min, max);
        }

        public float timeout { get; private set; }

        public float Next()
        {
            timeout = Time.realtimeSinceStartup + UnityEngine.Random.Range(minMax.x, minMax.y);
            return timeout;
        }

        public bool IsOverdue()
        {
            return Time.realtimeSinceStartup > timeout;
        }
    }
    public class CatAnimator : GameBehaviour
    {
        [SerializeField]
        RandomRange _blinkDelay = new RandomRange(0.1f, 20f);
        [SerializeField]
        RandomRange _yawnDelay = new RandomRange(4f, 20f);
        [SerializeField]
        RandomRange _meowDelay = new RandomRange(1f, 20f);

        Animator _animator;
        public Animator animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = GetComponentInChildren<Animator>();
                }
                return _animator;
            }
        }

        protected override void Start()
        {
            base.Start();

            _blinkDelay.Next();
        }

        protected override void Update()
        {
            base.Update();
            UpdateRandomRange(_blinkDelay, "Blink");
            UpdateRandomRange(_yawnDelay, "Yawn");
            UpdateRandomRange(_meowDelay, "Meow");
        }

        [ContextMenu("Test smash")]
        public void OnCatSmashed()
        {
            animator.ResetTrigger("Fed");
            animator.ResetTrigger("FullFed");
            animator.SetTrigger("Smash");
            animator.SetTrigger("NoBowl");
        }

        [ContextMenu("OnCatEating")]
        public void OnCatEating()
        {
            animator.ResetTrigger("NoBowl");
            animator.ResetTrigger("Angry");
            animator.SetTrigger("Fed");
            animator.SetTrigger("Smash");
        }

        [ContextMenu("OnCatHappy")]
        public void OnCatHappy()
        {
            animator.SetTrigger("Fed");
            animator.SetTrigger("FullFed");
            animator.SetTrigger("NoBowl");
        }

        [ContextMenu("OnCatHungry")]
        public void OnCatHungry()
        {
            animator.ResetTrigger("FullFed");
        }

        [ContextMenu("OnCatAfterEat")]
        public void OnCatAfterEat()
        {
            animator.SetTrigger("NoBowl");
        }

        [ContextMenu("OnCatIdleAfterDizzy")]
        public void OnCatIdleAfterDizzy()
        {
            animator.SetTrigger("Angry");
        }

        [ContextMenu("OnCatIdle")]
        public void OnCatIdle()
        {
            animator.ResetTrigger("Angry");
            animator.SetTrigger("NoBowl");
        }

        private void UpdateRandomRange(RandomRange range, string trigger)
        {
            if (range.IsOverdue())
            {
                range.Next();
                animator.SetTrigger(trigger);
            }
            else
            {
                //animator.ResetTrigger(trigger);
            }
        }
    }
}
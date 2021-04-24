using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class CatAI : GameBehaviour, IPointerClickHandler
    {
        public enum State
        {
            Idle,
            Eating,
            Playing,
            Dead
        }

        const float notHungry = 0.66f;
        const float veryHungry = 0.33f;
        const float fast = 2f;
        const float normal = 1f;
        const float slow = 2f;
        const float verySlow = 0.1f;

        [SerializeField]
        float _distanceFromBowl = 0.5f;
        [SerializeField]
        float _defaultHungry = 0.6f;

        [Inject]
        CatBowl _bowl;

        State _state = State.Idle;

        public float hungry { get; private set; }

        public void OnAfterSpawn()
        {
            hungry = _defaultHungry;
        }

        protected override void RunningUpdate()
        {
            UpdateStressAndHungry();

            if (_state == State.Eating)
            {
                if (!bowlIsCloseEnough())
                    StopEating();
            }
        }

        private void UpdateStressAndHungry()
        {
            hungry += GetHungrySpeedModifier() * GetHungrySpeed() * Time.deltaTime;

            hungry = Mathf.Clamp01(hungry);

            if (hungry == 0)
            {
                _state = State.Dead;
                _signalBus.Fire(new GameEvent(CoreGameEventType.CatIsDead, this));
            }
        }

        private float GetHungrySpeedModifier()
        {
            if (_state == State.Eating)
            {
                if (isNotHungry())
                    return slow;
                else if (isVeryHungry())
                    return fast;
                else
                    return normal;
            }
            else if (_state == State.Playing)
            {
                return verySlow;
            }
            else
            {
                if (isNotHungry())
                    return fast;
                else if (isVeryHungry())
                    return slow;
                else
                    return normal;
            }
        }

        public bool isVeryHungry()
        {
            return hungry < veryHungry;
        }

        public bool isNotHungry()
        {
            return hungry > notHungry;
        }

        private float GetHungrySpeed()
        {
            if (_state == State.Eating)
            {
                return 0.5f;
            }
            else
            {
                return -0.25f;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_gameState.isStartedOrRunning())
            {
                if (bowlIsCloseEnough())
                {
                    StartEating();
                }
            }
        }

        private bool bowlIsCloseEnough()
        {
            return Mathf.Abs(_bowl.transform.position.x - transform.position.x) < _distanceFromBowl;
        }

        private void StartEating()
        {
            Debug.Log("Start eating", this);
            _state = State.Eating;
            _signalBus.Fire(new GameEvent(CoreGameEventType.CatStartEating, this));
        }

        private void StopEating()
        {
            Debug.Log("Stop eating", this);
            _state = State.Idle;
            _signalBus.Fire(new GameEvent(CoreGameEventType.CatStopEating, this));
        }
    }
}
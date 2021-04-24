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
            AfterEat,
            Happy,
            Dizzy,
            Playing,
            Dead
        }

        [Inject]
        ProjectSettings _projectSettings;

        [Inject]
        CatBowl _bowl;

        State _state = State.Idle;
        float _stateChangeTime;

        public float hungry { get; private set; }

        private CatCommonSettings common
        {
            get => _projectSettings.catCommonSettings;
        }


        private Cat _cat;
        private Cat cat
        {
            get
            {
                if (_cat == null)
                {
                    _cat = GetComponent<Cat>();
                }
                return _cat;
            }
        }
        private CatCategorySettings category
        {
            get => cat.category;
        }

        public void OnAfterSpawn()
        {
            hungry = common.initHungry;
        }

        protected override void RunningUpdate()
        {
            UpdateHungry();

            if (_state == State.Eating)
            {
                if (!bowlIsCloseEnough())
                    SetState(State.AfterEat);
            }

            if (_state == State.Happy || _state == State.AfterEat || _state == State.Dizzy)
            {
                if (IsTimeout())
                {
                    UpdateStateAfterTimeout();
                }
            }
        }

        private void UpdateStateAfterTimeout()
        {
            if (_state == State.Dizzy)
            {
                hungry -= common.hit;
                UpdateHungry();
            }
            if (_state != State.Dead)
                SetState(State.Idle);
        }

        private bool IsTimeout()
        {
            return Time.realtimeSinceStartup > _stateChangeTime + GetStateDuration();
        }

        private float GetStateDuration()
        {
            switch (_state)
            {
                case State.Happy:
                    return common.happyDelay;
                case State.AfterEat:
                    return common.afterEatDelay;
                case State.Dizzy:
                    return common.dizzyDelay;
            }
            return 0f;
        }

        private void UpdateHungry()
        {
            hungry += GetHungrySpeedModifier() * GetHungrySpeed() * Time.deltaTime;

            hungry = Mathf.Clamp01(hungry);

            if (hungry == 0)
            {
                SetState(State.Dead);
            }
        }

        private float GetHungrySpeedModifier()
        {
            if (_state == State.Eating)
            {
                if (isNotHungry())
                    return common.slow;
                else if (isVeryHungry())
                    return common.fast;
                else
                    return common.normal;
            }
            else if (_state == State.Playing)
            {
                return common.verySlow;
            }
            else if (_state == State.Idle)
            {
                if (isNotHungry())
                    return common.fast;
                else if (isVeryHungry())
                    return common.slow;
                else
                    return common.normal;
            }
            return 0f;
        }

        public bool isVeryHungry()
        {
            return hungry < common.veryHungry;
        }

        public bool isNotHungry()
        {
            return hungry > common.notHungry;
        }

        private float GetHungrySpeed()
        {
            if (_state == State.Eating)
            {
                return common.eatSpeed * category.eatSpeedModifier;
            }
            else
            {
                return common.hungrySpeed * category.hungrySpeedModifier;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_gameState.isStartedOrRunning())
            {
                if (bowlIsCloseEnough())
                {
                    SetState(State.Eating);
                }
                else
                {
                    SetState(State.Dizzy);
                }
            }
        }

        private bool bowlIsCloseEnough()
        {
            return Mathf.Abs(_bowl.transform.position.x - transform.position.x) < common.distanceFromBowl;
        }

        private void SetState(State newState)
        {
            if (newState != _state)
            {
                var previousState = _state;
                _state = newState;
                _stateChangeTime = Time.realtimeSinceStartup;
                FireStateChanged(previousState, newState);
            }
        }

        private void FireStateChanged(State previousState, State newState)
        {
            if (previousState == State.Eating)
                _signalBus.Fire(new GameEvent(CoreGameEventType.CatStopEating, this));
            if (newState == State.Eating)
                _signalBus.Fire(new GameEvent(CoreGameEventType.CatStartEating, this));
            if (newState == State.Dead)
                _signalBus.Fire(new GameEvent(CoreGameEventType.CatIsDead, this));
        }
    }
}
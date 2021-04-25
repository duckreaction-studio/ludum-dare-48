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

        public void MouseIsDetected()
        {
            if (_state == State.Idle || _state == State.Playing)
            {
                SetState(State.Playing, true);
            }
        }

        protected override void RunningUpdate()
        {
            UpdateHungry();

            if (_state == State.Eating)
            {
                if (!bowlIsCloseEnough())
                    SetState(State.AfterEat);
            }

            if (_state == State.Happy || _state == State.AfterEat || _state == State.Dizzy || _state == State.Playing)
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

        protected override void OnGameResume(float pauseDuration)
        {
            _stateChangeTime += pauseDuration;
        }

        public void RestartHappy()
        {
            if (IsHappy())
                SetState(State.Happy, true);
        }

        public void StopHappy()
        {
            hungry -= 0.01f;
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
                case State.Playing:
                    return common.playingDelay;
            }
            return 0f;
        }

        private void UpdateHungry()
        {
            var previous = hungry;
            hungry += GetHungrySpeedModifier() * GetHungrySpeed() * Time.deltaTime;

            hungry = Mathf.Clamp01(hungry);

            if (hungry == 0)
            {
                SetState(State.Dead);
            }
            if (hungry == 1)
            {
                SetState(State.Happy);
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

        public bool IsHappy()
        {
            return _state == State.Happy;
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
                    if (_state == State.Dizzy)
                    {
                        SpamSmash();
                    }
                    else
                    {
                        SetState(State.Dizzy);
                    }
                }
            }
        }

        private void SpamSmash()
        {
            BroadcastMessage("OnSpamSmash", SendMessageOptions.DontRequireReceiver);
            _signalBus.Fire(new GameEvent(CoreGameEventType.SpamSmash, this));
        }

        private bool bowlIsCloseEnough()
        {
            return Mathf.Abs(_bowl.transform.position.x - transform.position.x) < common.distanceFromBowl;
        }

        private void SetState(State newState, bool forceRestartTimer = false)
        {
            if (newState != _state)
            {
                var previousState = _state;
                _state = newState;
                _stateChangeTime = Time.realtimeSinceStartup;
                FireStateChanged(previousState, newState);
                FireMessage(previousState, newState);
            }
            else if (forceRestartTimer)
            {
                _stateChangeTime = Time.realtimeSinceStartup;
            }
        }

        private void FireMessage(State previousState, State newState)
        {
            if (newState == State.Eating)
                BroadcastMessage("OnCatEating", SendMessageOptions.DontRequireReceiver);
            else if (newState == State.Dizzy)
                BroadcastMessage("OnCatSmashed", SendMessageOptions.DontRequireReceiver);
            else if (newState == State.Happy)
                BroadcastMessage("OnCatHappy", SendMessageOptions.DontRequireReceiver);
            else if (newState == State.AfterEat)
                BroadcastMessage("OnCatAfterEat", SendMessageOptions.DontRequireReceiver);
            else if (previousState == State.Dizzy && newState == State.Idle)
                BroadcastMessage("OnCatIdleAfterDizzy", SendMessageOptions.DontRequireReceiver);
            else if (newState == State.Idle)
                BroadcastMessage("OnCatIdle", SendMessageOptions.DontRequireReceiver);
        }

        public static readonly Dictionary<State, CoreGameEventType> mapStateEvent =
        new Dictionary<State, CoreGameEventType>()
        {
            { State.Eating, CoreGameEventType.CatStartEating },
            { State.Dizzy, CoreGameEventType.CatStartDizzy },
            { State.Playing, CoreGameEventType.CatIsPlaying },
            { State.Happy, CoreGameEventType.CatIsHappy },
            { State.Dead, CoreGameEventType.CatIsDead }
        };

        private void FireStateChanged(State previousState, State newState)
        {
            if (previousState == State.Eating)
                _signalBus.Fire(new GameEvent(CoreGameEventType.CatStopEating, this));

            if (previousState == State.Happy && newState == State.Dizzy)
                _signalBus.Fire(new GameEvent(CoreGameEventType.HitHappyCat, this));

            CoreGameEventType eventType;
            if (mapStateEvent.TryGetValue(newState, out eventType))
            {
                _signalBus.Fire(new GameEvent(eventType, this));
            }
        }
    }
}
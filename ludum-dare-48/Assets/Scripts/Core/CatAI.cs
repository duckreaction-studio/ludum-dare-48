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

        [Inject]
        ProjectSettings _projectSettings;

        [Inject]
        CatBowl _bowl;

        State _state = State.Idle;

        public float hungry { get; private set; }

        private CatCommonSettings common
        {
            get => _projectSettings.catCommonSettings;
        }

        public void OnAfterSpawn()
        {
            hungry = common.initHungry;
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
            else
            {
                if (isNotHungry())
                    return common.fast;
                else if (isVeryHungry())
                    return common.slow;
                else
                    return common.normal;
            }
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
                return common.eatSpeed;
            }
            else
            {
                return common.hungrySpeed;
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
            return Mathf.Abs(_bowl.transform.position.x - transform.position.x) < common.distanceFromBowl;
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
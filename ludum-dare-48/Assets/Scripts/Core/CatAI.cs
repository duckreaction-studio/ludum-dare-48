using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public enum CatState
    {
        Idle,
        Eating
    }
    public class CatAI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        float _distanceFromBowl = 0.5f;

        [Inject]
        CatBowl _bowl;

        [Inject]
        SignalBus _signalBus;

        CatState _state = CatState.Idle;

        public void Update()
        {
            if (_state == CatState.Eating)
            {
                if (!bowlIsCloseEnough())
                    StopEating();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (bowlIsCloseEnough())
            {
                StartEating();
            }
        }

        private bool bowlIsCloseEnough()
        {
            return Mathf.Abs(_bowl.transform.position.x - transform.position.x) < _distanceFromBowl;
        }

        private void StartEating()
        {
            Debug.Log("Start eating", this);
            _state = CatState.Eating;
            _signalBus.Fire(new GameEvent(CoreGameEventType.CatStartEating, this));
        }

        private void StopEating()
        {
            Debug.Log("Stop eating", this);
            _state = CatState.Idle;
            _signalBus.Fire(new GameEvent(CoreGameEventType.CatStopEating, this));
        }
    }
}
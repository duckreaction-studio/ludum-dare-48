using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
    public enum CoreGameEventType
    {
        CatStartEating = GameEventType.Other + 1,
        CatStopEating,
        CatIsDead
    }

    public class GameState : MonoBehaviour
    {
        public enum State
        {
            Init,
            Started,
            Running,
            Paused,
            GameOver
        }

        [SerializeField]
        int _scorePerSecond = 100;

        [SerializeField]
        int _scoreToChangeLevel = 1000;

        float _score = 0;
        public int score { get => (int)_score; }

        public int level { get; private set; } = 1;

        public State state { get; private set; } = State.Init;

        [Inject]
        SignalBus _signalBus;

        public void Start()
        {
            _score = 0;
            level = 1;
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
            SetState(State.Started);
        }

        private void OnGameEventReceived(GameEvent gameEvent)
        {
            if (state == State.Started && gameEvent.Is(CoreGameEventType.CatStartEating))
            {
                SetState(State.Running);
            }
        }

        public bool isRunning()
        {
            return state == State.Running;
        }

        public bool isStartedOrRunning()
        {
            return state == State.Started || state == State.Running;
        }

        public void Update()
        {
            if (isRunning())
            {
                _score += Time.deltaTime * _scorePerSecond;
                int newLevel = (int)(_score / _scoreToChangeLevel) + 1;
                if (newLevel > level)
                {
                    level = newLevel;
                    _signalBus.Fire(new GameEvent(GameEventType.LevelUp, newLevel));
                    Debug.Log("Level UP " + newLevel);
                }
            }
        }

        public void SetState(State newState)
        {
            if (newState != state)
            {
                state = newState;
                _signalBus.Fire(new GameEvent(GameEventType.GameStateChanged, newState));
            }
        }
    }
}
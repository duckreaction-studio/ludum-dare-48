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
        None = GameEventType.Other + 1,
        CatIsIdle,
        CatStartEating,
        CatStopEating,
        CatIsPlaying,
        CatIsHappy,
        HitHappyCat,
        CatIsDead,
        StartCombo,
        StopCombo
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

        float _score = 0;
        public int score { get => (int)_score; }

        public int totalScore { get => score + bonusScore; }

        public int bonusScore { get; set; } = 0;

        public int level { get; private set; } = 1;

        public State state { get; private set; } = State.Init;

        private State _previousState;
        private float _stateChangeTime;

        [Inject]
        SignalBus _signalBus;

        [Inject]
        ProjectSettings _projectSettings;

        public ScoreSettings scoreSettings { get => _projectSettings.scoreSettings; }

        public void Start()
        {
            _score = 0;
            bonusScore = 0;
            level = 1;
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
            SetState(State.Started);
        }

        public void Restart()
        {
            _score = 0;
            bonusScore = 0;
            level = 1;
            _signalBus.Fire(new GameEvent(GameEventType.GameReset));
            SetState(State.Started);
        }

        private void OnGameEventReceived(GameEvent gameEvent)
        {
            if (state == State.Started && gameEvent.Is(CoreGameEventType.CatStartEating))
            {
                SetState(State.Running);
                _signalBus.Fire(new GameEvent(GameEventType.GameStart));
            }
        }

        public void Pause()
        {
            SetState(State.Paused);
        }

        public void Resume()
        {
            if (state == State.Paused)
                SetState(_previousState);
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
                _score += Time.deltaTime * scoreSettings.scorePerSecond;
                int newLevel = totalScore / scoreSettings.scoreToChangeLevel + 1;
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
                _previousState = state;
                var stateDuration = Time.realtimeSinceStartup - _stateChangeTime;
                state = newState;
                _stateChangeTime = Time.realtimeSinceStartup;
                _signalBus.Fire(new GameEvent(GameEventType.GameStateChanged, newState));
                FirePauseEvent(stateDuration);
            }
        }

        private void FirePauseEvent(float stateDuration)
        {
            if (_previousState != State.Paused && state == State.Paused)
                _signalBus.Fire(new GameEvent(GameEventType.GamePause));
            if (_previousState == State.Paused && state != State.Paused)
                _signalBus.Fire(new GameEvent(GameEventType.GameResume, stateDuration));
        }
    }
}
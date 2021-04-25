using DuckReaction.Common;
using System;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameBehaviour : MonoBehaviour
    {
        [Inject]
        protected SignalBus _signalBus;

        [Inject]
        protected GameState _gameState;

        protected virtual void Start()
        {
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
        }

        protected virtual void OnGameEventReceived(GameEvent ge)
        {
            if (ge.type == GameEventType.GamePause)
                OnGamePause();
            else if (ge.type == GameEventType.GameResume)
                OnGameResume(ge.GetParam<float>());
            else if (ge.type == GameEventType.GameOver)
                OnGameOver();
            else if (ge.type == GameEventType.GameReset)
                OnGameReset();
        }

        protected virtual void OnGamePause()
        {

        }
        protected virtual void OnGameResume(float pauseDuration)
        {

        }

        protected virtual void OnGameOver()
        {
        }

        protected virtual void OnGameReset()
        {
        }

        protected virtual void Update()
        {
            if (_gameState.isRunning())
            {
                RunningUpdate();
            }
        }

        protected virtual void RunningUpdate()
        {

        }
    }
}
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
using DuckReaction.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        int _scorePerSecond = 100;

        [SerializeField]
        int _scoreToChangeLevel = 1000;

        float _score = 0;
        public int score { get => (int)_score; }

        public int level { get; private set; } = 1;

        [Inject]
        SignalBus _signalBus;

        public void Start()
        {
            _score = 0;
            level = 1;
        }

        public void Update()
        {
            _score += Time.deltaTime * _scorePerSecond;
            int newLevel = (int)(_score / _scoreToChangeLevel) + 1;
            if (newLevel > level)
            {
                level = newLevel;
                _signalBus.Fire(new GameEvent(GameEventType.LEVEL_UP, newLevel));
                Debug.Log("Level UP " + newLevel);
            }
        }
    }
}
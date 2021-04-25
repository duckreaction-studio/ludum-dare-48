using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DuckReaction.Common;

namespace DuckReaction.Audio
{
    public class AudioService : MonoBehaviour
    {
        [Inject]
        SignalBus _signalBus;

        AudioSourceService[] _sourceServiceList;

        protected void Start()
        {
            _sourceServiceList = GetComponentsInChildren<AudioSourceService>(true);
            _signalBus?.Subscribe<GameEvent>(OnGameEventReceived);
            ProcessEvent("start"); // Useful for background music
        }

        protected void OnDestroy()
        {
            _signalBus?.Unsubscribe<GameEvent>(OnGameEventReceived);
        }

        private void OnGameEventReceived(GameEvent e)
        {
            ProcessEvent(e.typeAsString);
        }

        private void ProcessEvent(string eventName)
        {
            foreach (var service in _sourceServiceList)
            {
                service.ProcessGameEvent(eventName);
            }
        }
    }
}
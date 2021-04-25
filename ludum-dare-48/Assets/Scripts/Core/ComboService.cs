using DuckReaction.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core
{
    public class ComboService : GameBehaviour
    {
        [Inject]
        ProjectSettings _projectSettings;

        [Inject]
        CatSpawner _catSpawner;

        int _previousHappyCatCount = 0;
        public int maxCombo { get; private set; } = 0;

        float _comboStart = 0;

        private ScoreSettings scoreSettings
        {
            get => _projectSettings.scoreSettings;
        }

        protected override void RunningUpdate()
        {
            if (ComboIsRunning() && Time.realtimeSinceStartup > (_comboStart + scoreSettings.comboDuration))
            {
                StopCombo();
            }
        }

        private bool ComboIsRunning()
        {
            return _previousHappyCatCount > 0;
        }

        protected override void OnGameEventReceived(GameEvent ge)
        {
            base.OnGameEventReceived(ge);

            if (ge.Is(CoreGameEventType.CatIsHappy))
            {
                var happyCatCount = GetHappyCats().Count();
                if (happyCatCount > 1 && happyCatCount > _previousHappyCatCount)
                {
                    StartCombo(happyCatCount);
                }
            }
            else if (ge.Is(CoreGameEventType.HitHappyCat))
            {
                Debug.Log("Happy Cat hit, Cancel combo");
                foreach (var cat in GetHappyCats())
                {
                    cat.ai.StopHappy();
                }
                StopCombo();
            }
        }

        protected override void OnGameResume(float pauseDuration)
        {
            _comboStart += pauseDuration;
        }

        private IEnumerable<Cat> GetHappyCats()
        {
            return _catSpawner.activeCatList.Where(cat => cat.ai.IsHappy());
        }

        private void StartCombo(int newHappyCatCount)
        {
            _previousHappyCatCount = newHappyCatCount;
            _comboStart = Time.realtimeSinceStartup;
            var multiply = newHappyCatCount - 1;
            _gameState.bonusScore += (multiply * scoreSettings.scorePerCombo);
            maxCombo = Mathf.Max(maxCombo, multiply);

            foreach (var cat in GetHappyCats())
            {
                cat.ai.RestartHappy();
            }

            _signalBus.Fire(new GameEvent(CoreGameEventType.StartCombo, multiply));
        }

        private void StopCombo()
        {
            _previousHappyCatCount = 0;
            _signalBus.Fire(new GameEvent(CoreGameEventType.StopCombo));
        }
    }
}
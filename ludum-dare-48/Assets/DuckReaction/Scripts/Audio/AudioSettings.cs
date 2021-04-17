using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;

namespace DuckReaction.Audio
{
    [Serializable]
    public class AudioClipSettings
    {
        [SerializeField]
        private AudioClip _clip;
        public AudioClip clip { get { return _clip; } }

        [SerializeField]
        private float _volumeFactor = 1f;
        public float volumeFactor { get { return _volumeFactor; } }

#if UNITY_EDITOR
        [Button("Play")]
        public void Test()
        {
            if (Application.isPlaying)
            {
                var goList = GameObject.FindGameObjectsWithTag("AudioService");
                foreach (var go in goList)
                {
                    go.BroadcastMessage("TestPlayAudioClipSettings", this);
                }
            }
            else
            {
                Debug.Log("You can only test sound in play mode");
            }
        }
#endif
    }

    [Serializable]
    public class AudioSettings
    {
        [SerializeField]
        private string[] _eventNameList;

        [SerializeField]
        [BoxGroup("Volume")]
        private bool _randomVolume = false;

        [SerializeField]
        [BoxGroup("Volume")]
        [ShowIf("_randomVolume")]
        [MinMaxSlider(0.01f,3f,true)]
        private Vector2 _volumeRange = new Vector2(0.5f,1.0f);

        [SerializeField]
        [BoxGroup("Volume")]
        [HideIf("_randomVolume")]
        [Range(0.01f,3f)]
        private float _volume = 1f;

        [SerializeField]
        [BoxGroup("Pitch")]
        private bool _randomPitch = false;

        [SerializeField]
        [BoxGroup("Pitch")]
        [ShowIf("_randomPitch")]
        [MinMaxSlider(-3f, 3f, true)]
        private Vector2 _pitchRange = new Vector2(-1f, 1.0f);

        [SerializeField]
        [BoxGroup("Pitch")]
        [HideIf("_randomPitch")]
        [Range(0.01f, 3f)]
        private float _pitch = 1f;

        [SerializeField]
        [BoxGroup("Timing")]
        private float _delay = 0f;
        public float delay { get { return _delay; } }

        [SerializeField]
        [TableList(AlwaysExpanded =true)]
        private AudioClipSettings[] _audioClipSettings;

        public bool HasEvent(string eventName)
        {
            return _eventNameList.Contains(eventName);
        }

        public bool hasDelay()
        {
            return _delay > 0.01f;
        }

        public AudioClipSettings GetAudioClipSettings()
        {
            return _audioClipSettings[UnityEngine.Random.Range(0, _audioClipSettings.Length)];
        }

        public float GetVolume(AudioClipSettings clipSettings)
        {
            float volume = clipSettings.volumeFactor;
            if (_randomVolume)
                volume *= UnityEngine.Random.Range(_volumeRange.x, _volumeRange.y);
            else
                volume *= _volume;
            return volume;
        }

        public float GetPitch(AudioClipSettings clipSettings)
        {
            if (_randomPitch)
                return UnityEngine.Random.Range(_pitchRange.x, _pitchRange.y);
            return _pitch;
        }

        public bool HasClipSettings(AudioClipSettings clipSettings)
        {
            return _audioClipSettings.Contains(clipSettings);
        }
    }
}
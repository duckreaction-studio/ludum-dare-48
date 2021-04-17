using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuckReaction.Audio {

    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceService : MonoBehaviour
    {
        [SerializeField]
        protected bool _abortOnNewEvent = true;

        [SerializeField]
        protected AudioSettings _settings;

        protected AudioSource _audioSource;
        public AudioSource audioSource {
            get
            {
                if(_audioSource == null)
                {
                    _audioSource = GetComponent<AudioSource>();
                }
                return _audioSource;
            }
        }

        public virtual bool ProcessGameEvent(string eventName)
        {
            if(_settings.HasEvent(eventName))
            {
                Play();
                return true;
            }
            return false;
        }

        protected virtual void Play()
        {
            if(_abortOnNewEvent || !audioSource.isPlaying)
            {
                var clipSettings = _settings.GetAudioClipSettings();
                PlayClipSettings(clipSettings);
            }
        }

        private void PlayClipSettings(AudioClipSettings clipSettings)
        {
            audioSource.volume = _settings.GetVolume(clipSettings);
            audioSource.pitch = _settings.GetPitch(clipSettings);
            audioSource.clip = clipSettings.clip;
            if (_settings.hasDelay())
                audioSource.PlayDelayed(_settings.delay);
            else
                audioSource.Play();
        }

#if UNITY_EDITOR
        public void TestPlayAudioClipSettings(AudioClipSettings clipSettings)
        {
            if(_settings.HasClipSettings(clipSettings))
                PlayClipSettings(clipSettings);
        }
#endif
    }
}

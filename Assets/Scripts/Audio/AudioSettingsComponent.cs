using System;
using Data;
using Infrastructure.Services.PersistentProgress;
using UI.Windows.Settings;
using UnityEngine;

namespace Audio
{
    public class AudioSettingsComponent : MonoBehaviour, ISavedProgress
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioModel _audioModel;

        private bool _firstSet;

        public float AudioVolume { get; private set; }

        public AudioSource Source => _source;

        public event Action OnChangeValue;

        private void Start()
        {
            SetValue(_source.volume);
            
            DontDestroyOnLoad(this);
        }

        public void SetValue(float value)
        {
            AudioVolume = value;
            _source.volume = value;


            OnChangeValue?.Invoke();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            switch (_audioModel)
            {
                case AudioModel.MusicAudioSource:
                    SetValue(progress.AudioData.MusicValue);
                    break;
                case AudioModel.SfxAudioSource:
                    SetValue(progress.AudioData.SfxValue);
                    break;
            }
            
            if (!_firstSet)
            {
                _firstSet = true;
                PlayBackground();
            }
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            switch (_audioModel)
            {
                case AudioModel.MusicAudioSource:
                    progress.AudioData.MusicValue = AudioVolume;
                    break;
                case AudioModel.SfxAudioSource:
                    progress.AudioData.SfxValue = AudioVolume;
                    break;
            }
        }

        private void PlayBackground()
        {
            if (_audioModel == AudioModel.MusicAudioSource)
            {
                _source.Play();
            }
        }
    }
}
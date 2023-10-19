using System.Collections.Generic;
using Audio;
using Infrastructure.Services.PersistentProgress.SaveLoad;
using UnityEngine;

namespace UI.Windows.Settings
{
    public class SettingsWindowController : WindowBase
    {
        [field: SerializeField] public Transform SfxContainer { get; private set; }
        [field: SerializeField] public Transform MusicContainer { get; private set; }
        [field: SerializeField] public Transform SettingsWindowContainer { get; private set; }

        public List<AudioSettingWidget> SettingWidgets = new List<AudioSettingWidget>();

        private AudioSettingsComponent _sfxSource;
        private AudioSettingsComponent _musicSource;

        private AudioSettingWidget _sfxSettings;
        private AudioSettingWidget _musicSettings;

        private ISaveLoadService _saveLoadService;

        private SettingsWindow _settingsWindow;

        public void Construct(AudioSettingsComponent sfxSource, AudioSettingsComponent musicSource,
            SettingsWindow settingsWindow, ISaveLoadService saveLoadService)
        {
            _sfxSource = sfxSource;
            _musicSource = musicSource;
            _settingsWindow = settingsWindow;
            _saveLoadService = saveLoadService;
        }

        protected override void Initialize()
        {
            InitAudioWidgets();

            _sfxSource.OnChangeValue += SetAudioSettings;
            _musicSource.OnChangeValue += SetAudioSettings;
            _sfxSettings.OnChanged += OnSettingsChanged;
            _musicSettings.OnChanged += OnSettingsChanged;
            
            _settingsWindow.SetButtonCallBack(OnClickButton);

            SetAudioSettings();
        }

        protected override void Cleanup()
        {
            _sfxSource.OnChangeValue -= SetAudioSettings;
            _musicSource.OnChangeValue -= SetAudioSettings;
            _sfxSettings.OnChanged -= OnSettingsChanged;
            _musicSettings.OnChanged -= OnSettingsChanged;
        }

        private void OnClickButton()
        {
            _saveLoadService.SaveProgress();

            DestroySettingsWindow();
        }

        private void DestroySettingsWindow()
        {
            Destroy(_settingsWindow.gameObject);
            Destroy(gameObject);
            Destroy(_sfxSettings.gameObject);
            Destroy(_musicSettings.gameObject);
        }

        private void SetAudioSettings()
        {
            _musicSettings.SetValue(_musicSource.AudioVolume, AudioModel.MusicAudioSource);
            _sfxSettings.SetValue(_sfxSource.AudioVolume, AudioModel.SfxAudioSource);
        }

        private void InitAudioWidgets()
        {
            foreach (AudioSettingWidget settingWidget in SettingWidgets)
            {
                switch (settingWidget.AudioModel)
                {
                    case AudioModel.MusicAudioSource:
                        _musicSettings = settingWidget;
                        break;
                    case AudioModel.SfxAudioSource:
                        _sfxSettings = settingWidget;
                        break;
                }
            }
        }

        private void OnSettingsChanged(float value, AudioModel model)
        {
            switch (model)
            {
                case AudioModel.MusicAudioSource:
                    _musicSource.SetValue(value);
                    break;
                case AudioModel.SfxAudioSource:
                    _sfxSource.SetValue(value);
                    break;
            }
        }
    }
}
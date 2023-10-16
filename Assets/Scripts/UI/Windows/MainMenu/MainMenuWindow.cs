using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.MainMenu
{
    public class MainMenuWindow : WindowBase
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        private Action onStartButton;
        private Action onSettingsAction;
        private Action onExitAction;
        private Action onContinueAction;

        public void SetStartButtonCallBack(Action onStart)
        {
            onStartButton = onStart;

            _startButton.onClick.AddListener(StartButtonCallBack);
        }

        public void SetSettingsButtonCallBack(Action onSettings)
        {
            onSettingsAction = onSettings;

            _settingsButton.onClick.AddListener(SettingsButtonCallBack);
        }

        public void SetContinueButtonCallBack(Action onContinue)
        {
            onContinueAction = onContinue;
            
            _continueButton.gameObject.SetActive(true);
            _continueButton.onClick.AddListener(ContinueButtonCallBack);
        }

        public void SetExitButtonCallBack(Action onExit)
        {
            onExitAction = onExit;

            _exitButton.onClick.AddListener(ExitButtonCallBack);
        }

        private void StartButtonCallBack()
        {
            onStartButton?.Invoke();
        }

        private void SettingsButtonCallBack()
        {
            onSettingsAction?.Invoke();
        }

        private void ContinueButtonCallBack()
        {
            onContinueAction?.Invoke();
        }

        private void ExitButtonCallBack()
        {
            onExitAction?.Invoke();
        }
    }
}
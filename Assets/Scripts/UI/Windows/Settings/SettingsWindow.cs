using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class SettingsWindow : WindowBase
    {
        [SerializeField] private Button _okButton;

        private Action onOkButton;

        public void SetButtonCallBack(Action call)
        {
            onOkButton = call;
            
            _okButton.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            onOkButton?.Invoke();
        }
    }
}
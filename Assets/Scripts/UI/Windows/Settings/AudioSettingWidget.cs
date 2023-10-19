using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Settings
{
    public class AudioSettingWidget : WindowBase
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _value;
        
        [field:SerializeField] public AudioModel AudioModel { get; private set; }
        
        public event Action<float, AudioModel> OnChanged;

        protected override void Initialize()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        
        public void SetValue(float newValue, AudioModel model)
        {
            if (AudioModel == model)
            {
                float textValue = Mathf.Round(newValue * 100);

                _value.text = textValue.ToString();
                _slider.normalizedValue = newValue;
            }
        }
        
        private void OnSliderValueChanged(float value)
        {
            OnChanged?.Invoke(value, AudioModel);
        }
    }
}
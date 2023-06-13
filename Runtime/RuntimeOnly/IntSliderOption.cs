using System;
using UnityEngine;
using UnityEngine.UI;
using static Screenshot.Prefs;

namespace Screenshot {
    public class IntSliderOption : UIOption {
        public Slider slider;
        public Text currentValue;
        public string prefsKey = null;

        public int Value { get { return (int)slider.value; } }

        public static new IntSliderOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<IntSliderOption>(prefabPath, parent);
        }

        private void OnEnable() {}

        public void Initialize(string labelText = null, string prefsKey = null, int defaultValue = 0, Action<int> onChange = null) {
            base.Initialize(labelText);

            slider.wholeNumbers = true;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener((v) => {
                currentValue.text = v.ToString();
                SetInt(prefsKey, (int)v);
                onChange?.Invoke((int)v);
            });

            if (prefsKey != null) {
                this.prefsKey = prefsKey;
            }

            if (this.prefsKey != null) {
                SetValue(GetInt(this.prefsKey, defaultValue));
            }
        }

        public void SetValue(int value) {
            slider.value = value;
        }

        public void SetMinMax(int min, int max) {
            slider.minValue = min;
            slider.maxValue = max;
        }

    }
}
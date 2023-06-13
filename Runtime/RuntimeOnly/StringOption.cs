using System;
using UnityEngine;
using UnityEngine.UI;
using static Screenshot.Prefs;

namespace Screenshot {
    public class StringOption : UIOption {
        public InputField inputField;
        public string prefsKey = null;
        public string Value { get { return inputField.text; } }

        public static new StringOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<StringOption>(prefabPath, parent);
        }

        private void OnEnable() {}

        public void Initialize(string labelText = null, string prefsKey = null, string defaultValue = "", Action<string> onChange = null) {
            base.Initialize(labelText);

            inputField.onValueChanged.RemoveAllListeners();
            inputField.onEndEdit.RemoveAllListeners();

            inputField.onValueChanged.AddListener((v) => {
                SetString(prefsKey, v);
                onChange?.Invoke(v);
            });

            if (prefsKey != null) {
                this.prefsKey = prefsKey;
            }

            if (this.prefsKey != null) {
                SetValue(GetString(this.prefsKey, defaultValue));
            }
        }

        public void SetValue(string value) {
            inputField.text = value;
        }

    }
}
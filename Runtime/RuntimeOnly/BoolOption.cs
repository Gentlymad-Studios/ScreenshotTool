using System;
using UnityEngine;
using UnityEngine.UI;
using static Screenshot.Prefs;

namespace Screenshot {
    public class BoolOption : UIOption {
        public Toggle toggle;
        public string prefsKey = null;

        public bool Value { get { return toggle.isOn; } }

        public static new BoolOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<BoolOption>(prefabPath, parent);
        }

        private void OnEnable() {}

        public void Initialize(string labelText = null, string prefsKey = null, bool defaultValue = false, Action<bool> onChange = null) {
            base.Initialize(labelText);

            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((v) => {
                SetBool(prefsKey, v);
                onChange?.Invoke(v);
            });

            if (prefsKey != null) {
                this.prefsKey = prefsKey;
            }

            if (this.prefsKey != null) {
                SetValue(GetBool(this.prefsKey, defaultValue));
            }
        }

        public void SetValue(bool value) {
            toggle.isOn = value;
        }
    }
}


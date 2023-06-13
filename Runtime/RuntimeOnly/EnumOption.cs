using System;
using UnityEngine;
using UnityEngine.UI;
using static Screenshot.Prefs;
using System.Linq;
using System.Collections.Generic;

namespace Screenshot {
    public class EnumOption<T> : UIOption where T:Enum {
        public Dropdown dropdown;
        public string prefsKey = null;
        private string[] enumNames;
        private int[] enumValues;

        public T Value { get { return (T)(object)enumValues[dropdown.value]; } }

        public static new EnumOption<T> Instantiate(string prefabPath, Transform parent) {
            return Instantiate<EnumOption<T>>(prefabPath, parent);
        }

        private void OnEnable() {}

        public void Initialize(string labelText = null, string prefsKey = null,T defaultValue = default, Action<T> onChange = null) {
            base.Initialize(labelText);

            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.onValueChanged.AddListener((v) => {
                SetInt(prefsKey, enumValues[v]);
                onChange?.Invoke(Value);
            });

            enumNames = Enum.GetNames(typeof(T));
            enumValues = Enum.GetValues(typeof(T)).Cast<int>().ToArray();

            dropdown.ClearOptions();
            var options = new List<Dropdown.OptionData>();
            foreach (var name in enumNames) {
                options.Add(new Dropdown.OptionData(name));
            }
            dropdown.AddOptions(options);

            if (prefsKey != null) {
                this.prefsKey = prefsKey;
            }

            if (this.prefsKey != null) {
                int savedValInt = GetInt(this.prefsKey, (int)(object)defaultValue);
                int foundIndex = FindIndexOf(savedValInt);
                SetValue(foundIndex);
            }
        }

        public int FindIndexOf(int compareableValue, int defaultIndex = 0) {
            int foundIndex = defaultIndex;
            for (int i = 0; i < enumValues.Length; i++) {
                if (enumValues[i] == compareableValue) {
                    foundIndex = i;
                    break;
                }
            }
            return foundIndex;
        }

        public int FindIndexOf(T value, int defaultIndex = 0) {
            return FindIndexOf((int)(object)value, defaultIndex);
        }

        public void SetValue(T value) {
            dropdown.value = FindIndexOf(value);
        }

        public void SetValue(int valueIndex) {
            dropdown.value = valueIndex;
        }
    }
}

using UnityEngine;

namespace Screenshot {
    public static class Prefs {

        public static int GetInt(string key, int defaultValue = 0) {
            if (key == null) {
                return defaultValue;
            }
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetInt(key, defaultValue);
#else
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue);
#endif
        }

        public static bool GetBool(string key, bool defaultValue = false) {
            if (key == null) {
                return defaultValue;
            }
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetBool(key, defaultValue);
#else
            return UnityEngine.PlayerPrefs.GetInt(key, defaultValue == true ? 1 : 0) == 1 ? true : false;
#endif
        }

        public static string GetString(string key, string defaultValue = "") {
            if (key == null) {
                return defaultValue;
            }
#if UNITY_EDITOR
            string p = UnityEditor.EditorPrefs.GetString(key, defaultValue);
            p = string.IsNullOrWhiteSpace(p) ? defaultValue : p;
            return p;
#else
            string p = UnityEngine.PlayerPrefs.GetString(key, defaultValue);
            p = string.IsNullOrWhiteSpace(p) ? defaultValue : p;
            return p;
#endif
        }

        public static void SetInt(string key, int value) {
            if (key == null) {
                return;
            }
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetInt(key, value);
#else
            UnityEngine.PlayerPrefs.SetInt(key, value);
#endif
        }

        public static void SetBool(string key, bool value) {
            if (key == null) {
                return;
            }
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetBool(key, value);
#else
            UnityEngine.PlayerPrefs.SetInt(key, value == true ? 1 : 0);
#endif
        }

        public static void SetString(string key, string value) {
            if (key == null) {
                return;
            }
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString(key, value);
#else
            UnityEngine.PlayerPrefs.SetString(key, value);
#endif
        }
    }
}

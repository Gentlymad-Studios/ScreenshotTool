using UnityEngine;
using UnityEngine.UI;

namespace Screenshot {
    public class UIOption : MonoBehaviour {
        public Text label;

        public static T Instantiate<T>(string prefabPath, Transform parent) where T: Object {
            return Instantiate(Resources.Load<T>(prefabPath), parent, false);
        }

        public static UIOption Instantiate(string prefabPath, Transform parent){
            return Instantiate(Resources.Load<UIOption>(prefabPath), parent, false);
        }

        public void SetLabelText(string labelText) {
            if (labelText != null) {
                label.text = labelText;
            }
        }

        protected virtual void Initialize(string labelText) {
            SetLabelText(labelText);
        }
    }
}

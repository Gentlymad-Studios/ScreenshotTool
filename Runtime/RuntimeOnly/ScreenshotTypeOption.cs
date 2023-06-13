using UnityEngine;

namespace Screenshot {
    public class ScreenshotTypeOption : EnumOption<ScreenshotType> {

        public new static ScreenshotTypeOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<ScreenshotTypeOption>(prefabPath, parent);
        }
    }
}
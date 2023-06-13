using UnityEngine;
using UnityEngine.UIElements;

namespace Screenshot {
    public abstract class ScreenshotMethod {
#if UNITY_EDITOR
        public abstract VisualElement Draw();
#endif
        public abstract Texture2D TakeScreenshot();
        public abstract void CreateUIElements(Transform parent);
        public ContainerOption container = null;
    }
}


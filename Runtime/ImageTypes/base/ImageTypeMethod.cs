using UnityEngine;
using UnityEngine.UIElements;

namespace Screenshot {
    public abstract class ImageTypeMethod {
        public abstract string GetFileEnding();
#if UNITY_EDITOR
        public abstract VisualElement Draw();
#endif
        public abstract byte[] Encode(Texture2D texture);
        public abstract void CreateUIElements(Transform parent);
        public ContainerOption container = null;
    }
}

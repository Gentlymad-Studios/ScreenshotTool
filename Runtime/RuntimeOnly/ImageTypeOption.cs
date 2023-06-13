using UnityEngine;

namespace Screenshot {
    public class ImageTypeOption : EnumOption<ImageType> {
        public new static ImageTypeOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<ImageTypeOption>(prefabPath, parent);
        }
    }
}


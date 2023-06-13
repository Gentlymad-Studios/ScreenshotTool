using UnityEngine;

namespace Screenshot {
    public class EXRFlagsOption : EnumOption<Texture2D.EXRFlags> {
        public new static EXRFlagsOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<EXRFlagsOption>(prefabPath, parent);
        }
    }
}


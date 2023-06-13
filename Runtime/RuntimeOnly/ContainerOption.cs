using UnityEngine;

namespace Screenshot {
    public class ContainerOption : UIOption {
        public static new ContainerOption Instantiate(string prefabPath, Transform parent) {
            return Instantiate<ContainerOption>(prefabPath, parent);
        }
    }
}


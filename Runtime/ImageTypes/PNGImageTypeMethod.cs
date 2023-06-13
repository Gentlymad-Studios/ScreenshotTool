using UnityEngine;
using UnityEngine.UIElements;

namespace Screenshot {
    public class PNGImageTypeMethod : ImageTypeMethod {
        private const string messageText = "This image type has no options!";

#if UNITY_EDITOR
        public override VisualElement Draw() {
			Label messageLbl = new Label(messageText);
			messageLbl.SetEnabled(false);
			return messageLbl;
		}
#endif

        public override void CreateUIElements(Transform parent) {
            var container = ContainerOption.Instantiate(nameof(PrefabTypes.Container), parent);
            var message = UIOption.Instantiate(nameof(PrefabTypes.EmptyOption), container.transform);
            message.SetLabelText(messageText);
            this.container = container;
        }

        public override byte[] Encode(Texture2D texture) {
            return texture.EncodeToPNG();
        }

        public override string GetFileEnding() {
            return ".png";
        }
    }
}

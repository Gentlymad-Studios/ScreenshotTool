using UnityEngine;
using UnityEngine.UIElements;

namespace Screenshot {
    public class ReadPixelsMethod : ScreenshotMethod {
        private const string messageText = "This method has no options!";

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

        public override Texture2D TakeScreenshot() {
            //Create a texture to pass to encoding
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //Put buffer into texture
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); //Unity complains about this line's call being made "while not inside drawing frame", but it works just fine.*
            return texture;
        }
    }
}

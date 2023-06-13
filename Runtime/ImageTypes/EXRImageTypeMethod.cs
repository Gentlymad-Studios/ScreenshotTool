using UnityEngine;
using UnityEngine.UIElements;
using static Screenshot.Prefs;

namespace Screenshot {
    public class EXRImageTypeMethod : ImageTypeMethod {
        private const string flagsLabel = nameof(Texture2D.EXRFlags);
        private const string flagsKey = nameof(Screenshot.EXRImageTypeMethod) + "." + flagsLabel;

        private Texture2D.EXRFlags flags = Texture2D.EXRFlags.None;

        public EXRImageTypeMethod() {
            flags = (Texture2D.EXRFlags)GetInt(flagsKey);
        }

#if UNITY_EDITOR
        public override VisualElement Draw() {
			EnumField enumField = new EnumField(flagsLabel, flags);
			enumField.RegisterValueChangedCallback((evt) => {
				flags = (Texture2D.EXRFlags)evt.newValue;
				SetInt(flagsKey, (int)flags);
			});

			return enumField;
		}
#endif

        public override void CreateUIElements(Transform parent) {
            var container = ContainerOption.Instantiate(nameof(PrefabTypes.Container), parent);
            var flagsOption = EXRFlagsOption.Instantiate(nameof(PrefabTypes.EXRFlagsOption), container.transform);
            flagsOption.Initialize(flagsLabel, flagsKey, flags, (newFlag) => flags = newFlag);
            this.container = container;
        }

        public override byte[] Encode(Texture2D texture) {
            Texture2D tex = new Texture2D(texture.width, texture.height, TextureFormat.RGBAFloat, false);
            tex.SetPixels(texture.GetPixels());
            tex.Apply();
            byte[] bytes = tex.EncodeToEXR(flags);
            UnityEngine.Object.DestroyImmediate(tex);
            return bytes;
        }

        public override string GetFileEnding() {
            return ".exr";
        }
    }
}

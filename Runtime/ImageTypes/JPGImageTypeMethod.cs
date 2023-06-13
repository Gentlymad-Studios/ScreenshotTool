using UnityEngine;
using UnityEngine.UIElements;
using static Screenshot.Prefs;

namespace Screenshot {
    public class JPGImageTypeMethod : ImageTypeMethod {
        private const string qualityLabel = nameof(Quality);
        private const string QualityKey = nameof(Screenshot.JPGImageTypeMethod) + "." + qualityLabel;

        private int Quality = 90;
        private const int lValue = 1, rValue = 100;

        public JPGImageTypeMethod() {
            Quality = GetInt(QualityKey, 90);
        }

#if UNITY_EDITOR
        public override VisualElement Draw() {
			VisualElement sliderContainer = new VisualElement();
			sliderContainer.style.flexDirection = FlexDirection.Row;

			SliderInt slider = new SliderInt(qualityLabel, lValue, rValue);
			sliderContainer.Add(slider);
			IntegerField integerField = new IntegerField();
			sliderContainer.Add(integerField);

			slider.style.flexGrow = 1;
			slider.SetValueWithoutNotify(Quality);
			slider.RegisterValueChangedCallback((evt) => {
				Quality = evt.newValue;
				SetInt(QualityKey, Quality);
				integerField.SetValueWithoutNotify(Quality);
			});

			integerField.style.width = 50;
			integerField.SetValueWithoutNotify(Quality);
			integerField.RegisterValueChangedCallback((evt) => {
				int value = evt.newValue;
				if (value < lValue) {
					value = lValue;
				} else if (value > rValue) {
					value = rValue;
				}
				Quality = value;
				SetInt(QualityKey, Quality);
				integerField.SetValueWithoutNotify(Quality);
				slider.SetValueWithoutNotify(Quality);
			});

			return sliderContainer;
        }
#endif

        public override void CreateUIElements(Transform parent) {
            var container = ContainerOption.Instantiate(nameof(PrefabTypes.Container), parent);
            var intSlider = IntSliderOption.Instantiate(nameof(PrefabTypes.SliderOption), container.transform);
            intSlider.SetMinMax(lValue, rValue);
            intSlider.Initialize(qualityLabel, QualityKey, Quality, (v) => Quality = v);
            this.container = container;
        }

        public override byte[] Encode(Texture2D texture) {
            return texture.EncodeToJPG(Quality);
        }

        public override string GetFileEnding() {
            return ".jpg";
        }
    }
}


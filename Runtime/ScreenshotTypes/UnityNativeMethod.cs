using UnityEngine;
using UnityEngine.UIElements;
using static Screenshot.Prefs;

namespace Screenshot {
    public class UnityNativeMethod : ScreenshotMethod {
        private const string superSampleLabel = nameof(SuperSample);
        private const string superSampleKey = nameof(Screenshot.UnityNativeMethod) + "." + superSampleLabel;

        private int SuperSample = 1;
        private const int lValue = 1, rValue = 8;

        public UnityNativeMethod() {
            SuperSample = GetInt(superSampleKey, 1);
        }

#if UNITY_EDITOR
        public override VisualElement Draw() {
			VisualElement sliderContainer = new VisualElement();
			sliderContainer.style.flexDirection = FlexDirection.Row;

			SliderInt slider = new SliderInt(superSampleLabel, lValue, rValue);
			sliderContainer.Add(slider);
			IntegerField integerField = new IntegerField();
			sliderContainer.Add(integerField);

			slider.style.flexGrow = 1;
			slider.SetValueWithoutNotify(SuperSample);
			slider.RegisterValueChangedCallback((evt) => {
				SuperSample = evt.newValue;
				SetInt(superSampleKey, SuperSample);
				integerField.SetValueWithoutNotify(SuperSample);
			});

			integerField.style.width = 50;
			integerField.SetValueWithoutNotify(SuperSample);
			integerField.RegisterValueChangedCallback((evt) => {
				int value = evt.newValue;
				if (value < lValue) {
					value = lValue;
				} else if (value > rValue) {
					value = rValue;
				}
				SuperSample = value;
				SetInt(superSampleKey, SuperSample);
				integerField.SetValueWithoutNotify(SuperSample);
				slider.SetValueWithoutNotify(SuperSample);
			});

			return sliderContainer;
        }
#endif
        public override void CreateUIElements(Transform parent) {
            var container = ContainerOption.Instantiate(nameof(PrefabTypes.Container), parent);
            var intSlider = IntSliderOption.Instantiate(nameof(PrefabTypes.SliderOption), container.transform);
            intSlider.SetMinMax(lValue, rValue);
            intSlider.Initialize(superSampleLabel, superSampleKey, SuperSample, (v) => SuperSample = v);
            this.container = container;
        }

        public override Texture2D TakeScreenshot() {
            return ScreenCapture.CaptureScreenshotAsTexture(SuperSample);
        }

    }
}

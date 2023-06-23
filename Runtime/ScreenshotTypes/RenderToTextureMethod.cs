using UnityEngine;
using System.Linq;
using static Screenshot.Prefs;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

namespace Screenshot {
	public class RenderToTextureMethod : ScreenshotMethod {
		private const string superSampleLabel = nameof(SuperSample);
		private const string superSampleKey = nameof(Screenshot.RenderToTextureMethod) + "." + superSampleLabel;

		private const string renderFromAllLabel = nameof(RenderFromAll);
		private const string renderFromAllKey = nameof(Screenshot.RenderToTextureMethod) + "." + renderFromAllLabel;

		private RenderTexture m_Input;
		private int SuperSample = 1;
		private const int lValue = 1, rValue = 8;
		private Camera camera;
		private bool RenderFromAll = true;
		private string cameraName = "MainCamera";
		private GameObject gameObjectCopy;
		private Camera cameraCopy;
		private Canvas[] canvasCopies;

		public RenderToTextureMethod() {
			SuperSample = GetInt(superSampleKey, 1);
			RenderFromAll = GetBool(renderFromAllKey, true);
		}

#if UNITY_EDITOR
		public override VisualElement Draw() {
			VisualElement container = new VisualElement();

			ObjectField cameraField = new ObjectField("Camera");

			Toggle toggle = new Toggle("Render from all cameras");
			toggle.SetValueWithoutNotify(RenderFromAll);
			toggle.RegisterValueChangedCallback((evt) => {
				if (evt.newValue != RenderFromAll) {
					RenderFromAll = evt.newValue;
					SetBool(renderFromAllKey, RenderFromAll);
				}

				if (RenderFromAll) {
					cameraField.style.display = DisplayStyle.None;
				} else {
					cameraField.style.display = DisplayStyle.Flex;
				}
			});
			container.Add(toggle);

			if (!RenderFromAll) {
				cameraField.objectType = typeof(Camera);
				cameraField.allowSceneObjects = true;
				cameraField.SetValueWithoutNotify(camera);
				cameraField.RegisterValueChangedCallback((evt) => camera = evt.newValue as Camera);
				container.Add(cameraField);
			}

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
			container.Add(sliderContainer);

			return container;
		}
#endif

		public override void CreateUIElements(Transform parent) {
			var container = ContainerOption.Instantiate(nameof(PrefabTypes.Container), parent);

			var toggle = BoolOption.Instantiate(nameof(PrefabTypes.ToggleOption), container.transform);
			toggle.Initialize(renderFromAllLabel, renderFromAllKey, RenderFromAll, (v) => RenderFromAll = v);

			var inputField = StringOption.Instantiate(nameof(PrefabTypes.TextOption), container.transform);
			inputField.Initialize("Camera Name", "Screenshot.CameraName", cameraName, (v) => {
				cameraName = v;
				foreach (var cam in Camera.allCameras) {
					if (cam.name == cameraName || cam.name == $"{cameraName}(Clone)") {
						camera = cam;
						break;
					}
				}
			});

			var intSlider = IntSliderOption.Instantiate(nameof(PrefabTypes.SliderOption), container.transform);
			intSlider.SetMinMax(lValue, rValue);
			intSlider.Initialize(superSampleLabel, superSampleKey, SuperSample, (v) => SuperSample = v);

			this.container = container;
		}

		/*
        private void Capture(Camera referenceCam, RenderTexture input, ref bool success) {
            Canvas[] tmpCanvases = canvasCopies.Where(_ => _.worldCamera == referenceCam).ToArray();
            gameObjectCopy = UnityEngine.Object.Instantiate(referenceCam.gameObject, referenceCam.transform.position, referenceCam.transform.rotation);
            cameraCopy = gameObjectCopy.GetComponent<Camera>();
            cameraCopy.CopyFrom(referenceCam);
            //cameraCopy.enabled = false;
            cameraCopy.targetTexture = input;
            foreach (var canvas in tmpCanvases) {
                canvas.worldCamera = cameraCopy;
                UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.transform as RectTransform);
                Canvas.ForceUpdateCanvases();
            }
            cameraCopy.Render();
            cameraCopy.targetTexture = null;
            success = true;
            foreach (var canvas in tmpCanvases) {
                canvas.worldCamera = referenceCam;
            }
#if UNITY_EDITOR
            UnityEngine.Object.DestroyImmediate(gameObjectCopy);
#else
            UnityEngine.Object.Destroy(gameObjectCopy);
#endif
        }
        */

		private void Capture(Camera referenceCam, RenderTexture input, ref bool success) {
			referenceCam.enabled = false;
			referenceCam.targetTexture = input;
			referenceCam.Render();
			referenceCam.enabled = true;
			referenceCam.targetTexture = null;
			success = true;
		}

		public override Texture2D TakeScreenshot() {
			int width = Screen.width * SuperSample;
			int height = Screen.height * SuperSample;
			bool success = false;
			m_Input = new RenderTexture(width, height, 32);
			m_Input.Create();

			// get all canvases that use a dedicated UI camera
			//canvasCopies = UnityEngine.Object.FindObjectsOfType<Canvas>().Where(_ => _.renderMode != RenderMode.ScreenSpaceOverlay && _.worldCamera != null).ToArray();

			if (RenderFromAll) {
				// Render from all!
				foreach (Camera cam in Camera.allCameras) {
					Capture(cam, m_Input, ref success);
				}
			} else {
				// Try to get the main camera if camera is currently null
				if (camera == null) {
					camera = Camera.main;
				}

				if (camera != null) {
					Capture(camera, m_Input, ref success);
					// if still no camera could be found, throw error message
				} else {
#if UNITY_EDITOR
					Debug.Log($"Camera [{cameraName}] not found!");
#endif
				}
			}

			Texture2D tex = null;
			if (success) {
				RenderTexture.active = m_Input;
				tex = new Texture2D(width, height);
				tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
				tex.Apply();
				RenderTexture.active = null;
			}
			m_Input.Release();
			m_Input = null;
			return tex;
		}
	}
}

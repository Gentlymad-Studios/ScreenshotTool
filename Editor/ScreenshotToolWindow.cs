using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Screenshot.Prefs;

namespace Screenshot {

	public class ScreenshotToolWindow : EditorWindow {
		private const string windowName = "Screenshoter";
		public ScreenshotToolBase sTool;
		public VisualElement settingsContainer;
		public VisualElement methodContainer;

		[MenuItem("Tools/" + windowName, priority = 150)]
		static void OnWindow() {
			GetWindow<ScreenshotToolWindow>(windowName);
		}

		void OnEnable() {
			sTool = new ScreenshotToolBase();
		}

		private void CreateGUI() {
			if (Application.isPlaying) {
				VisualElement topContainer = new VisualElement();
				topContainer.style.flexDirection = FlexDirection.Row;
				topContainer.style.height = 25;
				rootVisualElement.Add(topContainer);

				Button createScreenshotBtn = new Button();
				createScreenshotBtn.style.flexGrow = 1;
				createScreenshotBtn.text = "Take Screenshot";
				createScreenshotBtn.clicked -= sTool.TakeScreenshot;
				createScreenshotBtn.clicked += sTool.TakeScreenshot;
				topContainer.Add(createScreenshotBtn);

				EnumField imageTypeMethodEf = new EnumField(sTool.imageType);
				imageTypeMethodEf.RegisterValueChangedCallback((evt) => {
					ImageType value = (ImageType)evt.newValue;
					if (sTool.imageType != value) {
						sTool.imageType = value;
						SetInt(nameof(ImageType), (int)sTool.imageType);
						SetupScreenshotSettingGUI();
					}
				});
				topContainer.Add(imageTypeMethodEf);
			} else {
				HelpBox playmodeWarning = new HelpBox("Enter play mode to make screenshots!", HelpBoxMessageType.Info);
				rootVisualElement.Add(playmodeWarning);
			}

			settingsContainer = new VisualElement();
			rootVisualElement.Add(settingsContainer);

			SetupScreenshotSettingGUI();
		}

		/// <summary>
		/// Setup GUI for Screenshot Settings
		/// </summary>
		private void SetupScreenshotSettingGUI() {
			settingsContainer.Clear();

			VisualElement subContainer = new VisualElement();
			subContainer.style.marginTop = 5;
			settingsContainer.Add(subContainer);

			Label title = new Label(sTool.imageType + " Format Options");
			title.style.unityFontStyleAndWeight = FontStyle.Bold;
			title.style.marginLeft = 3;
			subContainer.Add(title);

			subContainer.Add(sTool.CurrentImageMethod.Draw());

			subContainer.Add(Space(10));

			Label generalisticSettingsLbl = new Label("General");
			generalisticSettingsLbl.style.unityFontStyleAndWeight = FontStyle.Bold;
			generalisticSettingsLbl.style.marginLeft = 3;
			subContainer.Add(generalisticSettingsLbl);

			string saveLabel = string.IsNullOrWhiteSpace(sTool.saveLocation) ? "Choose a path..." : sTool.saveLocation;
			Button pathButton = new Button();
			pathButton.style.textOverflow = TextOverflow.Ellipsis;
			pathButton.text = "Save Path (" + saveLabel + ")";
			pathButton.clicked += () => {
				sTool.saveLocation = EditorUtility.OpenFolderPanel("Choose a location for screenshots", "", "");
				if (sTool.saveLocation == null) {
					sTool.saveLocation = "";
				}
				SetString(nameof(ScreenshotToolBase.saveLocation), sTool.saveLocation);

				string saveLabel = string.IsNullOrWhiteSpace(sTool.saveLocation) ? "Choose a path..." : sTool.saveLocation;
				pathButton.text = "Save Path (" + saveLabel + ")";
			};
			subContainer.Add(pathButton);

			EnumField methodEf = new EnumField("Method", sTool.screenshotType);
			methodEf.RegisterValueChangedCallback((evt) => {
				if (sTool.screenshotType != (ScreenshotType)evt.newValue) {
					sTool.screenshotType = (ScreenshotType)evt.newValue;
					SetInt(nameof(ScreenshotType), (int)sTool.screenshotType);
					SetupMethodGUI();
				}
			});
			subContainer.Add(methodEf);

			subContainer.Add(Space(10));

			methodContainer = new VisualElement();
			subContainer.Add(methodContainer);

			SetupMethodGUI();
		}

		/// <summary>
		/// Setup GUI for Method Settings
		/// </summary>
		private void SetupMethodGUI() {
			methodContainer.Clear();

			VisualElement subContainer = new VisualElement();
			methodContainer.Add(subContainer);

			Label title = new Label("" + sTool.screenshotType);
			title.style.unityFontStyleAndWeight = FontStyle.Bold;
			title.style.marginLeft = 3;
			subContainer.Add(title);
			subContainer.Add(sTool.CurrentScreenshotMethod.Draw());
		}

		/// <summary>
		/// Create Spacer 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private VisualElement Space(float value) {
			VisualElement spacer = new VisualElement();
			spacer.style.height = value;
			return spacer;
		}
	}

}


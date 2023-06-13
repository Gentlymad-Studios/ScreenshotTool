using UnityEngine;
using UnityEngine.UI;

namespace Screenshot {
    public class ScreenshotToolUI : MonoBehaviour {
        public RectTransform mainPanel;

        public ScreenshotToolBase sTool;
        public Button takeScreenshotButton;

        public Text imageTypeText;
        public ImageTypeOption imageTypeOption;
        public ContainerOption imageOptionsContainer;
        private RectTransform imageOptionsContainerRT;

        public ContainerOption screenshotOptionsContainer;
        private RectTransform screenshotOptionsContainerRT;
        public StringOption pathOption;
        public ScreenshotTypeOption screenshotTypeOption;
        public Text screenshotTypeText;

        void Awake() {
            imageOptionsContainerRT = imageOptionsContainer.transform as RectTransform;
            screenshotOptionsContainerRT = screenshotOptionsContainer.transform as RectTransform;

            sTool = new ScreenshotToolBase();
            sTool.imageType = ImageType.JPG;
            sTool.screenshotType = ScreenshotType.ReadPixels;

            sTool.onAfterScreenshotTaken = OnAfterScreenshotTaken;
            imageTypeOption.Initialize(null, null, sTool.imageType, OnImageTypeChange);
            screenshotTypeOption.Initialize(null, null, sTool.screenshotType, OnScreenshotTypeChange);
            takeScreenshotButton.onClick.AddListener(TakeScreenshot);
            pathOption.Initialize(null, sTool.saveLocationKey, sTool.saveLocation, OnSaveLocationChange);

            OnImageTypeChange(sTool.imageType);
            OnScreenshotTypeChange(sTool.screenshotType);
        }

        private void OnAfterScreenshotTaken() {
            mainPanel.gameObject.SetActive(true);
        }

        void TakeScreenshot() {
            mainPanel.gameObject.SetActive(false);
            if (string.IsNullOrWhiteSpace(sTool.saveLocation) || !System.IO.Directory.Exists(sTool.saveLocation)) {
#if !UNITY_EDITOR
                sTool.saveLocation = UnityEngine.Application.dataPath;
                Prefs.SetString(sTool.saveLocationKey, sTool.saveLocation);
                pathOption.inputField.text = sTool.saveLocation;
#else
                sTool.saveLocation = UnityEngine.Application.streamingAssetsPath;
                pathOption.inputField.text = sTool.saveLocation;
#endif
            }
            sTool.TakeScreenshot();
        }

#if DISABLESTEAMWORKS && !GOGGALAXY &&!STEAM
        /// <summary>
        /// TODO: Talk with Stephan how to handle this the correct way
        /// </summary>
        private void Update() {
            if (Input.GetKeyDown(KeyCode.F12)) {
                mainPanel.gameObject.SetActive(!mainPanel.gameObject.activeSelf);
            }
        }
#endif

        void OnImageTypeChange(ImageType imageType) {
            imageTypeText.text = imageType.ToString();
            sTool.imageType = imageType;

            foreach (var imageTypeMethod in sTool.ImageTypeMethods) {
                if (sTool.imageType == imageTypeMethod.Key) {
                    if (imageTypeMethod.Value.container == null) {
                        imageTypeMethod.Value.CreateUIElements(imageOptionsContainer.transform);
                    }
                    RectTransform rTrans = imageTypeMethod.Value.container.transform as RectTransform;
                    imageOptionsContainerRT.sizeDelta = new Vector2(imageOptionsContainerRT.sizeDelta.x, rTrans.childCount * 25);
                    imageTypeMethod.Value.container.gameObject.SetActive(true);
                } else if(imageTypeMethod.Value.container != null) {
                    imageTypeMethod.Value.container.gameObject.SetActive(false);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(mainPanel);
        }

        void OnScreenshotTypeChange(ScreenshotType screenshotType) {
            screenshotTypeText.text = screenshotType.ToString();
            sTool.screenshotType = screenshotType;

            foreach (var screenshotMethod in sTool.ScreenshotMethods) {
                if (sTool.screenshotType == screenshotMethod.Key) {
                    if (screenshotMethod.Value.container == null) {
                        screenshotMethod.Value.CreateUIElements(screenshotOptionsContainer.transform);
                    }
                    RectTransform rTrans = screenshotMethod.Value.container.transform as RectTransform;
                    screenshotOptionsContainerRT.sizeDelta = new Vector2(screenshotOptionsContainerRT.sizeDelta.x, rTrans.childCount * 25);
                    screenshotMethod.Value.container.gameObject.SetActive(true);
                } else if (screenshotMethod.Value.container != null) {
                    screenshotMethod.Value.container.gameObject.SetActive(false);
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(mainPanel);
        }

        void OnSaveLocationChange(string v) {
            sTool.saveLocation = v;
        }
    }
}


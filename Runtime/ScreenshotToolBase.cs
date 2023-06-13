using System;
using System.Collections.Generic;
using UnityEngine;
using static Screenshot.Prefs;

namespace Screenshot {
    public class ScreenshotToolBase {

        private int screenshotCounter = 0;

        public readonly string saveLocationKey = nameof(ScreenshotToolBase.saveLocation);
        public string saveLocation = "";
        public readonly string screenshotTypeKey = nameof(Screenshot.ScreenshotType);
        public ScreenshotType screenshotType = ScreenshotType.ReadPixels;
        public readonly string imageTypeKey = nameof(Screenshot.ImageType);
        public ImageType imageType = ImageType.JPG;
        public Action onAfterScreenshotTaken = null;

        public ScreenshotToolBase() {
            screenshotType = (ScreenshotType)GetInt(screenshotTypeKey, 0);
            imageType = (ImageType)GetInt(imageTypeKey, 1);
            saveLocation = GetString(saveLocationKey, Application.streamingAssetsPath);
        }

        private static Dictionary<ImageType, ImageTypeMethod> imageTypeMethods = null;
        public Dictionary<ImageType, ImageTypeMethod> ImageTypeMethods {
            get {
                if (imageTypeMethods == null) {
                    imageTypeMethods = new Dictionary<ImageType, ImageTypeMethod>() {
                        { ImageType.JPG, new JPGImageTypeMethod() },
                        { ImageType.PNG, new PNGImageTypeMethod() },
                        { ImageType.EXR, new EXRImageTypeMethod() },
                    };
                }
                return imageTypeMethods;
            }
        }
        public ImageTypeMethod CurrentImageMethod {
            get {
                return ImageTypeMethods[imageType];
            }
        }

        private static Dictionary<ScreenshotType, ScreenshotMethod> screenshotMethods = null;
        public Dictionary<ScreenshotType, ScreenshotMethod> ScreenshotMethods {
            get {
                if (screenshotMethods == null) {
                    screenshotMethods = new Dictionary<ScreenshotType, ScreenshotMethod>() {
                        { ScreenshotType.UnityNative, new UnityNativeMethod() },
                        { ScreenshotType.RenderToTexture, new RenderToTextureMethod() },
                        { ScreenshotType.ReadPixels, new ReadPixelsMethod() },
                    };
                }
                return screenshotMethods;
            }
        }
        public ScreenshotMethod CurrentScreenshotMethod {
            get {
                return ScreenshotMethods[screenshotType];
            }
        }

        private string UniquePathAndFilename {
            get {
                screenshotCounter++;
                return saveLocation
                    + System.IO.Path.DirectorySeparatorChar
                    + "Screenshot" + DateTime.Now.ToString("_yyyy-MM-dd_HH-mm-ss") + "_" + screenshotCounter;
            }
        }

        /// <summary>
        /// we need to WaitForEndOfFrame and some articles suggest we should also start in LateUpdate.
        /// This is not available to us in a editor script. We circumvent this problem by creating a monobehavior,
        /// as soon as the user wants to take the first screenshot.
        /// </summary>
        private ScreenshotItem _playmodeBridge = null;
        private ScreenshotItem PlaymodeBridge {
            get {
                if (_playmodeBridge == null) {
                    _playmodeBridge = UnityEngine.Object.FindObjectOfType<ScreenshotItem>();
                    if (_playmodeBridge == null) {
                        GameObject go = new GameObject("ScreenshotTool");
                        _playmodeBridge = go.AddComponent<ScreenshotItem>();
                        UnityEngine.Object.DontDestroyOnLoad(go);
                    }
                }
                return _playmodeBridge;
            }
        }

        private void TakeScreenshotBase() {
#if UNITY_EDITOR
            Debug.Log("saving screen");
#endif
            Texture2D texture = ScreenshotMethods[screenshotType].TakeScreenshot();
            if (texture != null) {
                byte[] bytes = ImageTypeMethods[imageType].Encode(texture);
                System.IO.File.WriteAllBytes(UniquePathAndFilename + ImageTypeMethods[imageType].GetFileEnding(), bytes);
                Destroy(texture);
            }
            onAfterScreenshotTaken?.Invoke();
        }

        public void TakeScreenshot() {
            if (!PlaymodeBridge.currentlyCapturing) {
                PlaymodeBridge.StartTakeScreenshotProcess(TakeScreenshotBase);
            }
        }

        public static void Destroy(UnityEngine.Object obj) {
            if (Application.isPlaying) {
                UnityEngine.Object.Destroy(obj);
            } else {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
    }
}


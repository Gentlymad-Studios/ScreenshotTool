using System.Collections;
using UnityEngine;
using System;

public class ScreenshotItem : MonoBehaviour {

    private Action takeScreenshotAction = null;
    private bool triggerCapture = false;
    [NonSerialized]
    public bool currentlyCapturing = false;

    public void StartTakeScreenshotProcess(Action method) {
        takeScreenshotAction = method;
        triggerCapture = currentlyCapturing = true;
    }

    private IEnumerator CaptureScreen() {
        yield return new WaitForEndOfFrame();
        takeScreenshotAction();
        currentlyCapturing = false;
    }

    private void LateUpdate() {
        if (triggerCapture) {
            StartCoroutine(CaptureScreen());
            triggerCapture = false;
        }
    }
}

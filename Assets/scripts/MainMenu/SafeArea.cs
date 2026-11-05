using UnityEngine;

// Put this on a full-screen panel inside the Canvas.
// All buttons and text go inside it, so nothing ends up under a notch.
[RequireComponent(typeof(RectTransform))]

public class SafeArea : MonoBehaviour
{
    private RectTransform panel;
    private Rect lastSafeArea;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        Apply();
    }

    void Update()
    {
        // re-apply if the screen changes (rotation, Device Simulator device switch)
        if (Screen.safeArea != lastSafeArea) Apply();
    }

    void Apply()
    {
        lastSafeArea = Screen.safeArea;

        Vector2 min = lastSafeArea.position;
        Vector2 max = lastSafeArea.position + lastSafeArea.size;

        // convert pixels to 0..1 anchor values
        min.x /= Screen.width;  min.y /= Screen.height;
        max.x /= Screen.width;  max.y /= Screen.height;

        panel.anchorMin = min;
        panel.anchorMax = max;
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}
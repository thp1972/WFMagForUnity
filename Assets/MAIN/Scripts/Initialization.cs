using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Initialization : MonoBehaviour
{
    const int WIDTH = 1920;
    const int HEIGHT = 1080;
    const float scale = 1;

    private void Awake()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            UnityEditorUtility.AddGameViewResolution(1920, 1080, "WM_1920x1080");
            UnityEditorUtility.AddGameViewResolution(800, 800, "WM_800x800");
            UnityEditorUtility.AddGameViewResolution(800, 600, "WM_800x600");
            UnityEditorUtility.AddGameViewResolution(400, 800, "WM_400x800");
            UnityEditorUtility.AddGameViewResolution(800, 400, "WM_800x400");
            UnityEditorUtility.AddGameViewResolution(1200, 700, "WM_1200x700");
            UnityEditorUtility.AddGameViewResolution(600, 800, "WM_600x800");
            UnityEditorUtility.AddGameViewResolution(400, 400, "WM_400x400");
            UnityEditorUtility.AddGameViewResolution(1000, 562, "WM_1000x562");
            UnityEditorUtility.AddGameViewResolution(600, 400, "WM_600x400");
            UnityEditorUtility.AddGameViewResolution(600, 600, "WM_600x600");
        }
#endif
    }

    private void Start()
    {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#else
        UnityEditorUtility.ChangeGameViewResolution(WIDTH, HEIGHT, $"WM_{WIDTH}x{HEIGHT}");
        UnityEditorUtility.ChangeGameViewScale(scale);
#endif
    }
}

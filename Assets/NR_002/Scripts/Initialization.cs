using UnityEngine;

namespace NR_002
{
    [ExecuteInEditMode]
    public class Initialization : MonoBehaviour
    {
        const int WIDTH = 400;
        const int HEIGHT = 800;
        const float scale = 1;

        void Awake()
        {
#if UNITY_EDITOR
            UnityEditorUtility.ChangeGameViewResolution(WIDTH, HEIGHT, $"WM_{WIDTH}x{HEIGHT}");
            UnityEditorUtility.ChangeGameViewScale(scale);
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
}

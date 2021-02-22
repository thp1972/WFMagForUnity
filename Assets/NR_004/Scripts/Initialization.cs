using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_004
{
    [ExecuteInEditMode]
    public class Initialization : MonoBehaviour
    {
        const int WIDTH = 800;
        const int HEIGHT = 800;
        const float scale = 1;

        private void Start()
        {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#else
            UnityEditorUtility.ChangeGameViewResolution(WIDTH, HEIGHT, $"WM_{WIDTH}x{HEIGHT}");
            UnityEditorUtility.ChangeGameViewScale(scale);
#endif
            ScreenUtility.GameResolution = new Vector2(WIDTH, HEIGHT);
        }
    }
}
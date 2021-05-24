using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_013
{
    [ExecuteInEditMode]
    public class Initialization : MonoBehaviour
    {
        const int WIDTH = 1000;
        const int HEIGHT = 1000 * 9 / 16;
        const float scale = 1;

        private void Awake()
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

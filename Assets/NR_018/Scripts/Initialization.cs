using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_018
{
    [ExecuteInEditMode]
    public class Initialization : MonoBehaviour
    {
        const int WIDTH = 600;
        const int HEIGHT = 600;
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

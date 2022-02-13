using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

// By Pellegrino ~thp~ Principe
namespace NR_025
{
    [ExecuteInEditMode]
    public class InitializationMatch3 : MonoBehaviour
    {
        [Header("Initialization Properties")]
        public int WIDTH = 800;
        public int HEIGHT = 600;
        public float scale = 1;
        public int frameRate = 60; // this is the Pygame default frame rate

        string assetPath = "Assets/__TEMPLATES__/Data/";

        private void Awake()
        {
            // warning this not working if the procjet is built (AssetDatabse -> UnityEditor)
            // change in future
            INITDefinitions def = AssetDatabase.LoadAssetAtPath<INITDefinitions>($"{assetPath}INITDefinition.asset");
            WIDTH = def.width;
            HEIGHT = def.height;
            scale = def.scale;
            frameRate = def.frameRate;

            var pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
            if (pixelPerfectCamera)
            {
                pixelPerfectCamera.refResolutionX = WIDTH;
                pixelPerfectCamera.refResolutionY = HEIGHT;
                Camera.main.orthographicSize = HEIGHT / 2;
            }
        }

        private void Start()
        {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#else
            UnityEditorUtility.ChangeGameViewResolution(WIDTH, HEIGHT, $"WM_{WIDTH}x{HEIGHT}");
            UnityEditorUtility.ChangeGameViewScale(scale);
#endif
            ScreenUtility.GameResolution = new Vector2(WIDTH, HEIGHT);
            Application.targetFrameRate = frameRate;
        }
    }
}

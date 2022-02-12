using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

using Kyusyukeigo.Helper;

public class UnityEditorUtility
{
    public enum SceneMode { _2D_, _3D_ };

    public static void AddGameViewResolution(int width, int height, string name)
    {
        var groupType = GameViewSizeHelper.GetCurrentGameViewSizeGroupType();

        var gameViewSize = new GameViewSizeHelper.GameViewSize();
        gameViewSize.type = GameViewSizeHelper.GameViewSizeType.FixedResolution;
        gameViewSize.width = width;
        gameViewSize.height = height;
        gameViewSize.baseText = name;

        if (!GameViewSizeHelper.Contains(groupType, gameViewSize))
        {
            GameViewSizeHelper.AddCustomSize(groupType, gameViewSize);
        }
    }

    public static void ChangeGameViewResolution(int width, int height, string name)
    {
        var groupType = GameViewSizeHelper.GetCurrentGameViewSizeGroupType();

        GameViewSizeHelper.ChangeGameViewSize(groupType, new GameViewSizeHelper.GameViewSize()
        { type = GameViewSizeHelper.GameViewSizeType.FixedResolution, width = width, height = height, baseText = name });
    }

    public static void ChangeGameViewScale(float value)
    {
        GameViewSizeHelper.ChangeGameViewScale(value);
    }

    public static void ChangeSceneViewMode(SceneMode sceneMode)
    {
        bool in2D = sceneMode == SceneMode._2D_ ? true : false;
        SceneView.lastActiveSceneView.in2DMode = in2D;
        SceneView.lastActiveSceneView.camera.orthographic = in2D;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().orthographic = in2D;
    }

    public static void AddSceneToBuildSettings(string scenePath)
    {
        // scenes containts all the scenes already in the build settings
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        // only bother adding scenes we don't have already.
        foreach (EditorBuildSettingsScene scene in scenes)
        {
            if (scene.path == scenePath)
            {
                Debug.LogWarning("Scene already inserted!");
                return;
            }
        }

        EditorBuildSettingsScene newScene = new EditorBuildSettingsScene();
        newScene.path = scenePath;
        newScene.enabled = true;
        scenes.Add(newScene);
        EditorBuildSettings.scenes = scenes.ToArray();
    }
}

#endif
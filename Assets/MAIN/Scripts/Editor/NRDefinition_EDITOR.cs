using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Wireframe.EditorUI
{
    public class NRDefinition_EDITOR : EditorWindow
    {
        static NRDefinition_EDITOR win;
        static NRDefinitions nrDefdata;
        static INITDefinitions initDefData;
        public static bool exitWithoutData;

        int width;
        int height;
        float scale;
        int frameRate;
        bool in3D;
        Sprite cover;
        Sprite coverSwap;
        string sceneName;
        string nrAndDate;

        enum WindowType { END, INIT };
        static WindowType windowType;

        public static void LaunchEndStructureSetupWindow(NRDefinitions nRDefinitions)
        {
            nrDefdata = nRDefinitions;
            win = NRDefinition_EDITOR.GetWindow<NRDefinition_EDITOR>(true, "NRDefinition End Structure Setup");

            windowType = WindowType.END;
            win.ShowModal();
        }
        public static void LaunchInitStructureSetupWindow(INITDefinitions initDef)
        {
            initDefData = initDef;
            win = NRDefinition_EDITOR.GetWindow<NRDefinition_EDITOR>(true, "NRDefinition Init Structure Setup");

            windowType = WindowType.INIT;
            win.ShowModal();
        }

        private void OnGUI()
        {
            switch (windowType)
            {
                case WindowType.END:
                    width = EditorGUILayout.IntField("Width:", width);
                    nrDefdata.width = width;
                    height = EditorGUILayout.IntField("Height:", height);
                    nrDefdata.height = height;
                    scale = EditorGUILayout.FloatField("Scale:", scale);
                    nrDefdata.scale = scale;
                    in3D = EditorGUILayout.Toggle("In 3D:", in3D);
                    nrDefdata.in3D = in3D;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Cover:");
                    cover = EditorGUILayout.ObjectField(cover, typeof(Sprite), allowSceneObjects: false) as Sprite;
                    nrDefdata.cover = cover;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Cover Swap:");
                    coverSwap = EditorGUILayout.ObjectField(coverSwap, typeof(Sprite), allowSceneObjects: false) as Sprite;
                    nrDefdata.coverSwap = coverSwap;
                    EditorGUILayout.EndHorizontal();

                    sceneName = EditorGUILayout.TextField("Scene Name:", sceneName);
                    nrDefdata.sceneName = sceneName;
                    nrAndDate = EditorGUILayout.TextField("Nr And Date:", nrAndDate);
                    nrDefdata.NrAndDate = nrAndDate;
                    break;

                case WindowType.INIT:

                    sceneName = EditorGUILayout.TextField("Scene Name:", sceneName);
                    initDefData.sceneName = sceneName;
                    width = EditorGUILayout.IntField("Width:", width);
                    initDefData.width = width;
                    height = EditorGUILayout.IntField("Height:", height);
                    initDefData.height = height;
                    scale = EditorGUILayout.FloatField("Scale:", scale);
                    initDefData.scale = scale;
                    frameRate = EditorGUILayout.IntField("Frame Rate:", frameRate);
                    initDefData.frameRate = frameRate;
                    break;
            }

            if (GUILayout.Button("Set data"))
            {
                win.Close();
            }

            if (GUILayout.Button("Exit from setting"))
            {
                exitWithoutData = true;
                win.Close();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NRDefinition_EDITOR : EditorWindow
{
    static NRDefinition_EDITOR win;
    static NRDefinitions data;

    int width;
    int height;
    float scale;
    bool in3D;
    Sprite cover;
    Sprite coverSwap;
    string sceneName;
    string nrAndDate;

    public static void LaunchSetupWindow(NRDefinitions nRDefinitions)
    {
        data = nRDefinitions;
        win = NRDefinition_EDITOR.GetWindow<NRDefinition_EDITOR>(true, "NRDefinition Setup");
        win.ShowModal();
    }

    private void OnGUI()
    {
        width = EditorGUILayout.IntField("Width:", width);
        data.width = width;
        height = EditorGUILayout.IntField("Height:", height);
        data.height = height;
        scale = EditorGUILayout.FloatField("Scale:", scale);
        data.scale = scale;
        in3D = EditorGUILayout.Toggle("In 3D:", in3D);
        data.in3D = in3D;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Cover:");
        cover = EditorGUILayout.ObjectField(cover, typeof(Sprite), allowSceneObjects: false) as Sprite;
        data.cover = cover;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Cover Swap:");
        coverSwap = EditorGUILayout.ObjectField(coverSwap, typeof(Sprite), allowSceneObjects: false) as Sprite;
        data.coverSwap = coverSwap;
        EditorGUILayout.EndHorizontal();

        sceneName = EditorGUILayout.TextField("Scene Name:", sceneName);
        data.sceneName = sceneName;
        nrAndDate = EditorGUILayout.TextField("Nr And Date:", nrAndDate);
        data.NrAndDate = nrAndDate;

        if (GUILayout.Button("Set data"))
        {   win.Close();
        }
    }
}

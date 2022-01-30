using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NRHandler))]
public class NRHandler_EDITOR : Editor
{
    NRHandler targetNRHandler;
    string wantedDefinitionNumber;
    string assetPath = "Assets/MAIN/Data/";

    private void OnEnable()
    {
        targetNRHandler = target as NRHandler;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);
        wantedDefinitionNumber = EditorGUILayout.TextField("Definition Number:", wantedDefinitionNumber);
        if (GUILayout.Button("Add Last NR. Definition", GUILayout.Height(25)))
        {
            NRDefinitions currentNRDefinition = AssetDatabase.LoadAssetAtPath<NRDefinitions>
                ($"{assetPath}NRDefinition_0{wantedDefinitionNumber}.asset");

            if (currentNRDefinition)
            {
                AddNewNrDefinition(currentNRDefinition);
            }
            else Debug.LogError($"{assetPath}NRDefinition_0{wantedDefinitionNumber}.asset NOT FOUND!!!");
        }
        GUILayout.Space(20);
    }

    private void AddNewNrDefinition(NRDefinitions currentNRDefinition)
    {
        List<NRDefinitions> tmpDefs = targetNRHandler.definitions.ToList();

        NRDefinitions emptyNRDefinition = AssetDatabase.LoadAssetAtPath<NRDefinitions>($"{assetPath}Empty.asset");
        int lastIndexElement = targetNRHandler.definitions.Length - 1;

        for (int s = lastIndexElement - 2; s <= lastIndexElement; s++)
        {
            if (targetNRHandler.definitions[s].sceneName == "Empty")
            {
                tmpDefs[s] = currentNRDefinition;
                break;
            }
        }

        if(targetNRHandler.definitions[lastIndexElement].sceneName != "Empty")
        {
            tmpDefs.Add(currentNRDefinition);
            tmpDefs.Add(emptyNRDefinition);
            tmpDefs.Add(emptyNRDefinition);
        }
 
        NRDefinitions[] newDefs = tmpDefs.ToArray();
        targetNRHandler.definitions = newDefs;
    }
}

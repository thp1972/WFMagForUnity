using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace Wireframe.EditorUI
{
    public class NewMagStructure_EDITOR
    {
        static string dataPath = "/MAIN/Data/";

        // this path is for AssetDatabase API: it requires relative path from Project
        static string assetPath = "Assets/MAIN/Data/";
        static string imgLastPath = "Assets/MAIN/Images/_LAST_/";
        static string imgPath = "Assets/MAIN/Images";

        [MenuItem("Wireframe Mag Tools/Create Init Structure")]
        public static void CreatInitStructure()
        {
        }

        [MenuItem("Wireframe Mag Tools/Create End Structure")]
        public static void CreatEndStructure()
        {
            string appPath = Application.dataPath;
            string finalPath = $"Assets/{dataPath}";

            int lastNumber = GetLastNumberId(appPath + dataPath);

            // create scriptable asset
            NRDefinitions newNrDef = ScriptableObject.CreateInstance<NRDefinitions>();

            // open a Window to get data: this is a modal window
            NRDefinition_EDITOR.LaunchSetupWindow(newNrDef);

            if(NRDefinition_EDITOR.exitWithoutData)
            {
                Debug.LogWarning("No data setted!");
                return;
            }

            AssetDatabase.CreateAsset(newNrDef, $"{finalPath}/NRDefinition_0{lastNumber + 1}.asset");

            var guid = AssetDatabase.CreateFolder(imgPath, $"NR_0{lastNumber + 1}");

            if (string.IsNullOrEmpty(guid))
            {
                Debug.LogError($"Folder NR_0{lastNumber + 1} not created!");
                return;
            }

            AssetDatabase.MoveAsset($"{imgLastPath}{newNrDef.cover.name}.png",
                                    $"{imgPath}/NR_0{lastNumber + 1}/{newNrDef.cover.name }.png");

            AssetDatabase.MoveAsset($"{imgLastPath}{newNrDef.coverSwap.name}.png",
                                    $"{imgPath}/NR_0{lastNumber + 1}/{newNrDef.coverSwap.name}.png");

            AssetDatabase.Refresh();

            AddNewNrDefinition(newNrDef);

            var scenePath = $"Assets/NR_0{lastNumber + 1}/Scenes/";
            AddSceneInBuild($"{scenePath}{newNrDef.sceneName}.unity");
        }

        static void AddSceneInBuild(string sceneAsset)
        {
            UnityEditorUtility.AddSceneToBuildSettings(sceneAsset);
        }

        static int GetLastNumberId(string dataPath)
        {
            int count = 0;
            string[] fileEntries = Directory.GetFiles(dataPath);
            foreach (var file in fileEntries)
            {
                if (file.IndexOf("NRDefinition") != -1 && file.IndexOf(".meta") == -1)
                    count++;
            }
            return count;
        }

        static void AddNewNrDefinition(NRDefinitions currentNRDefinition)
        {
            NRHandler targetNRHandler = GameObject.FindObjectOfType<NRHandler>();
            if (targetNRHandler)
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

                if (targetNRHandler.definitions[lastIndexElement].sceneName != "Empty")
                {
                    tmpDefs.Add(currentNRDefinition);
                    tmpDefs.Add(emptyNRDefinition);
                    tmpDefs.Add(emptyNRDefinition);
                }

                NRDefinitions[] newDefs = tmpDefs.ToArray();
                targetNRHandler.definitions = newDefs;
            }
            else
            {
                Debug.LogError($"targetNRHandler is NULL!!!");
            }
        }
    }
}
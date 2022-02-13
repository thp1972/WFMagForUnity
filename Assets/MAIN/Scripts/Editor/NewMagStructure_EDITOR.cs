using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;

namespace Wireframe.EditorUI
{

    public class NewMagStructure_EDITOR
    {
        static string appPath = Application.dataPath;
        static string finalPath = $"Assets/{dataPath}";
        static string dataPath = "/MAIN/Data/";

        // this path is for AssetDatabase API: it requires relative path from Project
        static string assetPath = "Assets/MAIN/Data/";

        static string imgLastPath = "Assets/MAIN/Images/_LAST_/";
        static string imgPath = "Assets/MAIN/Images";

        static string templateDataPath = "Assets/__TEMPLATES__/Data/";
        static string templatePath = "Assets/__TEMPLATES__/";

        static List<string> folderNames = new List<string>()
        {
            "Images", "Prefabs", "Scenes", "Scripts", "Sprites"
        };

        [MenuItem("Wireframe Mag Tools/Create Init Structure")]
        public static void CreatInitStructure()
        {
            int lastNumber = GetLastNumberId(appPath + dataPath);

            // create scriptable asset
            INITDefinitions newInitDef = ScriptableObject.CreateInstance<INITDefinitions>();

            // open a Window to get data: this is a modal window
            NRDefinition_EDITOR.LaunchInitStructureSetupWindow(newInitDef);

            if (NRDefinition_EDITOR.exitWithoutData)
            {
                Debug.LogWarning("No data setted!");
                return;
            }

            newInitDef.scriptName = $"Initialization{newInitDef.sceneName}";
            newInitDef.namespaceName = $"NR_0{lastNumber + 1}";

            // this aset is then read by Initialization script when the main scene load
            AssetDatabase.DeleteAsset($"{templateDataPath}/INITDefinition.asset");
            AssetDatabase.CreateAsset(newInitDef, $"{templateDataPath}/INITDefinition.asset");

            // create dir structure
            string rootFolder = $"NR_0{lastNumber + 1}";
            AssetDatabase.CreateFolder("Assets", rootFolder);
            foreach (var folderName in folderNames)
            {
                AssetDatabase.CreateFolder($"Assets/{rootFolder}", folderName);
            }

            AssetDatabase.CopyAsset($"{templatePath}TEMPLATE.unity", $"Assets/{rootFolder}/Scenes/{newInitDef.sceneName}.unity");
            AssetDatabase.CopyAsset($"{templatePath}Initialization.cs", $"Assets/{rootFolder}/Scripts/{newInitDef.scriptName}.cs");

            // now change into the file the namespace and the class name itself
            ChangeInnerFileNameDatas($"{appPath}/{rootFolder}/Scripts/{newInitDef.scriptName}.cs",
                                     $"{newInitDef.scriptName}",
                                     $"NR_0{lastNumber + 1}");

            AssetDatabase.Refresh();

            // this is mandatory cause the script file is not inserted into Assembly-CSharp and the PreInit script
            // does not find the cs file to make the automatic addcomponent statement
            CompilationPipeline.RequestScriptCompilation();
        }


        [MenuItem("Wireframe Mag Tools/Create End Structure")]
        public static void CreatEndStructure()
        {
            int lastNumber = GetLastNumberId(appPath + dataPath);

            // create scriptable asset
            NRDefinitions newNrDef = ScriptableObject.CreateInstance<NRDefinitions>();

            // open a Window to get data: this is a modal window
            NRDefinition_EDITOR.LaunchEndStructureSetupWindow(newNrDef);

            if (NRDefinition_EDITOR.exitWithoutData)
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

            AddNewNrDefinition(newNrDef);

            var scenePath = $"Assets/NR_0{lastNumber + 1}/Scenes/";
            AddSceneInBuild($"{scenePath}{newNrDef.sceneName}.unity");

            AssetDatabase.Refresh();
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

        static void ChangeInnerFileNameDatas(string filePath, string newClassName, string newNameSpaceName)
        {
            try
            {
                String[] fileText = File.ReadAllLines(filePath);

                for (int i = 0; i < fileText.Length; i++)
                {
                    if (Regex.IsMatch(fileText[i], @"\bnamespace\b"))
                    {
                        // match the identifier of a class so it can be replaced by 'className'
                        // we use a Positive Lookbehind...
                        String regexPattern = @"(?<=namespace )\w+";
                        fileText[i] = Regex.Replace(fileText[i], regexPattern, newNameSpaceName);
                        File.WriteAllLines(filePath, fileText);
                    }

                    if (Regex.IsMatch(fileText[i], @"\bclass\b"))
                    {
                        // match the identifier of a class so it can be replaced by 'className'
                        // we use a Positive Lookbehind...
                        String regexPattern = @"(?<=class )\w+";
                        fileText[i] = Regex.Replace(fileText[i], regexPattern, newClassName);
                        File.WriteAllLines(filePath, fileText);
                    }
                }
            }
            catch (Exception exc) { Debug.Log(exc.Message); }
        }
    }
}
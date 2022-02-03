using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Wireframe.EditorUI
{
    public class NewMagStructure_EDITOR
    {
        static string dataPath = "/MAIN/Data/";
        static string imgPath = "/MAIN/Images/";

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

            // open a Window to get data


            AssetDatabase.CreateAsset(newNrDef, $"{finalPath}/NRDefinition_0{lastNumber + 1}.asset");

            // create img folder
            Directory.CreateDirectory($"{appPath}{imgPath}/NR_0{lastNumber + 1}");

            AssetDatabase.Refresh();
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

    }
}
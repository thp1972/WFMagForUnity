using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace TEMPLATE
{
    [ExecuteInEditMode]
    public class PreInit : MonoBehaviour
    {
        string assetPath = "Assets/__TEMPLATES__/Data/";

        void Awake()
        {
            // warning this not working if the procjet is built (AssetDatabse -> UnityEditor)
            // change in future
            INITDefinitions def = AssetDatabase.LoadAssetAtPath<INITDefinitions>($"{assetPath}INITDefinition.asset");

            // Attempt to search for type on the loaded assemblies
            Assembly[] currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type type = null;
            foreach (Assembly assembly in currentAssemblies)
            {
            
                type = assembly.GetType($"NR_025.Initialization{def.sceneName}");

                if (type != null)
                {
                    Debug.Log(assembly);
                    break;
                }
                //Type t = Type.GetType($"Initialization{def.sceneName}");
            }

            //Debug.Log(Type.GetType($"NR_025.Initialization{def.sceneName}"));
            

            //Debug.Log(Type.GetType($"NR_025.Initialization{def.sceneName}"));
            gameObject.AddComponent(Type.GetType($"{def.namespaceName}.{def.scriptName}"));
        }     
    }
}
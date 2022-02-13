using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wireframe Magazine/INIT Definitions", fileName = "INITDefinition")]

public class INITDefinitions : ScriptableObject
{
    public string sceneName;
    public string scriptName;
    public string namespaceName;
    public int width;
    public int height;
    public float scale;
    public int frameRate;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wireframe Magazine/NR Definitions", fileName = "NRDefinition")]
public class NRDefinitions : ScriptableObject
{    
    public int width;
    public int height;
    public float scale;
    public bool in3D;

    public Sprite cover;
    public Sprite coverSwap;

    public string sceneName; 
}

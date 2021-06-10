using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class test : MonoBehaviour
{
    public Texture2D myTexture;
    RenderTexture rTex;

    List<(float, float)> previouspositions;


    // Start is called before the first frame update
    void Start()
    {
        previouspositions = (from i in Enumerable.Range(0, 100) select (400f - i * 4, 100f)).ToList();
        print(previouspositions);

        previouspositions.Insert(0, (404, 100));
        previouspositions.RemoveAt(previouspositions.Count - 1);

        print(previouspositions);

    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

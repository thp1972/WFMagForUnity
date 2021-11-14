using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;

public class SurfaceTest : MonoBehaviour
{
    Surface scrambleSurface;
    // Start is called before the first frame update
    void Start()
    {
        scrambleSurface = new Surface((800, 600), SRCALPHA: false);

        print(scrambleSurface.GetAt((0, 0)));

    }

    // Update is called once per frame
    void Update()
    {

        print(scrambleSurface.GetAt(((int)Input.mousePosition.x, (int)Input.mousePosition.y)));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;

public class Scramble : MonoBehaviour
{
    Surface scrambleSurface;
    int landLevel = 600;
    int roofLevel = 10;
    int landChange = -3;
    int roofChange = 3;
    Actor jet;

    // Start is called before the first frame update
    void Awake()
    {
        scrambleSurface = new Surface((800, 600));
        jet = new Actor("jet", (400, 300));
        jet.SortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        jet.Draw();
    }
}

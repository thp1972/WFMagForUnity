using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;
using System.Linq;

public class Scramble : MonoBehaviour
{
    Surface scrambleSurface;
    int landLevel = 600;
    int roofLevel = 10;
    int landChange = -3;
    int roofChange = 3;
    int speed = 3;
    bool crash = false;

    Actor jet;
    public GameObject spacePrefab;
    GameObject space;

    // Start is called before the first frame update
    void Awake()
    {
        scrambleSurface = new Surface((800, 600), SRCALPHA: true);
        jet = new Actor("jet", (400, 300));

        jet.SortingOrder = 1;
        space = Instantiate(spacePrefab);

        UpdateLand();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateLand();
        Draw();
    }

    void UpdateLand()
    {
        if (Random.Range(0f, 10f) == 3) roofChange = (int)Random.Range(0f, 6f) - 3;
        if (Random.Range(0f, 10f) == 3) landChange = (int)Random.Range(0f, 6f) - 3;

        roofLevel += roofChange;
        landLevel += landChange;
        landLevel = Limit(landLevel, 200, 590);
        roofLevel = Limit(roofLevel, 10, 400);

        if (roofLevel > landLevel - 200) roofLevel = landLevel - 200;

        scrambleSurface.Scroll(-1, 0);

        DrawLand();
    }


    private void Draw()
    {
        if (crash)
        {

        }
        else
        {
            ScreenUtility.Blit(space, (0, 0));
            jet.Draw();
        }

    }

    void DrawLand()
    {
        foreach (var i in Enumerable.Range(0, 600))
        {
            (byte, byte, byte, byte) c = (0, 0, 0, 0);
            if (i > landLevel)
            {
                var g = Limit(i - landLevel, 0, 255);
                c = (255, g, 0, 255);
                // print("land " + c);
            }
            else if (i < roofLevel)
            {
                var r = Limit(roofLevel - i, 0, 255);
                c = (255, r, 0, 255);
                // print("roof " + c);
            }

            scrambleSurface.SetAt((799, i), c);
        }
    }

    private byte Limit(int n, int minn, int maxn)
    {
        return (byte)Mathf.Max(Mathf.Min(maxn, n), minn);
    }
}
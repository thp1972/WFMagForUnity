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

        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var _ in Enumerable.Range(0, (int)Mathf.Ceil(speed)))
        {
            UpdateLand();
        }
        
        Draw();
    }

    void UpdateLand()
    {
        if (MathUtility.RandInt(0, 10) == 3) roofChange = MathUtility.RandInt(0, 6) - 3;
        if (MathUtility.RandInt(0, 10) == 3) landChange = MathUtility.RandInt(0, 6) - 3;

        roofLevel += roofChange;
        landLevel += landChange;
        landLevel = Limit(landLevel, 200, 590);
        roofLevel = Limit(roofLevel, 10, 400);

        if (roofLevel > landLevel - 200) roofLevel = landLevel - 200;

        //scrambleSurface.Scroll2(-50, 0);
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

        /*scrambleSurface.SetAt((0, 0),  (255, 0, 0, 255));

        scrambleSurface.Apply();

        scrambleSurface.Scroll(1, 0);
        */
    }

    int ii = 0;
    void DrawLand()
    {
        foreach (var i in Enumerable.Range(0, 600))
        {
            (byte, byte, byte, byte) c = (0, 0, 0, 0);

            if (i > landLevel)
            {
                var g = (byte)Limit(i - landLevel, 0, 255);
                c = (255, g, 0, 255);
            }
            else if (i < roofLevel)
            {
                var r = (byte)Limit(roofLevel - i, 0, 255);
                c = (255, r, 0, 255);
            }

            scrambleSurface.SetAt((799-ii, i), c);
        }

        scrambleSurface.Apply();
        ii++;
        if (ii == 799)
        {
            scrambleSurface.SurfaceClear();
            ii = 0;
        }
    }

    private int Limit(int n, int minn, int maxn)
    {
        // this is the same as Unity's Mathf.Clamp
        return Mathf.Max(Mathf.Min(maxn, n), minn);
    }
}

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
    float speed = 3;
    bool crash = false;

    Actor jet;
    public GameObject spacePrefab;
    GameObject space;


    // Start is called before the first frame update
    void Awake()
    {
        scrambleSurface = new Surface((2800, 600), SRCALPHA: true);
        scrambleSurface.SetOrigin(new Vector2(1800, 0));


        jet = new Actor("jet", (400, 300));

        jet.SortingOrder = 1;
        space = Instantiate(spacePrefab);

        //DrawLand();

        Draw();
    }

    // Update is called once per frame
    void Update()
    {
        if(crash == false)
        {
            if (EventsDetector.Keyboard.Up) jet.Y -= speed;
            if (EventsDetector.Keyboard.Down) jet.Y += speed;
            if (EventsDetector.Keyboard.Left) speed = Limit(speed - 0.1f, 1f, 10f);
            if (EventsDetector.Keyboard.Right) speed = Limit(speed + 0.1f, 1f, 10f);

            jet.X = 310 + (speed * 30);
        }

        foreach (var _ in Enumerable.Range(0, (int)Mathf.Ceil(speed)))
        {
            UpdateLand();
        }

        //print($"{jet.X + 32} {jet.Y}");

        if (scrambleSurface.GetAt(((int)Mathf.Ceil(jet.X + 32), (int)Mathf.Ceil(jet.Y))) != (0, 0, 0, 0))
            crash = true;

        Draw();

        if(Input.GetMouseButton(0))
        print(scrambleSurface.GetAt(((int)Input.mousePosition.x, (int)Input.mousePosition.y)));
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

        //scrambleSurface.Scroll(-100, 0);
        DrawLand();
    }

    private void Draw()
    {
        if (crash)
        {
            ScreenUtility.Fill(((byte)Random.Range(100, 200 + 1), 0, 0), useTexture : true);
        }
        else
        {
            ScreenUtility.Blit(space, (0, 0));
            jet.Draw();
        }
    }

    public int XX = 0;
    int ii = 0;
    void DrawLand()
    {
        if (ii == 2800)
        {        
            return;
        }

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

            scrambleSurface.SetAt((ii, i), c);
            //scrambleSurface.SetAt((2800, i), c);
        }
        scrambleSurface.Apply();

        ii++;
        /*if (ii == 2800)
        {
            //scrambleSurface.SurfaceClear(); 
            ii = 0;
        }*/
    }

    private int Limit(int n, int minn, int maxn)
    {
        // this is the same as Unity's Mathf.Clamp
        return Mathf.Max(Mathf.Min(maxn, n), minn);
    }

    private float Limit(float n, float minn, float maxn)
    {
        // this is the same as Unity's Mathf.Clamp
        return Mathf.Max(Mathf.Min(maxn, n), minn);
    }
}

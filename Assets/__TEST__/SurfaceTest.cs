using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;

public class SurfaceTest : MonoBehaviour
{
    Surface scrambleSurface;
    // Start is called before the first frame update

    Actor jet;

    public bool scroll;

    public Texture2D texture;
    void Start()
    {
        scrambleSurface = new Surface((1800, 600), texture, SRCALPHA: false);
        jet = new Actor("jet", (10, 300));

        jet.SortingOrder = 1;
        jet.Draw();

        if (scroll)
            //scrambleSurface.SetOrigin(new Vector2(1300, 0));
            scrambleSurface.Scroll(-40, 0);
    }

    bool crash;
    float speed = 3;

    // Update is called once per frame
    void Update()
    {
        if (scroll)
            //scrambleSurface.SetOrigin(new Vector2(1300, 0));
            scrambleSurface.Scroll(-40, 0);

        if (crash == false)
        {
            if (EventsDetector.Keyboard.Up) jet.Y -= speed;
            if (EventsDetector.Keyboard.Down) jet.Y += speed;
            if (EventsDetector.Keyboard.Left) speed = Limit(speed - 0.1f, 1f, 10f);
            if (EventsDetector.Keyboard.Right) speed = Limit(speed + 0.1f, 1f, 10f);

            jet.X = 210 + (speed * 30);

            // jet.X = speed * 30;
        }






        if (Input.GetKeyDown(KeyCode.A))
        {
            /*Vector2 pp = ScreenUtility.InversePosition(0, 0);

            Vector2 p = Camera.main.ScreenToWorldPoint(new Vector2((int)Input.mousePosition.x, (int)Input.mousePosition.y));

            jet.X = pp.x;
            jet.Y = pp.y;

            print($"mouse {xx} {yy} {scrambleSurface.GetAt2((xx, yy))}");
            print($"jet {jet.X} {jet.Y} {scrambleSurface.GetAt(((int)jet.X + (900 - 800 / 2), (int)jet.Y))}");*/

            var off = -scrambleSurface._surface.transform.position.x;

           // Vector2 p = (new Vector2((int)Input.mousePosition.x /*+ ((900 - 800 / 2) + off )*/, (int)Input.mousePosition.y));
           // print($"mouse {p.x} {p.y} {scrambleSurface.GetAt2(((int)p.x, (int)p.y))}");

       }


        var xx = (int)Mathf.Ceil((jet.X) + ( (900 - 800 / 2)  ));
        var yy = (int)Mathf.Ceil(jet.Y);

        Vector2 p = (new Vector2((int)Input.mousePosition.x /*+ ((900 - 800 / 2) + off )*/, (int)Input.mousePosition.y));
        print($"mouse {p.x} {p.y} {scrambleSurface.GetAt2(((int)p.x, (int)p.y))}");




        if (scrambleSurface.GetAt((xx, yy)) != (0, 0, 0, 255))
        {
            crash = true;
            speed = 0;
            print("crash");
        }
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

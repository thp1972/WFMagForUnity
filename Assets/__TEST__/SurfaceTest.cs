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

    public float off;
    void Start()
    {
        scrambleSurface = new Surface((800, 600), texture, SRCALPHA: false);
        jet = new Actor("jet", (0, 100));

        scrambleSurface.SetOrigin(new Vector2(400, 0));

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

        // if (crash == false)
        // {
        if (EventsDetector.Keyboard.Up) jet.Y -= speed;
        if (EventsDetector.Keyboard.Down) jet.Y += speed;
        if (EventsDetector.Keyboard.Left) speed = Limit(speed - 0.1f, 1f, 100f);
        if (EventsDetector.Keyboard.Right) speed = Limit(speed + 0.1f, 1f, 100f);

        jet.X = 10 + (speed * 30);


        // }

        //jet.X = 500;



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


        var xx = (int)Mathf.Ceil(jet.X + 32);
        var yy = (int)Mathf.Ceil(jet.Y);

        Vector2 position = Vector2.zero;
        if (scrambleSurface.PositionInsideSurface((xx, yy), ref position))
        {
            if (scrambleSurface.GetAt(((int)position.x, (int)position.y)) != (0, 0, 0, 255))
            {
                crash = true;
                // speed = 0;
                print("crash");
            }
            else
            {
                crash = false;
                // speed = 0;
                print("NOT crash");
            }

        }
        else
        {
            print("OUT OF THE SURFACE");
        }


        //Vector2 p = (new Vector2((int)Input.mousePosition.x, (int)Input.mousePosition.y));
        //print($"mouse {p.x} {p.y} {scrambleSurface.GetAt2(((int)p.x, (int)p.y))}");

        
      //var ox = xx - scrambleSurface.X;
      //var oy = yy - scrambleSurface.Y;

      //print("...checking... ox=" + ox + " oy=" + oy + " xx=" + xx + " yy=" + yy + " X=" + scrambleSurface.X + " Y=" + scrambleSurface.Y);

      //if (ox >= 0 && ox <= 799 && oy >=0 && oy <= 600)
      //{
      //    if (scrambleSurface.X <= xx)
      //        ox = xx - scrambleSurface.X;
      //    else if (scrambleSurface.X >= xx)
      //        ox = scrambleSurface.X + xx;
         
      //  // print("jet " + xx + " " + ox + " " + scrambleSurface.GetAt(((int)ox, yy)));


      //  if (scrambleSurface.GetAt(((int)ox, (int)oy)) != (0, 0, 0, 255))
      //      {
      //          crash = true;
      //          // speed = 0;
      //          print("crash");
      //      }
      //      else
      //      {
      //          crash = false;
      //          // speed = 0;
      //          print("NOT crash");
      //      }

      //  }
    
    
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class test : MonoBehaviour
{
    public SpriteRenderer myTexture;
    public GameObject pixel;

    public int posX;
    public int posY;


    RenderTexture rTex;

    List<(float, float)> previouspositions;

    SpriteUtility su;

    static List<List<Color>> pixels;

    private Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        su = new SpriteUtility(myTexture);

        /*pixels = (from x in Enumerable.Range(0, 11)
                  select (from y in Enumerable.Range(0, 11)
                          select su.GetPixelAt(x, 10-y)).ToList()).ToList();

        int ix = 0;
        int iy = 0;
        foreach (List<Color> j in pixels)
        {
            foreach (Color x in j)
            {
                print($"[{ix}][{iy}] = {x}");
                iy++;
            }
            ix++;
            iy = 0;
        }*/

         su.SetPixelAt(new Vector2(0,0), Color.white);
         su.SetPixelAt(new Vector2(1, 0), Color.clear);
    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    // Update is called once per frame
    void Update()
    {

       // su.SetPixelAt(new Vector2(0,0), Color.white);
       // su.SetPixelAt(new Vector2(1, 0), Color.yellow);




        //var pos = Input.mousePosition;
        //var cpos = Camera.main.ScreenToWorldPoint(pos);

        //Color c = su.GetPixelAt(5, 6);

        //print(c);
        /*
        su.SetPixelAt(new Vector2(-300, 160), Color.black);

        c = su.GetPixelAt(100, 560);
        print(c);
        */
        /*Color32 c2 = new Color32((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255), (byte)(c.a * 255));


        pixel.transform.position = ScreenUtility.Position(posX, posY);

        print($"color at screen {pos} and world {cpos} = {c2}");
        */

        // print(pos + " " + su.WorldPosToLocalTexturePos(cpos));

        //print(su.WorldPosToLocalTexturePos(new Vector2(397f, -398.5f)));

    }
}

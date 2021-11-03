using PygameZero;
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

    SpriteUtility _spriteUtility;

    static List<List<Color>> pixels;

    private Texture2D texture;

    public GameObject quad;

    MeshRenderer meshRenderer;

    Surface scrambleSurface;

    // Start is called before the first frame update
    void Start()
    {
        scrambleSurface = new Surface((800, 600), SRCALPHA: false);
        /*
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                scrambleSurface.SetAt((x, y), (255, 0, 0, 255));
            }
        }

        scrambleSurface.Apply();
        */
        //scrambleSurface.Scroll2(-50, 0);



        /*meshRenderer = quad.GetComponent<MeshRenderer>();

        texture = new Texture2D(meshRenderer.material.mainTexture.width, meshRenderer.material.mainTexture.height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.SetPixels(((Texture2D)meshRenderer.material.mainTexture).GetPixels());
        texture.Apply();

        meshRenderer.material.mainTexture = texture;

        for (int y = 0; y < 600; y++)
        {
            for (int x = 0; x < 800; x++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }

        texture.Apply();

        /*texture.SetPixel(0, 0, Color.red);
        texture.SetPixel(1, 0, Color.red);
        texture.SetPixel(2, 0, Color.red);

        texture.Apply();
        */

        //_spriteUtility = new SpriteUtility(myTexture);

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

        //su.SetPixelAt(new Vector2(0,0), Color.white);
        //su.SetPixelAt(new Vector2(1, 0), Color.clear);

        /*
        for (int x = 0; x < 800; x++)
        {
            for (int y = 0; y < 600; y++)
            {
                _spriteUtility.SetPixelAt(ScreenUtility.Position(new Vector2(x, y)), Color.red);
            }
        }

        _spriteUtility.Apply();
        */

    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    float step = 0;
    int ii = 0;


    public float roofChange = 3;
    public float landChange = -3;
    public float roofLevel = 10;
    public float landLevel = 600;


    private float Limit(float n, float minn, float maxn)
    {
        // this is the same as Unity's Mathf.Clamp
        return Mathf.Max(Mathf.Min(maxn, n), minn);
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)Random.Range(0f, 10f) == 3) roofChange = (int)Random.Range(0f, 6f) - 3;
        if ((int)Random.Range(0f, 10f) == 3) landChange = (int)Random.Range(0f, 6f) - 3;

        roofLevel += roofChange;
        landLevel += landChange;
        landLevel = Limit(landLevel, 200, 590);
        roofLevel = Limit(roofLevel, 10, 400);

        if (roofLevel > landLevel - 200) roofLevel = landLevel - 200;

        /*foreach (var i in Enumerable.Range(0, 600))
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

            scrambleSurface.SetAt((799, i), c);
        }

        scrambleSurface.Apply();
        */




        int sy = Random.Range(0, 400);
        //int sx = Random.Range(0, 400);


        for (int y = sy; y < sy + 20; y++)
        {

            scrambleSurface.SetAt((799 - ii, y), (255, 0, 0, 255));

        }
        ii--;


        scrambleSurface.Apply();


        /*
        foreach (var i in Enumerable.Range(0, 600))
        {
            (byte, byte, byte, byte) c = (0, 0, 0, 0);


            byte g = 255;
            c = (255, g, 0, 255);

            byte r = 125;
            c = (255, r, 0, 255);

            int lim = Random.Range(100, 300);

            texture.SetPixel(799, lim, new Color(c.Item1, c.Item2, c.Item3, c.Item4));

            ii = i;
        }

        if (ii >= 599)
        {
            texture.Apply();
            ii = 0;
        }


        step += Time.deltaTime;
        meshRenderer.material.SetTextureOffset("_MainTex", new Vector2(step, 0));



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

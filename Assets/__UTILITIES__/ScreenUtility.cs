using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// this class assumes that screen coordinates 0,0 starts from TOP-LEFT
// as stated by Pygame documentation
// it converts pygames coordinates to Unity coordinates where coordinates 0,0
// starts at center of the screen
public class ScreenUtility
{
    private static Vector2 gameResolution;
    public static Vector2 GameResolution
    {
        get { return gameResolution; }
        set { gameResolution = value; }
    }

    /// <summary>
    ///  it converts pygames coordinates to Unity coordinates where coordinates 0,0
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 Position(float x, float y)
    {
        return TopLeft + new Vector2(x, -y);
    }

    /// <summary>
    ///  it converts pygames coordinates to Unity coordinates where coordinates 0,0
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Vector2 Position(Vector2 position)
    {
        return Position(position.x, position.y);
    }

    /// <summary>
    /// it converts Unity coordinates to pygames coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static Vector2 InversePosition(float x, float y)
    {
        return new Vector2(Mathf.Abs(TopLeft.x - x), TopLeft.y - y);
    }

    /// <summary>
    /// Convert pixel coordinate on Y from Unity to Pygame; Y in Unity starts bottom while in Pygame starts on top
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector2 InvertYOnPixelPosition(Vector2 pos)
    {
        var y = ScreenUtility.GameResolution.y - pos.y;
        return new Vector2(pos.x, y);
    }

    public static Vector2 BottomRight
    {
        get { return new Vector2(gameResolution.x / 2, -gameResolution.y / 2); }
    }

    public static Vector2 BottomLeft
    {
        get { return new Vector2(-gameResolution.x / 2, -gameResolution.y / 2); }
    }

    public static Vector2 TopRight
    {
        get { return new Vector2(gameResolution.x / 2, gameResolution.y / 2); }
    }

    public static Vector2 TopLeft
    {
        get { return new Vector2(-gameResolution.x / 2, gameResolution.y / 2); }
    }

    // blitter objects memory
    static Dictionary<string, GameObject> blitterObjects = new Dictionary<string, GameObject>();
    /// <summary>
    /// Draw a gameobject at specific positions
    /// </summary>
    /// <param name="g">GameObject to draw</param>
    /// <param name="pos">GameObject postion as tuple</param>
    /// <param name="blitRef">GameObject name reference</param>
    /// <param name="instantiate">Does instantiate the GameObject?</param>
    public static void Blit(GameObject g, (float, float) pos, string blitRef = "", bool instantiate = false, bool isActive = true)
    {
        var blitterObj = g; // already instance?
        if (instantiate)
        {
            blitterObj = GameObject.Instantiate(g);
            if (blitRef.Length != 0)
            {
                blitterObjects[blitRef] = blitterObj;
                blitterObj.name = blitRef;
            }
        }
        else // takes form the memory, but if not found take g
        {
            GameObject go;
            var res = blitterObjects.TryGetValue(blitRef, out go);
            if (res)
                blitterObj = go;
        }

        blitterObj.transform.position = Position(pos.Item1, pos.Item2);
        blitterObj.SetActive(isActive);
    }

    /// <summary>
    /// Removes all objects from the scene: useful if you need to draw lots of objects that are constantly moving around.
    /// </summary>
    /// <param name="gameobjectName">object name to search</param>
    public static void BlitClear(string gameobjectName)
    {
        foreach (var g in GameObject.FindObjectsOfType<GameObject>())
        {
            if (g.name.IndexOf(gameobjectName) != -1)
            {
                g.SetActive(false);
                GameObject.Destroy(g);
            }
        }
    }

    static bool useTextureAlreadyInstantiated;
    static GameObject fill_surface;
    public static void Fill((byte, byte, byte) color, bool useTexture = false)
    {
        if (useTexture) // if use texture
        {
            // put a texture in front of all other objects with a semi-transp.
            if (!useTextureAlreadyInstantiated)
            {
                var op = Addressables.LoadAssetAsync<GameObject>("Fill");
                var imageToInstantiate = op.WaitForCompletion(); // force sync!
                fill_surface = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);
                fill_surface.transform.localScale = new Vector3(GameResolution.x, GameResolution.y, 1);
                fill_surface.transform.position = new Vector3(fill_surface.transform.position.x,
                                                              fill_surface.transform.position.y,
                                                              Camera.main.transform.position.z + 1);
                useTextureAlreadyInstantiated = true;
            }
            // you can change color, i.e. in an Update with random colors
            fill_surface.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(color.Item1 / 255f,
                                                                                            color.Item2 / 255f,
                                                                                            color.Item3 / 255f,
                                                                                            0.75f));
        }
        else Camera.main.backgroundColor = new Color32(color.Item1, color.Item2, color.Item3, 255);
    }

}

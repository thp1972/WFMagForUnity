using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    /// <summary>
    /// Draw a gameobject at specific positions
    /// </summary>
    /// <param name="g"></param>
    /// <param name="pos"></param>
    public static void Blit(GameObject g, (float, float) pos)
    {
        g.transform.position = Position(pos.Item1, pos.Item2);
        if (!g.activeSelf)
            g.SetActive(true);
    }
}

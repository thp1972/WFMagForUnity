using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUtility
{
    private static Vector2 gameResolution;
    public static Vector2 GameResolution
    {
        get { return gameResolution; }
        set { gameResolution = value; }        
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
}

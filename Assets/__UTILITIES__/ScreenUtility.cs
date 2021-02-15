using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUtility
{
    public static Vector2 BottomRight
    {
        get { return new Vector2(Screen.width / 2, -Screen.height / 2); }
    }

    public static Vector2 BottomLeft
    {
        get { return new Vector2(-Screen.width / 2, -Screen.height / 2); }
    }

    public static Vector2 TopRight
    {
        get { return new Vector2(Screen.width / 2, Screen.height / 2); }
    }

    public static Vector2 TopLeft
    {
        get { return new Vector2(-Screen.width / 2, Screen.height / 2); }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsDetector : MonoBehaviour
{
    public static class Keyboard
    {
        public static bool Space { get; set; }
        public static bool E { get; set; }
        public static bool Up { get; set; }
        public static bool Down { get; set; }
        public static bool Left { get; set; }
        public static bool Right { get; set; }
    }

    public static class Mouse
    {
        public static bool Left { get; set; }
        public static bool Right { get; set; }
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard.Space = Input.GetKeyDown(KeyCode.Space);
        Keyboard.E = Input.GetKeyDown(KeyCode.E);

        Keyboard.Down = Input.GetKey(KeyCode.DownArrow);
        Keyboard.Up = Input.GetKey(KeyCode.UpArrow);
        Keyboard.Left = Input.GetKey(KeyCode.LeftArrow);
        Keyboard.Right = Input.GetKey(KeyCode.RightArrow);

        Mouse.Left = Input.GetMouseButtonDown(0);
        Mouse.Right = Input.GetMouseButtonDown(1);
    }
}

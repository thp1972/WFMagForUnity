using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsDetector : MonoBehaviour
{
    public static class Keyboard
    {
        public static bool Space { get; set; }
        public static bool E { get; set; }
    }

    // Update is called once per frame
    void Update()
    {
        Keyboard.Space = Input.GetKeyDown(KeyCode.Space);
        Keyboard.E = Input.GetKeyDown(KeyCode.E);
    }
}

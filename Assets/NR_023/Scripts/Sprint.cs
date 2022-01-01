using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTILITY;

// By Pellegrino ~thp~ Principe
namespace NR_023
{
    public class Sprint : MonoBehaviour
    {
        int WIDTH = 800;
        int HEIGHT = 300;

        public static float ACCELERATION = 0.005f;
        public static float DECELERATION = 0.0008f;

        // number of pixels representing 1m
        public static int SCALE = 75;

        Sprinter sprinter;

        public GameObject track;
        public GameObject _25m;
        public GameObject _50m;
        public GameObject _75m;
        public GameObject finishLine;

        Draw draw;
        Text distanceText, timeText;

        void Start()
        {
            draw = GetComponent<Draw>();
            distanceText = draw.CreateText(fontSize: 32, color: (255, 255, 255), isActive: false);
            distanceText.alignment = TextAnchor.MiddleLeft;
      
            timeText = draw.CreateText(fontSize: 32, color: (255, 255, 255), isActive : false);
            timeText.alignment = TextAnchor.MiddleLeft;

            // it seems that Pygame font pixel is non rendered as indicated; i.e fontsize=32 seems not being 32 pixel exactly
            // so, scaling at 0.68% does render in Unity as Pygame
            distanceText.transform.localScale = Vector2.one * 0.68f;
            timeText.transform.localScale = Vector2.one * 0.68f;

            sprinter = new Sprinter();
        }

        void Update()
        {
            // move and animate the sprinter
            sprinter.Update();
            // add to the finish time if race is still in progress
            if (sprinter.distance < 100)
                sprinter.finishTime = Time.time;

            Draw();
        }

        void Draw()
        {
            // draw the track
            ScreenUtility.Blit(track, (0, 0), "track", isActive: true);

            // draw distance markers and finish line
            DisplayAt(_25m, 25, 200);
            DisplayAt(_50m, 50, 200);
            DisplayAt(_75m, 75, 200);
            DisplayAt(finishLine, 100, 230);

            // draw the sprinter
            sprinter.Draw();

            // draw the current distance and time
            draw.DrawTex(distanceText, topLeft: (20, 20), text: $"Distance (m): {(int)Mathf.Min(100, sprinter.distance)}");
            draw.DrawTex(timeText, topLeft: (250, 20), text: $"Time (s): {System.Math.Round(sprinter.finishTime - sprinter.startTime, 2)}");
        }

        // a function to display an image at a specific distance along the track
        void DisplayAt(GameObject img, float pos, float y)
        {
            ScreenUtility.Blit(img, (sprinter.X + pos * SCALE - sprinter.distance * SCALE, y), isActive: true);
        }
    }
}
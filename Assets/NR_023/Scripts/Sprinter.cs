using PygameZero;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// By Pellegrino ~thp~ Principe
namespace NR_023
{
    public class Sprinter : Actor
    {
        public float startTime;
        public float finishTime;

        List<string> runFrames;
        int timeOnCurrentFrame;
        string lastPressed;
        bool keyPressed;

        public float distance;

        public Sprinter() : base(image: "idle", pos: (200, 220))
        {
            startTime = Time.time;
            finishTime = Time.time;
            runFrames = (from i in Enumerable.Range(1, 16 - 1) select ($"run{i}")).ToList();
            timeOnCurrentFrame = 0;
            speed = 0;
            lastPressed = null;
            keyPressed = false;
            distance = 0;
        }

        // advances the sprinter to the next frame
        public void nextFrame()
        {
            // start the running animation if currently idle
            if (Image == "idle")
                Image = runFrames[0];
            else
            {
                // find the next image in the list, wrapping back to the first image
                // when the end of the list is reached
                var nextImageIndex = (runFrames.IndexOf(Image) + 1) % runFrames.Count;
                Image = runFrames[nextImageIndex];
            }
        }

        // checks whether left and right arrow keys are
        // being pressed alternately
        public bool isNextKeyPressed()
        {
            if (EventsDetector.Keyboard.Left && lastPressed != "left" && !EventsDetector.Keyboard.Right)
            {
                lastPressed = "left";
                return true;
            }
            if (EventsDetector.Keyboard.Right && lastPressed != "right" && !EventsDetector.Keyboard.Left)
            {
                lastPressed = "right";
                return true;
            }
            return false;
        }

        public void Update()
        {
            // update sprinter's speed
            // accelerate on alternate key presses
            if (isNextKeyPressed() && distance < 100)   
                speed = Mathf.Min(speed + Sprint.ACCELERATION, 0.15f);           
            // decelerate if no key pressed
            else
                speed = Mathf.Max(0, speed - Sprint.DECELERATION);


            Debug.Log(speed);

            // use the sprinter's speed to update the distance            
            distance += speed;

            // animate the sprinter in relation to its speed
            timeOnCurrentFrame += 1;
            if (speed > 0 && timeOnCurrentFrame > 10 - speed * 75)
            {
                timeOnCurrentFrame = 0;
                nextFrame();
            }
            // set to idle animation if not moving
            if (speed <= 0)
                Image = "idle";       
        }

    }
}
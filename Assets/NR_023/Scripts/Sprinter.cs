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

        public int distance;

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

        public void nextFrame()
        {

        }



    }
}
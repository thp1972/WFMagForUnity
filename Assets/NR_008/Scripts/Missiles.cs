using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UTILITY;

namespace NR_008
{
    public class Missiles : MonoBehaviour
    {
        // Unity stuff
        public Draw draw;

        public static Missiles instance;
        public static int GRAVITY = -5;
        public static int TRAIL_LENGTH = 100; // 400 are too expensive for the current implementation
        public static int TRAIL_BRIGHTNESS = 100;
        public static (byte, byte, byte) FLARE_COLOR = (255, 220, 160);

        public List<Missile> missiles = new List<Missile>();

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            InvokeRepeating("NewMissile", 0, 5);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var m in missiles)
            {
                m.Draw();
                m.Step(Time.deltaTime);
            }
        }

        void NewMissile()
        {
            var m = new Missile(x: Random.Range(600, 801), vx: Random.Range(-70, -11));
            missiles.Add(m);
        }
    }
}
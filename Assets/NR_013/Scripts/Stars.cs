using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UTILITY;
using PygameZero;

namespace NR_013
{
    public class Stars : MonoBehaviour
    {
        class Star
        {
            public (float, float) pos;
            public (float, float) vel;
            public float speed;
            public byte brightness;

            public LineRenderer lineRenderer;

            public Star((float, float) pos, (float, float) vel)
            {
                this.pos = pos;
                this.vel = vel;
                brightness = 10;
                speed = MathUtility.Hypo(vel);

                lineRenderer = draw.CreateLine((brightness, brightness, brightness));
            }

            public (float, float) EndPos
            {
                // Get the point where the star trail ends.
                get
                {
                    var x = pos.Item1;
                    var y = pos.Item2;
                    var vx = vel.Item1;
                    var vy = vel.Item2;

                    return (x - vx * warp_factor * TRAIL_LENGTH / 60, y - vy * warp_factor * TRAIL_LENGTH / 60);
                }
            }
        }

        class Rect
        {
            UnityEngine.Rect _rect;

            public Rect(int x, int y, int width, int height)
            {
                _rect = new UnityEngine.Rect(x, y, width, height);
            }

            public bool CollidePoint(Star star, (float, float) point)
            {
                var coll = _rect.Contains(new Vector2(point.Item1, point.Item2));
                if (!coll) GameObject.Destroy(star.lineRenderer.gameObject);
                return coll;
            }
        }

        const int WIDTH = 1000;
        const int HEIGHT = 1000 * 9 / 16;
        const float ACCEL = 1.0f; // Warp factor per second
        const float DRAG = 0.71f; // Fraction of speed per second
        const int TRAIL_LENGTH = 2;
        const float MIN_WARP_FACTOR = 0.1f;
        Rect BOUNDS = new Rect(0, 0, WIDTH, HEIGHT);

        static float warp_factor = MIN_WARP_FACTOR;
        float centerx = WIDTH / 2;
        float centery = HEIGHT / 2;
        List<Star> stars = new List<Star>();

        static Draw draw;
        Text headUp1, headUp2;
        bool jumpStart = true;

        // Start is called before the first frame update
        void Start()
        {
            draw = GetComponent<Draw>();
            headUp1 = draw.CreateText(fontSize: 40, color: (180, 160, 0));
            headUp2 = draw.CreateText(text: "Hold SPACE to accelerate", fontSize: 30, color: (90, 80, 0));

            // Jump-start the star field
            foreach (var _ in Enumerable.Range(0, 30))
                UpdateStep(0.5f);

            foreach (var _ in Enumerable.Range(0, 5))
                UpdateStep(1 / 60);

            jumpStart = false;
        }


        private void Update()
        {
            if (!jumpStart)
                UpdateStep(Time.deltaTime);
        }

        // Update is called once per frame
        void UpdateStep(float deltaTime)
        {
            float dt = deltaTime;

            // Calculate the new warp factor
            if (Input.GetKey(KeyCode.Space))
                warp_factor += ACCEL * dt; // If space is held, accelerate

            // Apply drag to slow us, regardless of whether space is held
            warp_factor = MIN_WARP_FACTOR + (warp_factor - MIN_WARP_FACTOR) * Mathf.Pow(DRAG, dt);

            ScreenDrawText();

            // Spawn new stars until we have 300
            while (stars.Count < 300)
            {
                // Pick a direction and speed
                var angle = Random.Range(-Mathf.PI, Mathf.PI);
                var speed = 255 * Mathf.Pow(Random.Range(0.3f, 1.0f), 2);

                var dx = Mathf.Cos(angle);
                var dy = Mathf.Sin(angle);

                // Turn the direction into position and velocity vectors
                var d = Random.Range(25f + TRAIL_LENGTH, 100f);
                var pos = (centerx + dx * d, centery + dy * d);
                var vel = (speed * dx, speed * dy);

                stars.Add(new Star(pos, vel));
            }

            // Update the positions of stars
            foreach (var s in stars)
            {
                var x = s.pos.Item1;
                var y = s.pos.Item2;
                var vx = s.vel.Item1;
                var vy = s.vel.Item2;

                // Move according to speed and warp factor
                x += vx * warp_factor * dt;
                y += vy * warp_factor * dt;
                s.pos = (x, y);

                // Grow brighter
                s.brightness = (byte)Mathf.Min(s.brightness + warp_factor * 200 * dt, s.speed);

                // Get faster
                s.vel = (vx * Mathf.Pow(2, dt), vy * Mathf.Pow(2, dt));
            }

            // Drop any stars that are completely off-screen
            stars = (from star in stars
                     where BOUNDS.CollidePoint(star, star.EndPos)
                     select star).ToList();


            ScreenDrawLine();
        }

        void ScreenDrawText()
        {
            draw.DrawTex(headUp1, midBottom: (WIDTH / 2, HEIGHT - 40), text: $"||| Warp {warp_factor,0:f1} |||");
            draw.DrawTex(headUp2, midBottom: (WIDTH / 2, HEIGHT - 8));
        }

        void ScreenDrawLine()
        {
            // Draw all our stars
            foreach (var star in stars)
            {
                var pos = ScreenUtility.Position(star.pos.Item1, star.pos.Item2);
                var endPos = ScreenUtility.Position(star.EndPos.Item1, star.EndPos.Item2);
                var color = (star.brightness, star.brightness, star.brightness); // a grey
                draw.DrawLine(star.lineRenderer, color, (endPos.x, endPos.y), (pos.x, pos.y));
            }
        }
    }
}

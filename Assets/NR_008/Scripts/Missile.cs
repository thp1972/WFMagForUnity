using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nito.Collections;
using System.Linq;

namespace NR_008
{
    public class Missile
    {
        float x, y, vx, vy;
        Deque<(float, float)> trail;
        float t;

        LineRenderer sameLineRenderer;
        GameObject sameCircle;
        GameObject missileGameObject;

        public Missile(float x, float vx, float y = 0, float vy = 20)
        {
            this.x = ScreenUtility.TopLeft.x + x;
            this.y = ScreenUtility.TopLeft.y - y;
            this.vx = vx;
            this.vy = vy;
            trail = new Deque<(float, float)>(Missiles.TRAIL_LENGTH, maxLength : true);
            t = Random.Range(0, 4); // max is not included

            sameLineRenderer = Missiles.instance.draw.CreateLine(Missiles.FLARE_COLOR);
            sameCircle = Missiles.instance.draw.CreateFilledCircle(Missiles.FLARE_COLOR);

            missileGameObject = new GameObject($"Missile_{x}");

            sameLineRenderer.transform.SetParent(missileGameObject.transform);
            sameCircle.transform.SetParent(missileGameObject.transform);
        }

        public void Step(float dt)
        {
            if (missileGameObject)
            {
                t += dt;
                var uy = vy;
                vy -= Missiles.GRAVITY * dt;
                y -= 0.5f * (uy + vy) * dt;

                x += vx * dt;
                trail.AddToFront((x, y)); // append to left...

                if (trail.Last().Item2 < ScreenUtility.BottomRight.y)
                {
                    Remove();
                    return;
                }
            }
        }

        public void Draw()
        {
            if (missileGameObject)
            {
                foreach (var i in Enumerable.Range(0, trail.Count))
                {
                    if (i + 1 == trail.Count)
                    {
                        break;
                    }
                    var start = trail[i];
                    var end = trail[i + 1];

                    var c = Missiles.TRAIL_BRIGHTNESS * (1.0f - i / (Missiles.TRAIL_LENGTH * 1f));
                    var color = ((byte)c, (byte)c, (byte)c);

                    LineRenderer lineRenderer = Missiles.instance.draw.CreateLine(color, i);
                    lineRenderer.transform.SetParent(missileGameObject.transform);

                    Missiles.instance.draw.DrawLine(lineRenderer, start, end);
                }

                Missiles.instance.draw.DrawFilledCircle(sameCircle, (x, y), 2);

                // This small flickering lens flare makes it look like the
                // missile's exhaust is very bright.
                float flareLength = 4 + Mathf.Sin(t) * 2 + Mathf.Sin(t * 5) * 1;

                Missiles.instance.draw.DrawLine(sameLineRenderer, (x - flareLength, y), (x + flareLength, y));
            }
        }

        void Remove()
        {
            GameObject.Destroy(missileGameObject);
        }
    }
}
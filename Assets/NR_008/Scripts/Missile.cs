using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nito.Collections;
using System.Linq;

namespace NR_008
{
    public class Missile : MonoBehaviour
    {


        float x, y, vx, vy;
        Deque<(float, float)> trail;
        float t;

        public Missile(float x, float vx, float y = 0, float vy = 20)
        {
            this.x = ScreenUtility.TopLeft.x + x;
            this.y = ScreenUtility.TopLeft.y - y;
            this.vx = vx;
            this.vy = vy;
            trail = new Deque<(float, float)>(Missiles.TRAIL_LENGTH);
            t = Random.Range(0, 4); // max is not included
        }

        public void Step(float dt)
        {
            t += dt;
            var uy = vy;
            vy -= Missiles.GRAVITY * dt;
            y -= 0.5f * (uy + vy) * dt;

            x += vx * dt;
            trail.AddToFront((x, y)); // append to left...
            
  
            //if(trail)
        }

        public void Draw()
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
                Missiles.instance.DrawLine(start, end, color);
            }

            Missiles.instance.DrawFilledCircle((x, y), 2, Missiles.FLARE_COLOR, false);

            // This small flickering lens flare makes it look like the
            // missile's exhaust is very bright.
            float flareLength = 4 + Mathf.Sin(t) * 2 + Mathf.Sin(t * 5) * 1;            
            
           Missiles.instance.DrawLine((x - flareLength, y), (x + flareLength, y),
                Missiles.FLARE_COLOR, sameLine: true);                
        }

        void Remove()
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_008
{
    public class Missiles : MonoBehaviour
    {
        // Unity stuff
        public GameObject circlePrefab;
        public GameObject linePrefab;
        GameObject circle;
        SpriteRenderer circleRenderer;

        GameObject line;
        LineRenderer lineRenderer;

        public static Missiles instance;
        public static int GRAVITY = -5;
        public static int TRAIL_LENGTH = 400;
        public static int TRAIL_BRIGHTNESS = 100;
        public static (byte, byte, byte) FLARE_COLOR = (255, 220, 160);

        List<Missile> missiles = new List<Missile>();

        // Start is called before the first frame update
        void Start()
        {
            instance = this;

            circle = Instantiate(circlePrefab);
            circle.name = "MissileCircle";
            circleRenderer = circle.GetComponent<SpriteRenderer>();

            line = Instantiate(linePrefab);
            line.name = "MissileLine";
            lineRenderer = line.GetComponent<LineRenderer>();

             InvokeRepeating("NewMissile", 0, 5);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var m in missiles)
            {
                m.Step(Time.deltaTime);
                m.Draw();
            }
        }

        int il = 0;

        public void DrawLine((float, float) xy, (float, float) endxy, (byte, byte, byte) color, bool sameLine = false)
        {
            GameObject currentLine = null;
            LineRenderer currentLineRenderer = null;
            if (!sameLine)
            {
                currentLine = Instantiate(linePrefab);
                currentLineRenderer = currentLine.GetComponent<LineRenderer>();
            }

            var lR = currentLineRenderer != null ? currentLineRenderer : lineRenderer;

            lR.sortingOrder = il++;

            float startX = xy.Item1;
            float startY = xy.Item2;
            float endX = endxy.Item1;
            float endY = endxy.Item2;

            lR.material.color =
                new Color32(color.Item1, color.Item2, color.Item3, 255);
            lR.SetPosition(0, new Vector3(startX, startY, 1));
            lR.SetPosition(1, new Vector3(endX, endY, 1));

        }


        public void DrawFilledCircle((float, float) xy, int radius, (byte, byte, byte) color, bool sameCircle = false)
        {
            GameObject currentCircle = null;
            SpriteRenderer currentSpriteRenderer = null;
            if (!sameCircle)
            {
                currentCircle = Instantiate(circlePrefab);
                currentSpriteRenderer = currentCircle.GetComponent<SpriteRenderer>();
            }

            var cL = currentCircle != null ? currentCircle : circle;
            var sR = currentSpriteRenderer != null ? currentSpriteRenderer : circleRenderer;

            cL.transform.position = new Vector3(xy.Item1, xy.Item2, 1);
            cL.transform.localScale = new Vector3(radius, radius, 1);
            sR.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
        }

        void NewMissile()
        {
            var m = new Missile(x: Random.Range(600, 801), vx: Random.Range(-70, -11));
            missiles.Add(m);
        }
    }
}
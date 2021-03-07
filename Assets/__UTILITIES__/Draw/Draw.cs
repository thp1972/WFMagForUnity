using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UTILITY
{
    public class Draw : MonoBehaviour
    {
        public GameObject circlePrefab;
        public GameObject linePrefab;

        public LineRenderer CreateLine((byte, byte, byte) color, int layer = 0)
        {
            GameObject currentLine = Instantiate(linePrefab); ;
            LineRenderer currentLineRenderer = currentLine.GetComponent<LineRenderer>();
            currentLineRenderer.material.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
            currentLineRenderer.sortingOrder = layer;
            return currentLineRenderer;
        }

        public GameObject CreateFilledCircle((byte, byte, byte) color)
        {
            GameObject currentCircle = Instantiate(circlePrefab);
            SpriteRenderer currentSpriteRenderer = currentCircle.GetComponent<SpriteRenderer>();
            currentSpriteRenderer.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
            return currentCircle;
        }

        public void DrawLine(LineRenderer lineRenderer, (float, float) xy, (float, float) endxy)
        {
            float startX = xy.Item1;
            float startY = xy.Item2;
            float endX = endxy.Item1;
            float endY = endxy.Item2;

            lineRenderer.SetPosition(0, new Vector3(startX, startY, 1));
            lineRenderer.SetPosition(1, new Vector3(endX, endY, 1));
        }
        public void DrawFilledCircle(GameObject circle, (float, float) xy, int radius)
        {
            circle.transform.position = new Vector3(xy.Item1, xy.Item2, 1);
            circle.transform.localScale = new Vector3(radius, radius, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UTILITY
{
    public class Draw : MonoBehaviour
    {
        public GameObject circlePrefab;
        public GameObject linePrefab;
        public GameObject squarePrefab;
        public GameObject textPrefab;

        #region SHAPE CREATIONS
        public LineRenderer CreateLine((byte, byte, byte) color, int layer = 0)
        {
            GameObject currentLine = Instantiate(linePrefab); ;
            LineRenderer currentLineRenderer = currentLine.GetComponent<LineRenderer>();
            currentLineRenderer.material.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
            currentLineRenderer.sortingOrder = layer;
            return currentLineRenderer;
        }

        public GameObject CreateFilledCircle((byte, byte, byte) color, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            GameObject currentCircle = Instantiate(circlePrefab);
            SpriteRenderer currentSpriteRenderer = currentCircle.GetComponent<SpriteRenderer>();
            currentSpriteRenderer.color = new Color32(color.Item1, color.Item2, color.Item3, 255);

            currentSpriteRenderer.sprite = Sprite.Create(currentSpriteRenderer.sprite.texture,
                                                     currentSpriteRenderer.sprite.rect,
                                                     new Vector2(pivotX, pivotY),
                                                     currentSpriteRenderer.sprite.pixelsPerUnit
                                                     );
            return currentCircle;
        }

        public GameObject CreateFilledRect((byte, byte, byte) color, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            GameObject currentRect = Instantiate(squarePrefab);
            SpriteRenderer currentSpriteRenderer = currentRect.GetComponent<SpriteRenderer>();
            currentSpriteRenderer.color = new Color32(color.Item1, color.Item2, color.Item3, 255);

            currentSpriteRenderer.sprite = Sprite.Create(currentSpriteRenderer.sprite.texture,
                                                         currentSpriteRenderer.sprite.rect,
                                                         new Vector2(pivotX, pivotY),
                                                         currentSpriteRenderer.sprite.pixelsPerUnit
                                                         );
            return currentRect;
        }
        #endregion

        #region SHAPE DRAWING
        public void DrawLine(LineRenderer lineRenderer, (float, float) xy, (float, float) endxy)
        {
            float startX = xy.Item1;
            float startY = xy.Item2;
            float endX = endxy.Item1;
            float endY = endxy.Item2;

            lineRenderer.SetPosition(0, new Vector3(startX, startY, 1));
            lineRenderer.SetPosition(1, new Vector3(endX, endY, 1));
        }
        public void DrawLine(LineRenderer lineRenderer, (byte, byte, byte) color, (float, float) xy, (float, float) endxy)
        {
            lineRenderer.material.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
            DrawLine(lineRenderer, xy, endxy);
        }

        public void DrawFilledCircle(GameObject circle, (float, float) xy, int radius)
        {
            circle.transform.position = new Vector3(xy.Item1, xy.Item2, 1);
            circle.transform.localScale = new Vector3(radius, radius, 1);
        }

        public void DrawFilledRect(GameObject rect, (float, float) xy, (float, float) size)
        {
            rect.transform.position = new Vector3(xy.Item1, xy.Item2, 1);
            rect.transform.localScale = new Vector3(size.Item1, size.Item2, 1);
        }
        #endregion

        #region TEXT CREATION
        
        // if text is empty then use DrawText to draw dynamic text else text is static
        public Text CreateText(int fontSize, (byte, byte, byte) color, string text = "")
        {
            // find the Canvas
            var canvas = GameObject.Find("Canvas");
            if (!canvas)
            {
                Debug.LogError("No canvas found! Text creation failed.");
                return null;
            }

            RectTransform rectTransform = Instantiate(textPrefab).GetComponent<RectTransform>();
            rectTransform.SetParent(canvas.transform);

            var textComponent = rectTransform.GetComponent<Text>();
            textComponent.text = text;
            textComponent.color = new Color32(color.Item1, color.Item2, color.Item3, 255);
            textComponent.fontSize = fontSize;
            textComponent.alignByGeometry = true;
            return textComponent;
        }
        #endregion

        #region TEXT DRAWING

        // if text is empty then the text is static, initialized on CreateText
        public void DrawTex(Text textComponent, (float, float) midBottom, string text = "")
        {
            var pos = midBottom;
            textComponent.rectTransform.anchorMin = new Vector2(0.5f, 0);
            textComponent.rectTransform.anchorMax = new Vector2(0.5f, 0);
            textComponent.rectTransform.pivot = new Vector2(0.5f, 0);
            textComponent.rectTransform.position = Camera.main.WorldToScreenPoint(ScreenUtility.Position(pos.Item1, pos.Item2));

            if(text.Length > 0)
                textComponent.text = text;
        }

        #endregion
    }
}

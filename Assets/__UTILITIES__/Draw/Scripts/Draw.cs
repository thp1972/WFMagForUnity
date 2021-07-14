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

        #region TEXT DEFINITIONS
        // FOR TOP
        Vector2 topLeftAnchorMin = new Vector2(0, 1);
        Vector2 topLeftAnchorMax = new Vector2(0, 1);
        Vector2 topLeftPivot = new Vector2(0, 1);
        Vector2 topRightAnchorMin = new Vector2(1, 1);
        Vector2 topRightAnchorMax = new Vector2(1, 1);
        Vector2 topRightPivot = new Vector2(1, 1);
        // in Unity TOP,CENTER
        Vector2 midTopAnchorMin = new Vector2(.5f, 1);
        Vector2 midTopAnchorMax = new Vector2(.5f, 1);
        Vector2 midTopPivot = new Vector2(.5f, 1);
        //----------------------------------------------------

        // FOR CENTER
        Vector2 midLeftAnchorMin = new Vector2(0, 0.5f);
        Vector2 midLeftAnchorMax = new Vector2(0, 0.5f);
        Vector2 midLeftPivot = new Vector2(0, 0.5f);
        Vector2 midRightAnchorMin = new Vector2(1, 0.5f);
        Vector2 midRightAnchorMax = new Vector2(1, 0.5f);
        Vector2 midRightPivot = new Vector2(1, 0.5f);
        // in Unity MIDDLE,CENTER
        Vector2 centerAnchorMin = new Vector2(.5f, 0.5f);
        Vector2 centerAnchorMax = new Vector2(.5f, 0.5f);
        Vector2 centerPivot = new Vector2(.5f, 0.5f);
        //---------------------------------------------------

        // FOR BOTTOM
        Vector2 bottomLeftAnchorMin = new Vector2(0, 0);
        Vector2 bottomLeftAnchorMax = new Vector2(0, 0);
        Vector2 bottomLeftPivot = new Vector2(0, 0);
        Vector2 bottomRightAnchorMin = new Vector2(1, 0);
        Vector2 bottomRightAnchorMax = new Vector2(1, 0);
        Vector2 bottomRightPivot = new Vector2(1, 0);
        // in Unity BOTTOM,CENTER
        Vector2 midBottomAnchorMin = new Vector2(.5f, 0);
        Vector2 midBottomAnchorMax = new Vector2(.5f, 0);
        Vector2 midBottomPivot = new Vector2(.5f, 0);
        #endregion

        #region TEXT CREATION

        // if text is empty then use DrawText to draw dynamic text else text is static
        public Text CreateText(int fontSize, (byte, byte, byte) color, string text = "", bool isActive = true)
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
            textComponent.gameObject.SetActive(isActive);
            return textComponent;
        }
        #endregion

        #region TEXT DRAWING

        // if text is empty then the text is static, initialized on CreateText   
        public void DrawTex(Text textComponent,
            (float, float) topLeft = default,
            (float, float) topRight = default,
            (float, float) midTop = default,
            (float, float) bottomLeft = default,
            (float, float) bottomRight = default,
            (float, float) midBottom = default,
            (float, float) midLeft = default,
            (float, float) center = default,
            (float, float) midRight = default,
            string text = "", bool isActive = true)
        {
            (float, float) pos = default;
            if (topLeft != default)
            {
                SetAnchorAndPivot(topLeftAnchorMin, topLeftAnchorMax, topLeftPivot);
                pos = topLeft;
            }
            else if (topRight != default)
            {
                SetAnchorAndPivot(topRightAnchorMin, topRightAnchorMax, topRightPivot);
                pos = topRight;
            }
            else if (midTop != default)
            {
                SetAnchorAndPivot(midTopAnchorMin, midTopAnchorMax, midTopPivot);
                pos = midTop;
            }
            else if (bottomLeft != default)
            {
                SetAnchorAndPivot(bottomLeftAnchorMin, bottomLeftAnchorMax, bottomLeftPivot);
                pos = bottomLeft;
            }
            else if (bottomRight != default)
            {
                SetAnchorAndPivot(bottomRightAnchorMin, bottomRightAnchorMax, bottomRightPivot);
                pos = bottomRight;
            }
            else if (midBottom != default)
            {
                SetAnchorAndPivot(midBottomAnchorMin, midBottomAnchorMax, midBottomPivot);
                pos = midBottom;
            }
            else if (midLeft != default)
            {
                SetAnchorAndPivot(midLeftAnchorMin, midLeftAnchorMax, midLeftPivot);
                pos = midLeft;
            }
            else if (center != default)
            {
                SetAnchorAndPivot(centerAnchorMin, centerAnchorMax, centerPivot);
                pos = center;
            }
            else if (midRight != default)
            {
                SetAnchorAndPivot(midRightAnchorMin, midRightAnchorMax, midRightPivot);
                pos = midRight;
            }

            textComponent.rectTransform.position = Camera.main.WorldToScreenPoint(ScreenUtility.Position(pos.Item1, pos.Item2));

            if (text.Length > 0)
                textComponent.text = text;

            textComponent.gameObject.SetActive(isActive);

            void SetAnchorAndPivot(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
            {
                textComponent.rectTransform.anchorMin = anchorMin;
                textComponent.rectTransform.anchorMax = anchorMax;
                textComponent.rectTransform.pivot = pivot;
            }
        }

        #endregion
    }
}

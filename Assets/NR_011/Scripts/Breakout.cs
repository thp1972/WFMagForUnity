using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UTILITY;

namespace NR_011
{
    abstract class Sprite
    {
        public Vector2 position;
        public Vector2 size;

        private (float, float) vel;

        public (float, float) Vel
        {
            get { return vel; }
            set { vel = value; }
        }

        private float top;

        public float Top
        {
            get
            {
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                var y = rect.yMin;
                return y;
            }
            set
            {
                top = value;
                sprite.transform.position = ScreenUtility.Position(position.x, top);
            }
        }

        private float left;

        public float Left
        {
            get
            {
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                var x = rect.xMin;
                return x;
            }
            set
            {
                left = value;
                sprite.transform.position = ScreenUtility.Position(left, position.y);
            }
        }

        private float right;

        public float Right
        {
            get
            {
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                var x = rect.xMax;
                return x;
            }
            set
            {
                right = value;
                sprite.transform.position = ScreenUtility.Position(right - size.x, position.y);
            }
        }

        private (float, float) center;

        public (float, float) Center
        {
            get
            {
                center = (CenterX, CenterY);
                return center;
            }
            set
            {
                center = value;

                CenterX = center.Item1;
                CenterY = center.Item2;
            }
        }

        private float centerX;

        public float CenterX
        {
            get
            {
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                return rect.center.x;
            }
            set
            {
                centerX = value;
                position = new Vector2(centerX, position.y);
                var sr = sprite.GetComponent<SpriteRenderer>();

                // if sprite has a pivot not already centerd, then manually center
                if (sr.sprite.pivot.normalized.x != 0.5f && sr.sprite.pivot.normalized.y != 0.5f)
                {
                    position.x = position.x - size.x / 2;
                }
                sprite.transform.position = ScreenUtility.Position(position.x, position.y);
            }
        }

        private float centerY;

        public float CenterY
        {
            get
            {
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                return rect.center.y;
            }
            set
            {
                centerY = value;
                position = new Vector2(position.x, centerY);
                var sr = sprite.GetComponent<SpriteRenderer>();

                // if sprite has a pivot not already centerd, then manually center
                if (sr.sprite.pivot.normalized.x != 0.5f && sr.sprite.pivot.normalized.y != 0.5f)
                {
                    position.y = position.y - size.y / 2;
                }
                sprite.transform.position = ScreenUtility.Position(position.x, position.y);
            }
        }

        public (float, float) BottomLeft
        {
            get
            {
                var bL = (0f, 0f);
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                bL.Item1 = rect.xMin;
                bL.Item2 = rect.yMax;
                return bL;
            }
        }

        public (float, float) BottomRight
        {
            get
            {
                var bR = (0f, 0f);
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                bR.Item1 = rect.xMax;
                bR.Item2 = rect.yMax;
                return bR;
            }
        }

        public (float, float) TopLeft
        {
            get
            {
                var tL = (0f, 0f);
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                tL.Item1 = rect.xMin;
                tL.Item2 = rect.yMin;
                return tL;
            }
        }

        public (float, float) TopRight
        {
            get
            {
                var tR = (0f, 0f);
                var sr = sprite.GetComponent<SpriteRenderer>();
                Rect rect = new Rect(position, size);
                tR.Item1 = rect.xMax;
                tR.Item2 = rect.yMin;
                return tR;
            }
        }

        protected Color32 color;

        public Color32 Color
        {
            get { return color; }
            set
            {
                color = value;
                sprite.GetComponent<SpriteRenderer>().color = color;
            }
        }

        public GameObject sprite;
        protected Draw draw;

        public Sprite(float xPos, float yPos, float xSize, float ySize)
        {
            position = new Vector2(xPos, yPos);
            size = new Vector2(xSize, ySize);
            draw = GameObject.Find("Breakout").GetComponent<Draw>();
        }

        public abstract void Draw(UnityEngine.Color32 color);
        public abstract void Draw();

        public bool CollideRect(Sprite other)
        {
            bool collision = false;

            Collider2D[] hitColliders =
                Physics2D.OverlapBoxAll(sprite.transform.position, sprite.transform.localScale, 0);

            if (hitColliders.Length > 0)
            {
                foreach (var hc in hitColliders)
                {
                    if (hc.gameObject == other.sprite)
                        collision = true;
                }
            }

            return collision;
        }

        public int CollideList(List<Brick> bricks) // here better covariance generic?
        {
            int ix = -1;
            foreach (var b in bricks)
            {
                if (CollideRect(b))
                {
                    ix = bricks.IndexOf(b);
                    break;
                }
            }

            return ix;
        }
    }

    class Ball : Sprite
    {
        public Ball(float xPos, float yPos, float xSize, float ySize) : base(xPos, yPos, xSize, ySize)
        {
            sprite = draw.CreateFilledCircle((255, 255, 255), 0, 1);
            sprite.AddComponent<BoxCollider2D>();
            sprite.SetActive(false);
        }

        public override void Draw(UnityEngine.Color32 color)
        {
            Color = color;
            Vector2 posS = ScreenUtility.Position(position.x, position.y);
            draw.DrawFilledCircle(sprite, (posS.x, posS.y), (int)size.x);
            sprite.SetActive(true);
        }

        public override void Draw()
        {
            Draw(UnityEngine.Color.white);
        }
    }

    class Bat : Sprite
    {
        public Bat(float xPos, float yPos, float xSize, float ySize) : base(xPos, yPos, xSize, ySize)
        {
            sprite = draw.CreateFilledRect((255, 255, 255), 0, 1);
            sprite.transform.position = position;
            sprite.AddComponent<BoxCollider2D>();
            sprite.SetActive(false);
        }

        public override void Draw(UnityEngine.Color32 color)
        {
            Color = color;
            Vector2 posS = ScreenUtility.Position(position.x, position.y);
            draw.DrawFilledRect(sprite, (posS.x, posS.y), (size.x, size.y));
            sprite.SetActive(true);
        }

        public override void Draw()
        {
            Draw(UnityEngine.Color.white);
        }
    }

    class Brick : Sprite
    {
        public Color highlight;

        public Brick(float xPos, float yPos, float xSize, float ySize) : base(xPos, yPos, xSize, ySize)
        {
            sprite = draw.CreateFilledRect((255, 255, 255), 0, 1);

            // here the parent transform is necessary because we have to delete all bricks at once
            sprite.gameObject.transform.SetParent(GameObject.Find("Bricks").transform);

            sprite.AddComponent<BoxCollider2D>();
            sprite.SetActive(false);
        }

        public override void Draw(UnityEngine.Color32 color)
        {
            Color = color;
            Vector2 posS = ScreenUtility.Position(position.x, position.y);
            draw.DrawFilledRect(sprite, (posS.x, posS.y), (size.x, size.y));
            sprite.SetActive(true);
        }

        public override void Draw()
        {
            Draw(UnityEngine.Color.white);
        }
    }

    class Line : Sprite
    {
        public Vector2 endPosition;

        public new Color32 Color
        {
            get { return color; }
            set
            {
                color = value;
                sprite.GetComponent<LineRenderer>().material.color = color;
            }
        }

        public Line(float xPos, float yPos, float xPosEnd, float yPosEnd, float xSize, float ySize) : base(xPos, yPos,
            xSize, ySize)
        {
            endPosition = new Vector2(xPosEnd, yPosEnd);
            sprite = draw.CreateLine((255, 255, 255)).gameObject;

            // here the parent transform is necessary because we have to delete all lines at once
            sprite.gameObject.transform.SetParent(GameObject.Find("Lines").transform);

            sprite.SetActive(false);
        }

        public override void Draw(UnityEngine.Color32 color)
        {
            Color = color;
            Vector2 posS = ScreenUtility.Position(position.x, position.y);
            Vector2 posE = ScreenUtility.Position(endPosition.x, endPosition.y);
            draw.DrawLine(sprite.GetComponent<LineRenderer>(), (posS.x, posS.y), (posE.x, posE.y));
            sprite.SetActive(true);
        }

        public override void Draw()
        {
            Draw(UnityEngine.Color.white);
        }
    }

    public class Breakout : MonoBehaviour
    {
        int WIDTH = 600;
        int HEIGHT = 800;
        int BALL_SIZE = 10;
        int MARGIN = 50;

        int BRICKS_X = 10;
        int BRICKS_Y = 5;
        int BRICK_W;
        int BRICK_H = 25;

        Ball ball;
        Bat bat;

        List<Brick> bricks;
        Dictionary<Brick, List<Line>> lines;

        // Start is called before the first frame update
        void Start()
        {
            BRICK_W = (WIDTH - 2 * MARGIN) / BRICKS_X;

            ball = new Ball(WIDTH / 2, HEIGHT / 2, BALL_SIZE, BALL_SIZE);
            bat = new Bat(WIDTH / 2, HEIGHT - 50, 80, 12);

            bricks = new List<Brick>();
            lines = new Dictionary<Brick, List<Line>>();

            Reset();
        }

        private void Update()
        {
            // When you have fast moving objects, like the ball, a good trick
            // is to run the update step several times per frame with tiny time steps.
            // This makes it more likely that collisions will be handled correctly.
            foreach (int _ in Enumerable.Range(0, 3))
                UpdateStep(1 / 360f); // 1/180 runs on Unity too fast!!!
        }

        // Update is called once per frame
        void UpdateStep(float dt)
        {
            Draw();

            var x = ball.Center.Item1;
            var y = ball.Center.Item2;
            var vx = ball.Vel.Item1;
            var vy = ball.Vel.Item2;

            if (ball.Top > HEIGHT)
            {
                Reset();
                return;
            }

            // Update ball based on previous velocity
            x += vx * dt;
            y += vy * dt;

            ball.Center = (x, y);

            // Check for and resolve collisions
            if (ball.Left < 0)
            {
                vx = Mathf.Abs(vx);
                ball.Left = -ball.Left;
            }
            else if (ball.Right > WIDTH)
            {
                vx = -Mathf.Abs(vx);
                ball.Right -= 2 * (ball.Right - WIDTH);
            }

            if (ball.Top < 0)
            {
                vy = Mathf.Abs(vy);
                ball.Top *= -1;
            }

            if (ball.CollideRect(bat))
            {
                vy = -Mathf.Abs(vy);
                // randomise the x velocity but keep the sign
                vx = Random.Range(50f, 300f) * Mathf.Sign(vx);
            }
            else
            {
                // Find first collision
                var idx = ball.CollideList(bricks);
                if (idx != -1)
                {
                    var brick = bricks[idx];
                    // Work out what side we collided on
                    var dx = (ball.CenterX - brick.CenterX) / BRICK_W;
                    var dy = (ball.CenterY - brick.CenterY) / BRICK_H;

                    if (Mathf.Abs(dx) > Mathf.Abs(dy))
                        vx = Mathf.Abs(vx) * Mathf.Sign(dx);
                    else
                        vy = Mathf.Abs(vy) * Mathf.Sign(dy);

                    brick.sprite.SetActive(false);

                    lines[brick][0].sprite.gameObject.SetActive(false);
                    lines[brick][1].sprite.gameObject.SetActive(false);

                    lines.Remove(brick);
                    bricks.Remove(brick);
                }
            }

            // Write back updated position and velocity
            ball.Center = (x, y);
            ball.Vel = (vx, vy);

            OnMouseMove(Input.mousePosition);
        }

        private void Draw()
        {
            foreach (Brick brick in bricks)
            {
                brick.Draw(brick.Color);

              //  lines[brick][0].Draw(brick.highlight); // brick.bottomleft, brick.topleft
               // lines[brick][1].Draw(brick.highlight); // brick.topleft, brick.topright
            }

            bat.Draw(new Color32(255, 192, 203, 255)); // pink as from Pygame doc
            ball.Draw();
        }

        private void OnMouseMove(Vector3 pos)
        {
            float x = pos.x, y = pos.y;
            bat.CenterX = x;
            if (bat.Left < 50)
                bat.Left = 0;
            else if (bat.Right > WIDTH)
                bat.Right = WIDTH;
        }

        private void Reset()
        {
            // Reset bricks and ball.
            // First, let's do bricks
            DeleteBricks();
            foreach (float x in Enumerable.Range(0, BRICKS_X))
            {
                foreach (float y in Enumerable.Range(0, BRICKS_Y))
                {
                    var brick = new Brick(x * BRICK_W + MARGIN, y * BRICK_H + MARGIN, BRICK_W - 1, BRICK_H - 1);

                    var hue = (x + y) / BRICKS_X;
                    var saturation = (y / BRICKS_Y) * 0.5f + 0.5f;

                    brick.highlight = HsvColor(hue, saturation * 0.7f, 1);
                    brick.Color = HsvColor(hue, saturation, 0.8f);

                    bricks.Add(brick);

                    Line l1 = new Line(brick.BottomLeft.Item1, brick.BottomLeft.Item2, brick.TopLeft.Item1,
                        brick.TopLeft.Item2, 0, 0);
                    Line l2 = new Line(brick.TopLeft.Item1, brick.TopLeft.Item2, brick.TopRight.Item1,
                        brick.TopRight.Item2, 0, 0);
                    lines.Add(brick, new List<Line> { l1, l2 });
                }
            }

            ball.Center = (WIDTH / 2, HEIGHT / 2);
            ball.Vel = (Random.Range(-200, 200), 400);
        }

        private Color32 HsvColor(float h, float s, float v)
        {
            // Return an RGB color from HSV

            // in Python colorsys.hsv_to_rgb(h, s, v) if h > 1 it accepts and converts but not in Unity 
            // if h > 1 colors are all 0 (RGBA 0,0,0) so we make a little adjustment to accept values > 1
            if (h > 1)
            {
                h = (360 * h - 360) / 360;
            }

            Color rgb = Color.HSVToRGB(h, s, v);
            return new Color32((byte)(rgb.r * 255), (byte)(rgb.g * 255), (byte)(rgb.b * 255), 255);
        }

        private void DeleteBricks()
        {
            bricks.Clear();
            lines.Clear();

            foreach (Transform t in GameObject.Find("Bricks").GetComponentInChildren<Transform>())
            {
                Destroy(t.gameObject);
            }

            foreach (Transform t in GameObject.Find("Lines").GetComponentInChildren<Transform>())
            {
                Destroy(t.gameObject);
            }
        }
    }
}
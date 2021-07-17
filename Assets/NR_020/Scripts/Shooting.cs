using PygameZero;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_020
{
    class CrossHair : Actor
    {
        public CrossHair(string image) : base(image, (0, 0)) { }
    }

    class Enemy : Actor
    {
        public bool hit;
        public float timer;
        public List<Hit> hits;

        public Enemy(string image, (float, float) pos) : base(image, pos) { }
    }

    class Hit : Actor
    {
        public Hit(string image, Vector2 pos) : base(image, pos) { }
    }

    public class Shooting : MonoBehaviour
    {
        public GameObject bullet;
        GameObject blitBullet;
        int WIDTH = 800, HEIGHT = 800;
        CrossHair crossHair;
        List<Enemy> enemies = new List<Enemy>();

        int numberofbullets = 8;
        int MAXBULLETS = 8;

        private void Awake()
        {
            // change bullet pivot that must be drawn by blit to left, top cause Pygame Blit draws, by default, left,top
            // but Pygame Actor draws, by default, center
            blitBullet = Instantiate(bullet);
            blitBullet.SetActive(false);
            var su = new SpriteUtility(blitBullet.GetComponent<SpriteRenderer>());
            su.SetPivot(SpriteUtility.PivotPosition.TOPLEFT);
        }

        // Start is called before the first frame update
        void Start()
        {
            crossHair = new CrossHair("crosshair");

            // create 3 enemies at various positions
            foreach (var p in new List<(int, int)> { (0, 200), (-200, 400), (-400, 600) })
                enemies.Add(NewEnemy(p));

        }
        // Update is called once per frame
        void Update()
        {
            ScreenUtility.BlitClear("BlitBullet");

            OnMouseMove(Input.mousePosition);
            OnMouseDown(Input.mousePosition);

            Draw();

            foreach (var e in enemies.ToArray())
            {
                // hit enemies continue to display
                // until timer reaches 0
                if (e.hit)
                {
                    e.timer -= 1;
                    if (e.timer <= 0)
                    {
                        enemies.Remove(e);
                        RemoveEnemyAndBullets(e);// Unity stuff
                    }
                }
                else
                    e.X = Mathf.Min(e.X + 2, WIDTH);
            }
        }

        void Draw()
        {
            // draw enemies 
            foreach (var e in enemies)
            {
                e.Draw();
                // draw enemy hits 
                foreach (var h in e.hits)
                    h.Draw();
            }

            crossHair.Draw();

            // draw remaining bullets
            foreach (var n in Enumerable.Range(0, numberofbullets))
            {
                ScreenUtility.Blit(blitBullet, (10 + n * 30, 10), "BlitBullet", true, true);
            }
        }

        // creating a new enemy 
        private Enemy NewEnemy((float, float) pos)
        {
            var e = new Enemy("enemy", pos);
            e.hit = false;
            e.timer = 50;
            e.hits = new List<Hit>();
            return e;
        }

        // creating a bullet that has hit an enemy 
        private Hit NewHit(Vector2 pos)
        {
            var h = new Hit("bullet", pos);
            return h;
        }

        private void OnMouseMove(Vector2 pos)
        {
            var x = pos.x;
            // pixel coords in Unity starts from bottom,left (0,0)
            var y = ScreenUtility.GameResolution.y - pos.y;
            crossHair.Pos = (x, y);
        }

        private void OnMouseDown(Vector2 pos)
        {
            pos = ScreenUtility.InvertYOnPixelPosition(pos);

            // left to fire 
            if (EventsDetector.Mouse.Left && numberofbullets > 0)
            {
                //NewHit(pos);
                // check whether an enemy has been hit
                foreach (var e in enemies)
                {
                    if (crossHair.CollideRect(e))
                    {
                        // if hit, add position to 'hits' list
                        e.hits.Add(NewHit(pos));
                        e.hit = true;
                        break;
                    }
                }

                numberofbullets = Mathf.Max(0, numberofbullets - 1);
            }

            if (EventsDetector.Mouse.Right)
                numberofbullets = MAXBULLETS;
        }

        private void RemoveEnemyAndBullets(Enemy e)
        {
            foreach (var h in e.hits.ToArray())
            {
                e.hits.Remove(h);
                h.Destroy();
            }
            e.Destroy();
        }
    }
}
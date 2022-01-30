using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;
using System.Linq;

namespace NR_024
{
    class Barrel : Actor
    {
        public float frame;

        public Barrel(string image, (float, float) pos, SpriteUtility.PivotPosition anchor) :
            base(image, pos, anchor)
        {
            SortingOrder = 2;
        }
    }

    public class DonkeyKong : MonoBehaviour
    {
        public GameObject backgroundPrefab;
        GameObject background;

        List<Barrel> barrels = new List<Barrel>();
        Image platformMap;
        int spacer = 0;

        void Start()
        {
            background = Instantiate(backgroundPrefab);

            platformMap = new Image();
            platformMap.Load("map");
        }

        private void Draw()
        {
            ScreenUtility.Blit(background, (0, 0));

            foreach (var b in Enumerable.Range(0, barrels.Count))
            {
                if (OnScreen((int)barrels[b].X, (int)barrels[b].Y))
                    barrels[b].Draw();
                else
                    barrels[b].Destroy();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (MathUtility.RandInt(0, 100) == 1 && spacer < 0)
            {
                MakeBarrel();
                spacer = 100;
            }

            spacer -= 1;

            foreach (var b in Enumerable.Range(0, barrels.Count))
            {
                var x = (int)(barrels[b].X);
                var y = (int)(barrels[b].Y);
                if (OnScreen(x, y))
                {
                    var testcol1 = TestPlatform(x - 16, y + 16, 0);
                    var testcol2 = TestPlatform(x, y + 16, 0);
                    var testcol3 = TestPlatform(x + 16, y + 16, 0);
                    var move = 0;

                    if (testcol1 > testcol3) move = 1;
                    if (testcol3 > testcol1) move = -1;
                    barrels[b].X += move;

                    if (move != 0) barrels[b].frame += move * 0.1f;
                    else barrels[b].frame += 0.1f;

                    if (barrels[b].frame >= 4) barrels[b].frame = 1;
                    if (barrels[b].frame < 1) barrels[b].frame = 3.9f;

                    var testladder = platformMap.GetAt(x, y + 32);
                    if (testladder[2] == 255)
                    {
                        if (MathUtility.RandInt(0, 150) == 1)
                            barrels[b].Y += 20;
                    }

                    if (testcol2 == 0) barrels[b].Y += 1;

                    var frame = Mathf.Floor(barrels[b].frame).ToString();
                    if (TestPlatform(x, y + 16, 2) > 0)
                        barrels[b].Image = "bfrfront" + frame;
                    else
                        barrels[b].Image = "bfrside" + frame;
                }
            }

            Draw();
        }

        bool OnScreen(int x, int y)
        {
            return Enumerable.Range(16, 784 - 16).Contains(x) && Enumerable.Range(16, 584 - 16).Contains(y);
        }

        void MakeBarrel()
        {
            barrels.Add(new Barrel("bfrfront1", (200, 30), SpriteUtility.PivotPosition.CENTER));
            barrels[barrels.Count - 1].frame = 1;
        }

        int TestPlatform(int x, int y, int col)
        {
            int c = 0;
            foreach (var z in Enumerable.Range(0, 3))
            {
                var rgb = platformMap.GetAt(x, y + z);
                c += rgb[col];
            }
            return c;
        }
    }
}
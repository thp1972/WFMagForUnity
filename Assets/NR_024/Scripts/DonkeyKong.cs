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

        // Start is called before the first frame update
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
                {
                    barrels[b].Draw();
                }
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
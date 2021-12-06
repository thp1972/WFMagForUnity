using PygameZero;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_017
{
    public class Lemmings : MonoBehaviour
    {
        class Lemming : Actor
        {
            int direction = 1;
            int climbHeight = 4;
            int width = 10;
            int height = 20;

            public Lemming() : base("lemming", Lemmings.startPosition, SpriteUtility.PivotPosition.TOPLEFT) { }

            //  update a lemming's position in the level
            public void Update()
            {
                // if there's no ground below a lemming (check both corners), it is falling
                var bottomLeft = Lemmings.GroundAtPosition((pos.x, pos.y + height));
                var bottomRight = Lemmings.GroundAtPosition((pos.x + (width - 1), pos.y + height));
                if (!bottomLeft && !bottomRight)
                    Y += 1;
                // if not falling, a lemming is walking
                else
                {
                    int height = 0;
                    bool found = false;
                    // find the height of the ground in front of a lemming
                    // up to the maximum height a lemming can climb
                    (float, float) positionInFront;
                    while (found == false && height <= climbHeight)
                    {
                        //  the pixel 'in front' of a lemming will depend on
                        // the direction it's traveling
                        if (direction == 1)
                            positionInFront = (pos.x + width, pos.y + (this.height - 1) - height);
                        else
                            positionInFront = (pos.x - 1, pos.y + (this.height - 1) - height);

                        if (!Lemmings.GroundAtPosition(positionInFront))
                        {
                            X += direction;
                            // rise up to new ground level
                            Y -= height;
                            found = true;
                        }

                        height += 1;
                    }

                    //  turn the lemming around if the ground in front
                    // is too high to climb
                    if (!found)
                        direction *= -1;                    
                }
            }
        }

        // screen size
        int WIDTH = 800;
        int HEIGHT = 800;

        // level information
        GameObject level_image;
        static Color BACKGROUND_COLOUR = new Color(114f / 255, 114f / 255, 201f / 255, 255 / 255);

        SpriteUtility img;
        static List<List<Color>> pixels;

        // a list to keep track of the lemmings
        List<Lemming> lemmings = new List<Lemming>();
        int maxLemmings = 10;
        public static (float, float) startPosition = (100, 100);

        //  a timer and interval for creating new lemmings
        int timer = 0;
        int interval = 60; // 10 too fast on Unity

        public SpriteRenderer levelImage;

        // Start is called before the first frame update
        void Start()
        {
            // store the colour of each pixel in the level image
            img = new SpriteUtility(Instantiate(levelImage));
            pixels = (from x in Enumerable.Range(0, WIDTH)
                      select (from y in Enumerable.Range(0, HEIGHT)
                                  // (HEIGHT - 1) - y, cause Unity pixel coordinates start 0,0 at lower bottom
                              select img.GetPixelAt(x, (HEIGHT - 1) - y)).ToList()).ToList();

            // here the prefab image is already setted at -400, 400 so it is centered
            // else the GetPixelAt doesn't work
            level_image = Instantiate(levelImage.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            // increment the timer and create a new
            // lemming if the interval has passed
            timer += 1;
            if (timer > interval && lemmings.Count < maxLemmings)
            {
                timer = 0;
                lemmings.Add(new Lemming());
            }

            foreach (var i in lemmings)
                i.Update();

            Draw();
        }

        // returns true if the pixel specified is 'ground'
        // (i.e. anything except BACKGROUND_COLOUR)
        public static bool GroundAtPosition((float, float) pos)
        {
            // ensure position contains integer values
            (int, int) _pos = ((int)pos.Item1, (int)pos.Item2);

            // get the colour from the 'pixels' list
            if (pixels[_pos.Item1][_pos.Item2] != BACKGROUND_COLOUR)
                return true;
            else
                return false;
        }

        private void Draw()
        {
            // draw the level
            ScreenUtility.Blit(level_image, (0, 0));
            // draw lemmings
            foreach (var i in lemmings)
                i.Draw();
        }
    }
}
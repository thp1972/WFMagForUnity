using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PygameZero; // contains Actor class

namespace NR_014
{
    // the Player() class is a subclass of Actor()
    class Player : Actor
    {
        public float animationDelay;
        public float animationTimer;
        public int animationIndex;
        public Dictionary<string, List<string>> images;

        public string state;

        public Player() : base("stand1", new Vector2(200, 100))
        {
            state = "stand";
            animationDelay = 30; // 10 is yoo fast on Unity
            animationTimer = 0;
            // a list of image for each player state
            images = new Dictionary<string, List<string>>
            {
                {
                    "stand", new List<string> {"stand1" }
                },
                {
                    "walkleft", new List<string> { "walkleft1", "walkleft2" }
                },
                {
                    "walkright", new List<string> { "walkright1", "walkright2"}
                }
            };
            // the index of the current image in the image list
            animationIndex = 0;
        }

        public void Update()
        {
            // update position and state based on keyboard input
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                X -= 0.25f; // 1 is too fast on Unity
                state = "walkleft";
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                X += 0.25f; // 1 is too fast on Unity
                state = "walkright";
            }
            else
                state = "stand";

            animationTimer += 1;
            if (animationTimer > animationDelay)
            {
                animationTimer = 0;
                animationIndex += 1;
                if (animationIndex > images[state].Count - 1)
                    animationIndex = 0;
                Image = images[state][animationIndex];
            }

            Draw();
        }
    }

    public class WalkCycle : MonoBehaviour
    {
        Player p;

        // Start is called before the first frame update
        void Start()
        {
            p = new Player();
        }

        // Update is called once per frame
        void Update()
        {
            p.Update();
        }
    }
}
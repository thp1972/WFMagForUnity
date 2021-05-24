using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_014
{
    // the Player() class is a subclass of Actor()
    class Player
    {
        public float animationDelay;
        public float animationTimer;
        public int animationIndex;
        public Dictionary<string, List<string>> images;
        public GameObject image;
        public float Pos
        {
            set
            {

            }
        }

        float x;
        public float X
        {
            get { return x; }
            set
            {

            }
        }

        public string state;

        public Player()
        {
            state = "stand";
            animationDelay = 10;
            animationTimer = 0;
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

            animationIndex = 0;
        }

        public void Update()
        {

        }
    
        public void Draw()
        {

        }
    }



    public class WalkCycle : MonoBehaviour
    {
        public GameObject[] images;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
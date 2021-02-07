using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_003
{
    public class ParallaxScrolling : MonoBehaviour
    {
        struct Layer
        {
            public Transform transform;
            public int speed;
            public SpriteUtility spriteUtility;
        }
        List<Layer> layers = new List<Layer>();

        public Transform layerBack;
        public Transform layerMiddle;
        public Transform layerFront;

        int layerBackSpeed = 1;
        int layerMiddleSpeed = 3;
        int layerFrontSpeed = 5;

        // set screen width and height
        readonly int WIDTH = 800;
        readonly int HEIGHT = 400;

        int startPosition = 400;

        private void Awake()
        {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#endif
        }

        // Start is called before the first frame update
        void Start()
        {
            layers.Add(new Layer() { transform = layerBack, speed = layerBackSpeed, spriteUtility = new SpriteUtility(layerBack.GetComponent<SpriteRenderer>()) });
            layers.Add(new Layer() { transform = layerMiddle, speed = layerMiddleSpeed, spriteUtility = new SpriteUtility(layerMiddle.GetComponent<SpriteRenderer>()) });
            layers.Add(new Layer() { transform = layerFront, speed = layerFrontSpeed, spriteUtility = new SpriteUtility(layerFront.GetComponent<SpriteRenderer>()) });
        }

        // Update is called once per frame
        void Update()
        {
            foreach (var l in layers)
            {
                // move each layer to the left
                l.transform.Translate(-l.speed, 0, 0);

                // if the layer has moved far enough to the left
                // then reset the layers position
                if (l.spriteUtility.GetSpriteSidePosition(SpriteUtility.SpritePosition.RIGHT) <= WIDTH / 2)
                {
                    l.transform.position = new Vector3(startPosition, 0, 0);
                }
            }
        }
    }
}
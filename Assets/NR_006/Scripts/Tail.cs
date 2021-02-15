using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_006
{
    public class Tail : MonoBehaviour
    {
        // Constants that control the wobble effect 
        const int SEGMENT_SIZE = 50;  // pixels from one segment to the next 
        const float ANGLE = 2.5f; // Base direction for the tail (radians) 
        const float PHASE_STEP = 0.3f; // How much the phase differs in each tail piece (radians) 
        const float WOBBLE_AMOUNT = 0.5f; // How much of a wobble there is (radians) 
        const float SPEED = 4.0f;  // How fast the wobble moves (radians per second) 

        // Dimensions of the screen (pixels) 
        const int WIDTH = 800;
        const int HEIGHT = 800;

        // Kepp track of time
        float t; // seconds

        public GameObject tailPiece;
        public GameObject tailHook;

        List<GameObject> tail;

        SpriteUtility spriteUtility;
        float distanceFromRight;
        float distanceFromBottom;

        float angle;

        // Start is called before the first frame update
        void Start()
        {
            // The sprites we’ll use. 
            // 10 tail pieces 
            tail = Enumerable.Range(0, 10).Select((i) => { var t = Instantiate(tailPiece); t.SetActive(false); return t; }).ToList();

            // Plus a hook piece at the end 
            var th = Instantiate(tailHook);
            th.SetActive(false);
            tail.Add(th);

            // in Unity, by default, the pivot is centered
            spriteUtility = new SpriteUtility(tail[0].GetComponent<SpriteRenderer>());
            distanceFromRight = spriteUtility.GetSpriteSidePosition(SpriteUtility.SpritePosition.RIGHT);
            distanceFromBottom = spriteUtility.GetSpriteSidePosition(SpriteUtility.SpritePosition.BOTTOM);

            // First draw the even tail pieces
            foreach (var a in tail.ToList().Where((c, i) => i % 2 == 0).ToList())
            {
                a.SetActive(true);
            }

            // Now draw the odd tail pieces
            foreach (var a in tail.ToList().Where((c, i) => i % 2 != 0).ToList())
            {
                a.SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            t += Time.deltaTime;

            //  Start at the bottom right 
            float x = ScreenUtility.BottomRight.x - distanceFromRight;
            float y = ScreenUtility.BottomRight.y - distanceFromBottom;

            for (int seg = 0, i = 0; i < tail.Count; i++, seg++)
            {
                var _tail = tail[i];
                _tail.transform.position = new Vector2(x, y);

                // Calculate an angle to the next piece which wobbles sinusoidally 
                angle = ANGLE + WOBBLE_AMOUNT * Mathf.Sin(seg * PHASE_STEP + t * SPEED);

                x += SEGMENT_SIZE * Mathf.Cos(angle);
                y += SEGMENT_SIZE * Mathf.Sin(angle);
            }
        }
    }
}
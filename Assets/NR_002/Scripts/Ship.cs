using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_002
{
    public class Ship : MonoBehaviour
    {
        // How many wobbles the ship does while diving 
        readonly int DIVE_WOBBLE_SPEED = 2;

        // How far the ship wobbles while diving 
        readonly int DIVE_WOBBLE_AMOUNT = 100;

        readonly float EPSILON = 0.001f;

        float vx = 100;

        bool flipX;

        float t;

        public DiveBombing diveBombing;

        Action ShipControllerFunction;
        Func<float, Vector2> DivePathFunction;

        enum SpritePosition { LEFT, RIGHT, TOP, BOTTOM };
        SpriteRenderer spriteRenderer;
        Vector3 spriteSize;

        // Start is called before the first frame update
        void Start()
        {
            transform.position = new Vector2(-100, diveBombing.HEIGHT / 2 - 100);
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteSize = GetSpriteSize();
            ShipControllerFunction = ShipControllerPan;
        }

        // Update is called once per frame
        void Update()
        {
            ShipControllerFunction();
        }

        public Vector2 DivePath(float t)
        {
            /* Get the ship's position at time t when diving.
               This is relative to the ship's original position (so, at t=0, dive_path(t) returns(0, 0)).
               Here we flip to the right before diving.
            */
            if (t < 0.5f) // During the first 0.5s, do a half-loop to the right
            {
                return new Vector2
                    (
                      (float)(50 * (1 - Math.Cos(2 * t * Math.PI))),
                      (float)(-50 * (Math.Sin(2 * t * Math.PI)))
                    );
            }

            t -= 0.5f;

            // For the rest of the time, follow a sinusoidal path downwards
            return new Vector2
                (
                  (float)(DIVE_WOBBLE_AMOUNT * Math.Cos(t * DIVE_WOBBLE_SPEED)),
                  t * 350 * -1
                );
        }


        public Func<float, Vector2> MakeIndividualDive(Vector3 startPos, bool filpX = false)
        {
            /* Return a function that will return a dive path from start_pos.
               If flip_x is True then the path will be flipped in the x direction.
            */
            int dir = filpX ? -1 : 1;
            float sx = startPos.x, sy = startPos.y;

            Vector2 _DivePath(float t)
            {
                Vector2 path = DivePath(t);

                Vector2 X = new Vector2(sx, sy);

                return new Vector2
                    (
                        sx + path.x * dir,
                        sy + path.y
                    );
            }
            return _DivePath;
        }

        public void ShipControllerPan()
        {
            transform.Translate(vx * Time.deltaTime, 0, 0);

            if (GetSpriteSidePosition(SpritePosition.RIGHT) > diveBombing.WIDTH / 2 - 50)
            {
                vx = -vx;
            }
            else if (GetSpriteSidePosition(SpritePosition.LEFT) < -diveBombing.WIDTH / 2 + 50)
            {
                vx = -vx;
            }
        }

        public void ShipControllerDive()
        {
            //  Update the ship when the ship is diving.
            // Move the ship along the path
            t += Time.deltaTime;

            Vector2 _pos = DivePathFunction(t);

            Vector2 direction = _pos - new Vector2(transform.position.x, transform.position.y);

            // Look ahead along the path to see what direction the ship
            // is moving, and set the ship's rotation accordingly
            transform.rotation = Quaternion.FromToRotation(Vector3.up, _pos);

            transform.position = _pos;

            if (GetSpriteSidePosition(SpritePosition.TOP) < -diveBombing.HEIGHT / 2)
            {
                ShipControllerFunction = ShipControllerPan;
                transform.position = DivePathFunction(0);
                transform.rotation = Quaternion.identity;
                StartCoroutine(diveBombing.Shedule(StartDive, 3));
            }
        }

        public void StartDive()
        {
            // Make the ship start a dive.
            // flip the dive if we're on the left of the screen
            flipX = transform.position.x < 0;
            ShipControllerFunction = ShipControllerDive;
            DivePathFunction = MakeIndividualDive(transform.position, flipX);
            t = 0;
        }

        // UTILITIES
        float GetSpriteSidePosition(SpritePosition spritePosition)
        {
            Vector3 centerPos = spriteRenderer.bounds.center;
            float position;

            switch (spritePosition)
            {
                case SpritePosition.LEFT: position = centerPos.x - spriteSize.x / 2; break;
                case SpritePosition.RIGHT: position = centerPos.x + spriteSize.x / 2; break;
                case SpritePosition.TOP: position = centerPos.y + spriteSize.y / 2; break;
                case SpritePosition.BOTTOM: position = centerPos.y - spriteSize.y / 2; break;
                default: position = 0; break;
            }

            return position;
        }

        Vector3 GetSpriteSize()
        {
            return GetComponent<SpriteRenderer>().bounds.size;
        }
    }
}
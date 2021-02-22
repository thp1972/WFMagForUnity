using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_007
{
    public class JumpPhysics : MonoBehaviour
    {
        public GameObject rectPrefab;

        //  define screen size
        const int WIDTH = 800;
        const int HEIGHT = 800;

        // define a colour
        Color32 MAROON = new Color32(128, 0, 0, 255);

        // vertical acceleration
        const float GRAVITY = -0.2f;

        // a list of platforms, each is a rectangle
        // in the form ((x,y)(w,h))
        List<Rect> platforms = new List<Rect>();

        public Player player;
        void Start()
        {
            platforms.Add(new Rect(Instantiate(rectPrefab), new Vector2(0, 780), new Vector2(800, 20)));
            platforms.Add(new Rect(Instantiate(rectPrefab), new Vector2(200, 700), new Vector2(100, 100)));
            platforms.Add(new Rect(Instantiate(rectPrefab), new Vector2(400, 650), new Vector2(100, 20)));
            platforms.Add(new Rect(Instantiate(rectPrefab), new Vector2(600, 600), new Vector2(100, 20)));

            foreach (var p in platforms)
            {
                p.SetColor(MAROON);
            }

            // create a player and define initial vertical velocity
            player.SetPosition(50, 450);

            // define initial and jump velocities
            player.yVelocity = 0;
            player.jumpVelocity = 7;
            player.gameObject.SetActive(true);
        }

        void Update()
        {
            //
            // horizontal movement
            //

            // temporary variable to store new x position
            float newX = player.X;

            // calculate new horizontal position if
            // arrow keys are pressed
            if (Input.GetKey(KeyCode.LeftArrow) && player.X > ScreenUtility.TopLeft.x + 10)
                newX -= 2;
            if (Input.GetKey(KeyCode.RightArrow) && player.X < ScreenUtility.TopRight.x - 10)
                newX += 2;

            // create a Vector3 for the new x position
            Vector3 newPositionX = new Vector3(newX, player.Y, 0);

            // check whether the new player position
            // collides with any platform
            bool xCollision = false;
            foreach (var p in platforms)
                xCollision = player.ColliderRect(newPositionX, p.Go) || xCollision;

            // only allow the player to move if it
            // doesn't collide with any platforms
            if (!xCollision)
                player.X = newX;

            //
            // vertical movement
            //

            // temporary variable to store new y position
            float newY = player.Y;

            // acceleration is rate of change of velocity
            player.yVelocity += GRAVITY;
            // velocity is rate of change of position
            newY += player.yVelocity;

            // create a Vector3 for the new y position
            Vector3 newPositionY = new Vector3(player.X, newY, 0);

            // check whether the new player position
            // collides with any platform
            bool yCollision = false;
            // also check whether the player is on the ground
            bool playerOnGround = false;

            foreach (var p in platforms)
            {
                yCollision = player.ColliderRect(newPositionY, p.Go) || yCollision;
                // player collided with ground if player's y position is
                // lower than the y position of the platform
                if (player.ColliderRect(newPositionY, p.Go) &&
                    (Mathf.Round(player.Y) - player.h / 2) <= (p.Y + p.Go.transform.localScale.y / 2))
                {
                    playerOnGround = true || playerOnGround;
                }
            }

            // player no longer has vertical velocity
            // if colliding with a platform
            if (yCollision)
                player.yVelocity = 0;
            // only allow the player to move if it
            // doesn't collide with any platforms
            else
                player.Y = newY;

            // pressing space sets a positive vertical velocity
            // only if player is on the ground
            if (playerOnGround && Input.GetKeyDown(KeyCode.Space))
                player.yVelocity = player.jumpVelocity;
        }
    }
}
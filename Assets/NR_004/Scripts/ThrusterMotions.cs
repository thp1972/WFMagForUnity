using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_004
{
    public class ThrusterMotions : MonoBehaviour
    {
        // set screen width and height
        const int WIDTH = 800;
        const int HEIGHT = 800;

        float angle = 90;

        // set an acceleration for the spaceship
        const float ACCELERATION = 0.02f;

        // initially the spaceship is stationary
        float xSpeed = 0;
        float ySpeed = 0;

        public Sprite spaceship;
        public Sprite spaceshipThrust;
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#endif
        }

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            float newAngle = transform.eulerAngles.z;

            // rotate left on left arrow press
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(0, 0, 2);
            }

            // rotate right on right arrow press
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(0, 0, -2);
            }

            // accelerate forwards on up arrow press
            // and change displayed image
            if (Input.GetKey(KeyCode.UpArrow))
            {
                spriteRenderer.sprite = spaceshipThrust;

                xSpeed += Mathf.Cos(Mathf.Deg2Rad * newAngle) * ACCELERATION;
                ySpeed += Mathf.Sin(Mathf.Deg2Rad * newAngle) * ACCELERATION;
            }
            else
            {
                spriteRenderer.sprite = spaceship;
            }

            // use the x and y speed to update the spaceship position
            transform.position += new Vector3(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime);
        }
    }
}
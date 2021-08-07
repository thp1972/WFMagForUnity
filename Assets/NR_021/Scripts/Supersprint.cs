using PygameZero;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_021
{
    public class Car : Actor
    {
        public Car(string image, (float, float) pos, SpriteUtility.PivotPosition pivotPosition) :
            base(image, pos, pivotPosition)
        { }
    }

    public class Supersprint : MonoBehaviour
    {
        public GameObject trackPrefab;
        GameObject track;

        Image controlimage1 = new Image();
        Image controlimage2 = new Image();
        List<Car> cars;

        void Start()
        {
            Application.targetFrameRate = 60;
            track = Instantiate(trackPrefab);

            controlimage1.Load("guide1");
            controlimage2.Load("guide2");
            cars = new List<Car>();

            foreach (var c in Enumerable.Range(0, 4))
            {
                cars.Add(Actor("car" + c, center: (400, 70 + (30 * c))));
                cars[c].speed = 0;
            }
        }

        void Update()
        {
            Draw();

            if (EventsDetector.Keyboard.Up) cars[0].speed += 0.15f;
            if (EventsDetector.Keyboard.Down) cars[0].speed -= 0.15f;
            if (cars[0].speed != 0)
            {
                if (EventsDetector.Keyboard.Left) cars[0].Angle = 2;
                if (EventsDetector.Keyboard.Right) cars[0].Angle = -2;
            }

            foreach (var c in Enumerable.Range(0, 4))
            {
                (float, float) newPos;
                Color32 ccol;

                bool crash = false;
                foreach (var i in Enumerable.Range(0, 4))
                {
                    if (cars[c].CollidePoint(cars[i].Center) && c != i)
                    {
                        crash = true;
                        cars[c].speed = -(Random.Range(0f, 1f) / 10);
                    }
                }

                if (crash)
                    newPos = CalcNewXY(cars[c].Center, 2, Mathf.Deg2Rad * (Random.Range(0f, 360f) - cars[c].Angle));
                else
                    newPos = CalcNewXY(cars[c].Center, cars[c].speed * 2, Mathf.Deg2Rad * (180 - cars[c].Angle));

                if (c == 0)
                    ccol = controlimage1.GetAt((int)(newPos.Item1), (int)(newPos.Item2));
                else
                    ccol = controlimage2.GetAt((int)(newPos.Item1), (int)(newPos.Item2));

                if (cars[c].speed != 0)
                {
                    if (ccol != Color.blue && ccol != Color.red)
                        cars[c].Center = newPos;
                    else
                    {
                        if (c > 0)
                        {
                            if (ccol == Color.blue)
                                cars[c].Angle = 5;
                            if (ccol == Color.red)
                                cars[c].Angle = -5;
                        }
                        cars[c].speed = cars[c].speed / 1.1f;
                    }
                }

                if (c > 0 && cars[c].speed < 1.8f + (c / 10))
                {
                    cars[c].speed += Random.Range(0f, 1f) / 10;
                    if (crash)
                        cars[c].Angle = ((ccol.g - 136) / 136f) * (2.8f * cars[c].speed);
                    else
                        cars[c].Angle = -((ccol.g - 136) / 136f) * (2.8f * cars[c].speed);
                }
                else
                    cars[c].speed = cars[c].speed / 1.1f;
            }
        }

        (float, float) CalcNewXY((float, float) xy, float speed, float ang)
        {
            float newX = xy.Item1 - (speed * Mathf.Cos(ang));
            float newY = xy.Item2 - (speed * Mathf.Sin(ang));
            return (newX, newY);
        }

        public void Draw()
        {
            ScreenUtility.Blit(track, (0, 0));
            foreach (var c in Enumerable.Range(0, 4))
                cars[c].Draw();
        }

        public Car Actor(string image, (float, float) center)
        {
            return new Car(image, center, SpriteUtility.PivotPosition.CENTER);
        }
    }
}
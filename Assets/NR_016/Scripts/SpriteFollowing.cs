using PygameZero;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteFollowing : MonoBehaviour
{
    class Spaceship : Actor
    {
        public Spaceship(string image, (float, float) pos = default((float, float))) : base(image, pos) { }
    }

    class Powerups : Actor
    {
        public Powerups(string image, (float, float) pos = default((float, float))) : base(image, pos) { }
    }

    // set screen width and height
    int WIDTH = 800;
    int HEIGHT = 800;

    Spaceship spaceship;
    List<Powerups> powerups;
    List<(float, float)> previouspositions;

    // Start is called before the first frame update
    void Start()
    {
        // create spaceship and a list of 3 powerups
        spaceship = new Spaceship("spaceship", pos: (400, 400));
        spaceship.speed = 4;
        powerups = (from p in Enumerable.Range(0, 3) select new Powerups("powerup")).ToList();

        // create a list of previous positions
        // initially containing values to the left of the spaceship
        previouspositions = (from i in Enumerable.Range(0, 100) select (spaceship.X - i * spaceship.speed, spaceship.Y)).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        // store spaceship previous position
        (float, float) previousposition = (spaceship.X, spaceship.Y);

        // use arrow keys to move the spaceship
        if (Input.GetKey(KeyCode.UpArrow))
            spaceship.Y -= spaceship.speed;
        if (Input.GetKey(KeyCode.DownArrow))
            spaceship.Y += spaceship.speed;
        if (Input.GetKey(KeyCode.LeftArrow))
            spaceship.X -= spaceship.speed;
        if (Input.GetKey(KeyCode.RightArrow))
            spaceship.X += spaceship.speed;

        // add new position to list if the spaceship has moved
        // and ensure the list contains at most 100 positions
        if (previousposition != (spaceship.X, spaceship.Y))
        {
            previouspositions.Insert(0, previousposition);
            previouspositions.RemoveAt(previouspositions.Count - 1);
        }

        // set the new position of each powerup
        foreach (var ip in powerups.Select((i, p) => (i,p)))
        {
            var i = ip.Item2;
            var p = ip.Item1;
            var newPosition = previouspositions[(i + 1) * 20];
            p.Pos = (newPosition.Item1, newPosition.Item2);
        }

        Draw();
    }

    private void Draw()
    {
        spaceship.Draw();
        foreach(var p in powerups)
            p.Draw();
    }
}

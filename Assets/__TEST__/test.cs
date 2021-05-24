using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Vector2 pos = new Vector2(500, 562 - 40);
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        int WIDTH = 1000;
        int HEIGHT = 1000 * 9 / 16;

        int centerx = WIDTH / 2;
        int centery = HEIGHT / 2;

        int TRAIL_LENGTH = 2;





        var angle = 3.14f;// Random.Range(-Mathf.PI, Mathf.PI);
        var speed = 170; // 255 * Mathf.Pow(Random.Range(0.3f, 1.0f), 2);

        var dx = Mathf.Cos(angle);
        var dy = Mathf.Sin(angle);

        var d = 50; // random.uniform(25 + TRAIL_LENGTH, 100)
        var pos = (centerx + dx * d, centery + dy * d);
        var vel = (speed * dx, speed * dy);

        float hspeed = HypotenuseLength(vel.Item1, vel.Item2);


        print($"{pos} - {vel} - {hspeed}");


    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

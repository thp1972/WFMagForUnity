using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UTILITY;

public class DrawTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Draw draw = GetComponent<Draw>();

        GameObject sprite = draw.CreateFilledCircle((255, 255, 255), 0, 1);
        draw.DrawFilledCircle(sprite, (-40,0),10);
        sprite.GetComponent<SpriteRenderer>().color = Color.red;


        sprite = draw.CreateFilledRect((255, 255, 255), 0, 1);
        draw.DrawFilledRect(sprite, (0, 0), (100,100));        
        sprite.GetComponent<SpriteRenderer>().color = Color.red;



    }

    // Update is called once per frame
    void Update()
    {

    }
}

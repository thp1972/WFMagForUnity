using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rect rect = new Rect(transform.position, transform.localScale);

        var centerX = transform.position.x + transform.localScale.x / 2;

        print(rect.center.x + " " + centerX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

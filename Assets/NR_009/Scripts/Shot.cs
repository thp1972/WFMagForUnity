using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_009
{
    public class Shot
    {
        public Vector2 pos
        {
            get
            {
                if (sprite != null) return sprite.transform.position;
                else return Vector2.zero;
            }
            set { if (sprite != null) sprite.transform.position = value; }
        }

        public GameObject sprite;

        public bool exploding;
        public int timer;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_007
{
    public class Player : MonoBehaviour
    {
        public float yVelocity;
        public float jumpVelocity;
        public float w = 20;
        public float h = 20;

        public void SetPosition(float x, float y)
        {
            transform.position = new Vector3(
                ScreenUtility.TopLeft.x + x,
                ScreenUtility.TopLeft.y - y,
                0);
        }

        public float X
        {
            get { return transform.position.x; }
            set { transform.position = new Vector3(value, transform.position.y); }
        }

        public float Y
        {
            get { return transform.position.y; }
            set { transform.position = new Vector3(transform.position.x, value); }
        }

        public bool ColliderRect(Vector3 np, GameObject p)
        {
            bool collision = false;

            // localScale * 10 because the sprite is 20x20
            Collider[] hitColliders =
                Physics.OverlapBox(np,
                transform.localScale * 10, Quaternion.identity);

            if (hitColliders.Length > 0)
            {
                foreach (var hc in hitColliders)
                {
                    if (hc.gameObject == p)
                        collision = true;
                }
            }

            return collision;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_009
{
    public class Invaders : MonoBehaviour
    {
        // I changed the anchor point of images to TOP,LEFT as pygame do 
        public GameObject imageShot;
        public GameObject imageShotBlack;
        public GameObject imageExplode;
        public GameObject imageExplodeBlack;
        public GameObject imageShield;

        SpriteUtility spriteUtilityForImageShot;

        List<Shot> shots = new List<Shot>(),
                   toDelete = new List<Shot>();
        bool firstFrame = true;

        private void Awake()
        {
            // shot placeholder
            GameObject shot = Instantiate(imageShot);
            shot.SetActive(false);
            //---------------------------
            spriteUtilityForImageShot = new SpriteUtility(shot.GetComponent<SpriteRenderer>());
        }

        // Start is called before the first frame update
        void Start()
        {
            // A shot will be created in a random position every half second
            InvokeRepeating("CreateRandomShot", 0, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (firstFrame)
            {
                var width = (int)ScreenUtility.GameResolution.x;

                for (int x = 50; x < width; x += 300)
                {
                    var _x = (int)ScreenUtility.Position(x, 0).x;
                    Instantiate(imageShield, new Vector2(_x, ScreenUtility.Position(0, 500).y), Quaternion.identity);
                }

                firstFrame = false;
            }

            Blit();
        }

        private void Blit()
        {
            foreach (var item in toDelete)
            {
                item.pos = item.pos;
            }
            ClearToDelete(); // Clear list

            // Step backwards through the shots list. This avoids errors that occur
            // when deleting items from the list during the for loop.
            foreach (var shot in shots.Reverse<Shot>())
            {
                if (shot.exploding)
                {
                    shot.timer -= 1;
                    if (shot.timer <= 0)
                    {
                        var s = Instantiate(imageExplodeBlack, shot.pos, Quaternion.identity);
                        s.SetActive(false);
                        toDelete.Add(new Shot { pos = shot.pos, sprite = s });
                        Destroy(shot.sprite);
                        shots.Remove(shot);
                    }
                }
                else // Before moving shot, add the current position to the to_delete list
                {
                    var currentPos = new Vector2(shot.pos.x, shot.pos.y);
                    toDelete.Add(new Shot { pos = currentPos, sprite = Instantiate(imageShotBlack, currentPos, Quaternion.identity) });
                    shot.pos = new Vector2(shot.pos.x, shot.pos.y - 10);

                    // Do collision detection based on the centre of the sprite
                    var shotSize = spriteUtilityForImageShot.GetSpriteSize();
                    var halfWidth = shotSize.x / 2;
                    var halfHeight = shotSize.y / 2;

                    if (shot.pos.y - halfHeight <= ScreenUtility.BottomLeft.y)
                    {
                        shots.Remove(shot);
                        Destroy(shot.sprite);
                    }
                    else // Hit something? If so change to exploding sprite
                    {
                        var collideCheckPos = (shot.pos.x + halfWidth, shot.pos.y - halfHeight);

                        Collider2D objCollided;
                        if (GetAt(collideCheckPos, out objCollided))
                        {
                            Destroy(shot.sprite);
                            shot.sprite = Instantiate(imageExplode, new Vector2(shot.pos.x, shot.pos.y), Quaternion.identity);

                            SpriteUtility spriteUtility = new SpriteUtility(objCollided.gameObject.GetComponent<SpriteRenderer>(), spriteManipulation: true);
                            spriteUtility.ErasePixels(new Vector2(shot.pos.x, shot.pos.y), imageExplode.GetComponent<SpriteRenderer>().sprite);

                            shot.exploding = true;
                            shot.timer = 5;
                        }
                    }
                }
            }
        }

        private void ClearToDelete()
        {
            foreach (var i in toDelete.Reverse<Shot>())
            {
                if (i.sprite.gameObject.tag != "ExplodeBlack")
                {
                    Destroy(i.sprite);
                }
                toDelete.Remove(i);
            }
        }

        bool GetAt((float, float) pos, out Collider2D objCollided)
        {
            objCollided = null;
            var collided = false;
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(pos.Item1, pos.Item2), Vector3.down, .1f);

            if (hit.collider)
            {
                objCollided = hit.collider;
                collided = true;
            }
            return collided;
        }

        void CreateRandomShot()
        {
            var r1 = ScreenUtility.Position(0, 0).x;
            var r2 = ScreenUtility.Position((ScreenUtility.GameResolution.x - spriteUtilityForImageShot.GetSpriteSize().x) / 10 * 10, 0).x;
            var xPos = Random.Range(r1, r2);
            var yPos = ScreenUtility.Position(0, 0).y;
            var pos = new Vector2(xPos, yPos);

            shots.Add(new Shot
            {
                sprite = Instantiate(imageShot),
                pos = pos,
                exploding = false
            });
        }
    }
}
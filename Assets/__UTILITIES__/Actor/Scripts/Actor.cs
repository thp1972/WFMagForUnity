using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PygameZero
{
    // NOTE:
    // Actor in Pygame defaults to CENTER pivot
    public class Actor
    {
        string image;
        GameObject actor;
        Sprite sprite;

        /// <summary>
        /// Get/Set the current position, but as a Tuple
        /// </summary>
        public (float, float) Pos
        {
            get
            {
                (float, float) _pos = (pos.x, pos.y);
                return _pos;
            }
            set
            {
                pos.x = value.Item1;
                pos.y = value.Item2;
                X = pos.x;
                Y = pos.y;
            }
        }

        public Vector2 pos;
        public float speed;

        private Dictionary<string, Sprite> spriteStack;

        public SpriteUtility.PivotPosition Anchor
        {
            set
            {
                if (spriteRenderer)
                {
                    var su = new SpriteUtility(spriteRenderer);
                    su.SetPivot(value);
                }
            }
        }

        public int SortingOrder
        {
            get { return spriteRenderer.sortingOrder; }
            set { spriteRenderer.sortingOrder = value; }
        }

        /// <summary>
        /// Set a new image, but as a String
        /// </summary>
        public string Image
        {
            set
            {
                image = value;
                Sprite spriteToInstantiate = null;
                sprite = GetSpriteFromStack(value);

                if (!sprite)
                {
                    var op = Addressables.LoadAssetAsync<Sprite>(value);
                    spriteToInstantiate = op.WaitForCompletion(); // force sync!
                    spriteStack[value] = spriteToInstantiate;
                }

                spriteRenderer = actor.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = spriteStack[value];
                SortingOrder = 1; // actor at least on top of a background...
                X = pos.x;
                Y = pos.y;
            }
            get { return image; }
        }

        private float x;
        private float y;
        private int angle;

        private Sprite GetSpriteFromStack(string image)
        {
            return spriteStack.ContainsKey(image) ? spriteStack[image] : null;
        }

        public float X
        {
            get => x;
            set
            {
                x = value;
                pos.x = x;
                actor.transform.position = ScreenUtility.Position(pos);
            }
        }
        public float Y
        {
            get => y;
            set
            {
                y = value;
                pos.y = y;
                actor.transform.position = ScreenUtility.Position(pos);
            }
        }

        /// <summary>
        /// Get/set angle in degree
        /// </summary>
        public float Angle
        {
            set
            {
                angle = (int)actor.transform.rotation.eulerAngles.z;
                actor.transform.Rotate(Vector3.forward * value);
            }
            get { return angle; }
        }

        public Vector2 center;
        /// <summary>
        /// Get/Set the center position, but as a Tuple
        /// </summary>
        public (float, float) Center
        {
            get
            {
                var su = new SpriteUtility(spriteRenderer);
                if (su.GetPivotPosition() != SpriteUtility.PivotPosition.CENTER)
                {
                    return (spriteRenderer.sprite.bounds.center.x, spriteRenderer.sprite.bounds.center.y);
                }
                else
                    return Pos;
            }
            set
            {
                var su = new SpriteUtility(spriteRenderer);
                if (su.GetPivotPosition() != SpriteUtility.PivotPosition.CENTER)
                {
                    Anchor = SpriteUtility.PivotPosition.CENTER;
                    center = new Vector2(value.Item1, value.Item2);
                    Pos = value;
                }
                else
                    Pos = value;

                actor.transform.position = ScreenUtility.Position(pos);
            }
        }

        private SpriteRenderer spriteRenderer;

        public Actor() { }

        public Actor(string image, Vector2 pos = default,
                     SpriteUtility.PivotPosition anchor = SpriteUtility.PivotPosition.CENTER)
        {
            spriteStack = new Dictionary<string, Sprite>();

            // load the default Actor prefab
            var op = Addressables.LoadAssetAsync<GameObject>("Actor");
            var actorToInstantiate = op.WaitForCompletion(); // force sync!
            actor = GameObject.Instantiate(actorToInstantiate, pos, Quaternion.identity);
            actor.SetActive(false);
            // ----

            this.pos = pos;
            Image = image;

            // default is TOPLEFT also in Editor settings
            if (anchor != SpriteUtility.PivotPosition.CENTER) Anchor = anchor;
        }

        public Actor(string image, (float, float) pos = default,
            SpriteUtility.PivotPosition anchor = SpriteUtility.PivotPosition.CENTER) : this(image, new Vector2(pos.Item1, pos.Item2), anchor) { }

        public virtual void Destroy()
        {
            GameObject.Destroy(actor);
        }

        public virtual void Draw()
        {
            actor?.SetActive(true);
        }

        public bool CollideRect(Actor other)
        {
            var r1 = new Rect(pos.x, pos.y, spriteRenderer.sprite.rect.width, spriteRenderer.sprite.rect.height);
            var r2 = new Rect(other.pos.x, other.pos.y, other.spriteRenderer.sprite.rect.width, other.spriteRenderer.sprite.rect.height);

            return // is the left-hand edge of sprite 1 further  
                   // left than the right - hand edge of sprite 2
                   r1.x < r2.x + r2.width &&
                   // Is the right - hand edge of sprite 1 further
                   // right than the left - hand edge of sprite 2
                   r1.x + r1.width > r2.x &&
                   // Is the top edge of sprite 1 higher up than 
                   // the bottom edge of sprite 2(y1 < y2 + h2)
                   r1.y < r2.y + r2.height &&
                   // Is the bottom edge of sprite 1 lower down
                   // than the top edge of sprite 2(y1 + h1 > y2) ?
                   r1.y + r1.height > r2.y;
        }

        public bool CollidePoint((float, float) point)
        {
            Vector2 _point = new Vector2(point.Item1, point.Item2);
            var r1 = new Rect(pos.x, pos.y, spriteRenderer.sprite.rect.width, spriteRenderer.sprite.rect.height);
            return r1.Contains(_point);
        }
    }
}

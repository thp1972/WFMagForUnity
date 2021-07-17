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
    abstract class Actor
    {
        /// <summary>
        /// Get the current image, but as a GameObject
        /// </summary>
        public GameObject image;

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

        private Dictionary<string, GameObject> imageStack;

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

        /// <summary>
        /// Set a new image, but as a String
        /// </summary>
        public string Image
        {
            set
            {
                GameObject imageToInstantiate = null;
                image = GetImageFromStack(value);
                if (!image)
                {
                    var op = Addressables.LoadAssetAsync<GameObject>(value);
                    imageToInstantiate = op.WaitForCompletion(); // force sync!
                    image = GameObject.Instantiate(imageToInstantiate, pos, Quaternion.identity);
                    imageStack[value] = image;
                }
                image.SetActive(false);
                spriteRenderer = this.image.GetComponent<SpriteRenderer>();
                X = pos.x;
                Y = pos.y;
            }
        }

        private float x;
        private float y;

        private GameObject GetImageFromStack(string image)
        {
            // first make all disabled...
            foreach (var i in imageStack)
                i.Value.SetActive(false);

            return imageStack.ContainsKey(image) ? imageStack[image] : null;
        }

        public float X
        {
            get => x;
            set
            {
                x = value;
                pos.x = x;
                image.transform.position = ScreenUtility.Position(pos);
            }
        }
        public float Y
        {
            get => y;
            set
            {
                y = value;
                pos.y = y;
                image.transform.position = ScreenUtility.Position(pos);
            }
        }

        private SpriteRenderer spriteRenderer;

        public Actor() { }

        public Actor(string image, Vector2 pos = default,
                     SpriteUtility.PivotPosition anchor = SpriteUtility.PivotPosition.TOPLEFT)
        {
            imageStack = new Dictionary<string, GameObject>();
            this.pos = pos;
            Image = image;

            // default is TOPLEFT also in Editor settings
            if (anchor != SpriteUtility.PivotPosition.TOPLEFT) Anchor = anchor;
        }

        public Actor(string image, (float, float) pos = default,
            SpriteUtility.PivotPosition anchor = SpriteUtility.PivotPosition.TOPLEFT) : this(image, new Vector2(pos.Item1, pos.Item2), anchor) { }

        public virtual void Destroy()
        {
            GameObject.Destroy(image);
        }

        public virtual void Draw()
        {
            image?.SetActive(true);
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
    }
}

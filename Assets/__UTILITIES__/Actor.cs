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
    abstract class Actor
    {
        /// <summary>
        /// Get the current image, but as a GameObject
        /// </summary>
        public GameObject image;

        /// <summary>
        /// Get the current position, but as a Tuple
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

        public Actor() { }

        public Actor(string image, Vector2 pos = default(Vector2))
        {
            imageStack = new Dictionary<string, GameObject>();
            this.pos = pos;
            Image = image;
        }

        public Actor(string image, (float, float) pos = default((float, float))) : this(image, new Vector2(pos.Item1, pos.Item2)) { }

        public virtual void Destroy()
        {
            GameObject.Destroy(image);
        }

        public virtual void Draw()
        {
            image?.SetActive(true);
        }
    }
}

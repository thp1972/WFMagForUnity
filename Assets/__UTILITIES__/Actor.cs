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
        public GameObject image;
        protected Vector2 pos;

        // every image has a name; i.e. 'walkleft' then 'stand' can have one or more images (i.e. for an animation)
        // i.e 'walkleft1','walkleft2'
        protected Dictionary<string, Dictionary<string, GameObject>> images =
            new Dictionary<string, Dictionary<string, GameObject>>();

        public Actor(Vector2 pos, string imageNameState, params string[] imageFrameNames)
        {
            this.pos = pos;

            if (!ImageAlreadyLoaded(imageNameState))
            {
                Addressables.LoadAssetAsync<GameObject>(image).Completed += (AsyncOperationHandle<GameObject> obj) =>
                {
                    GameObject s = obj.Result;
                    this.image = GameObject.Instantiate(s, pos, Quaternion.identity);
                    this.image.SetActive(false);

                };
            }
        }

        public virtual void Destroy()
        {
            GameObject.Destroy(image);
        }

        bool ImageAlreadyLoaded(string imageNameState)
        {
           var imageFrames = images[imageNameState];
            return false;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PygameZero
{
    public class Image
    {
        GameObject _image;
        SpriteUtility _spriteUtility;

        public void Load(string image)
        {
            var op = Addressables.LoadAssetAsync<GameObject>(image);
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _image = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);
            _spriteUtility = new SpriteUtility(_image.GetComponent<SpriteRenderer>());
        }

        public Color32 GetAt(int posX, int posY)
        {
            // convert pygame coords to unity coords
            Vector2 UnityWorldPos = ScreenUtility.Position(posX, posY);
            return _spriteUtility.GetPixelAt(UnityWorldPos);
        }
    }
}
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
        Sprite _sprite;
        SpriteRenderer _spriteRenderer;

        public Image()
        {
            // load the default addressable Image
            var op = Addressables.LoadAssetAsync<GameObject>("Image");
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _image = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);
            _spriteRenderer = _image.GetComponent<SpriteRenderer>();
        }

        public void Load(string image)
        {
            var op = Addressables.LoadAssetAsync<Sprite>(image);
            var _sprite = op.WaitForCompletion(); // force sync!
            _spriteUtility = new SpriteUtility(_image.GetComponent<SpriteRenderer>());
            _spriteRenderer.sprite = _sprite;
        }

        public Color32 GetAt(int posX, int posY)
        {
            // convert pygame coords to unity coords
            Vector2 UnityWorldPos = ScreenUtility.Position(posX, posY);
            return _spriteUtility.GetPixelAt(UnityWorldPos);
        }
    }
}
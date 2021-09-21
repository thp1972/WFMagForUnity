using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PygameZero
{
    public class Surface
    {
        GameObject _surface;
        SpriteUtility _spriteUtility;

        public Surface((int, int) size)
        {
            var op = Addressables.LoadAssetAsync<GameObject>("Surface");
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _surface = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);

            // default sprite is 256x256 so scale based to size
            _surface.transform.localScale = new Vector3(size.Item1 / 256f, size.Item2 / 256f, 1);
            _surface.transform.position = ScreenUtility.Position(0, 0);

            _spriteUtility = new SpriteUtility(_surface.GetComponent<SpriteRenderer>());
        }
    }
}

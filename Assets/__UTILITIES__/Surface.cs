using System;
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
        Texture2D texture;

        class TimeUtilityImpl : MonoBehaviour { }
        static TimeUtilityImpl ti;

        public Surface((int, int) size, bool SRCALPHA = false)
        {
            var op = Addressables.LoadAssetAsync<GameObject>($"Surface{size.Item1}x{size.Item2}");
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _surface = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);

            // default sprite is 256x256 so scale based to size
            //_surface.transform.localScale = new Vector3(size.Item1 / 256f, size.Item2 / 256f, 1);
            _surface.transform.position = ScreenUtility.Position(Vector3.zero);

            _spriteUtility = new SpriteUtility(_surface.GetComponent<SpriteRenderer>());

            if (SRCALPHA)
                MakeAllTextureTransparent(size);

            GameObject go = new GameObject();
            ti = go.AddComponent<TimeUtilityImpl>();
        }

        private void MakeAllTextureTransparent((int, int) size)
        {
            for (int x = 0; x < size.Item1; x++)
            {
                for (int y = 0; y < size.Item2; y++)
                {
                    _spriteUtility.SetPixelAt(ScreenUtility.Position(new Vector2(x, y)), Color.clear);
                }
            }

            _spriteUtility.Apply();
        }

        public void SetAt((int, int) coords, (byte, byte, byte, byte) color)
        {
            Vector2 pos = ScreenUtility.Position(new Vector2(coords.Item1, coords.Item2));
            _spriteUtility.SetPixelAt(pos, new Color32(color.Item1, color.Item2, color.Item3, color.Item4));
        }

        public void Apply()
        {
            _spriteUtility.Apply();
        }


        public void Scroll(int x, int y)
        {
            ti.StartCoroutine(_Scroll());

            IEnumerator _Scroll()
            {
                yield return new WaitForEndOfFrame();
                float offset = Time.time * 1f;
                _spriteUtility.Scroll(offset, y);
            }
        }
    }
}

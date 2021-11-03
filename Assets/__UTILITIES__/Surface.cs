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
        Texture2D texture;
        MeshRenderer meshRenderer;
        Coroutine scrollCor;

        class TimeUtilityImpl : MonoBehaviour { }
        static TimeUtilityImpl ti;

        public Surface((int, int) size, bool SRCALPHA = false)
        {
            var op = Addressables.LoadAssetAsync<GameObject>("Surface");
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _surface = GameObject.Instantiate(imageToInstantiate, Vector3.zero, Quaternion.identity);

            _surface.transform.localScale = new Vector3(size.Item1, size.Item2, 1);

            SurfaceInit();

            if (SRCALPHA)
                MakeAllTextureTransparent(size);

            GameObject go = new GameObject();
            ti = go.AddComponent<TimeUtilityImpl>();
        }

        // convert Unity texture bottom/left to pygame top,left
        Vector2 BottomLeftToTopLeft(Vector3 worldPos)
        {
            Vector2 texSpaceCoord = new Vector2(worldPos.x, (texture.height - worldPos.y) - 1);
            return texSpaceCoord;
        }

        void SetPixelAt(Vector2 worldPosition, Color c)
        {
            Vector2 texturePosition = BottomLeftToTopLeft(worldPosition);
            texture.SetPixel((int)texturePosition.x, (int)texturePosition.y, c);
        }

        public void SurfaceClear()
        {
            for (int x = 0; x < _surface.transform.localScale.x; x++)
            {
                for (int y = 0; y < _surface.transform.localScale.y; y++)
                {
                    SetPixelAt(new Vector2(x, y), Color.black);
                }
            }


            MakeAllTextureTransparent(((int)_surface.transform.localScale.x, (int)_surface.transform.localScale.y));

        }

        private void SurfaceInit()
        {
            meshRenderer = _surface.GetComponent<MeshRenderer>();

            texture = new Texture2D(meshRenderer.material.mainTexture.width,
                                    meshRenderer.material.mainTexture.height,
                                    TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.SetPixels(((Texture2D)meshRenderer.material.mainTexture).GetPixels());
            texture.Apply();

            meshRenderer.material.mainTexture = texture;
        }

        private void MakeAllTextureTransparent((int, int) size)
        {
            for (int x = 0; x < size.Item1; x++)
            {
                for (int y = 0; y < size.Item2; y++)
                {
                    SetPixelAt(new Vector2(x, y), Color.clear);
                }
            }

            Apply();
        }

        public void SetAt((int, int) coords, (byte, byte, byte, byte) color)
        {
            Vector2 pos = (new Vector2(coords.Item1, coords.Item2));
            SetPixelAt(pos, new Color32(color.Item1, color.Item2, color.Item3, color.Item4));
        }

        public void Apply()
        {
            texture.Apply();
        }

        public void Scroll(int x, int y)
        {
            if (scrollCor != null) return;

            scrollCor = ti.StartCoroutine(_Scroll());
            IEnumerator _Scroll()
            {
                Vector2 scrollOffset = Vector2.zero;
                while (true)
                {
                    yield return new WaitForEndOfFrame();

                    var _x = scrollOffset.x + (x / 10f) * Time.deltaTime;
                    var _y = scrollOffset.y + y * Time.deltaTime;
                    scrollOffset.x = _x;
                    scrollOffset.y = _y;

                    meshRenderer.material.SetTextureOffset("_MainTex", scrollOffset);
                }

                ti.StopCoroutine(scrollCor);
                yield return null;
            }
        }

        public void Scroll2(int x, int y)
        {
            if (scrollCor != null) return;

            scrollCor = ti.StartCoroutine(_Scroll());
            IEnumerator _Scroll()
            {
                Vector2 scrollOffset = Vector2.zero;
                while (true)
                {
                    yield return new WaitForEndOfFrame();

                    scrollOffset.x = x * Time.deltaTime;
                    scrollOffset.y = y * Time.deltaTime;

                    _surface.transform.Translate(scrollOffset);

                    if (Mathf.Abs(_surface.transform.position.x) >= 800) _surface.transform.position = new Vector3(0, 0, 0);
                }

                ti.StopCoroutine(scrollCor);
                yield return null;
            }
        }
    }
}

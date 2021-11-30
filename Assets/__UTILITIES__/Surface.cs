using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PygameZero
{
    /// <summary>
    /// // Surface is a QUAD mesh with left-top pivot point
    /// </summary>
    public class Surface
    {
        public GameObject _surface;
        Texture2D texture;
        MeshRenderer meshRenderer;

        // position of the Surface
        float x;
        float y;
        Vector2 pos;

        // sise of the Surface
        int width;
        int height;

        Coroutine scrollCor;
        class TimeUtilityImpl : MonoBehaviour { }
        static TimeUtilityImpl ti;


        // The surface is a QUAD mesh scaled based on size
        // it has a Surface Material wich has a Shader Unlit/Transparent
        // the main texture is sized based on QUAD size
        public Surface((int, int) size, Texture2D texture = null, bool SRCALPHA = false)
        {
            var op = Addressables.LoadAssetAsync<GameObject>("Surface");
            var imageToInstantiate = op.WaitForCompletion(); // force sync!
            _surface = GameObject.Instantiate(imageToInstantiate, ScreenUtility.Position(Vector3.zero), Quaternion.identity);
            _surface.transform.localScale = new Vector3(size.Item1, size.Item2, 1);
            width = size.Item1;
            height = size.Item2;

            if (texture != null)
                SurfaceInitWithTexture(texture);
            else
                SurfaceInit(size);

            if (SRCALPHA)
                MakeAllTextureTransparent(size);

            GameObject go = new GameObject();
            ti = go.AddComponent<TimeUtilityImpl>();
        }


        public void SetOrigin(Vector2 position)
        {
            x = position.x;
            y = position.y;
            _surface.transform.position = ScreenUtility.Position(position);
        }

        public int Width { get => width; }
        public int Height { get => height; }

        public float X
        {
            get
            {
                pos = ScreenUtility.InversePosition(_surface.transform.position);
                x = pos.x;
                return x;
            }
            set
            {
                x = value;
                pos.x = x;
                pos.y = y;
                _surface.transform.position = ScreenUtility.Position(pos);
            }
        }

        public float Y
        {
            get
            {
                pos = ScreenUtility.InversePosition(_surface.transform.position);
                y = pos.y;
                return y;
            }
            set
            {
                y = value;
                pos.x = x;
                pos.y = y;
                _surface.transform.position = ScreenUtility.Position(pos);
            }
        }

        private void SurfaceInitWithTexture(Texture2D textureInit)
        {
            meshRenderer = _surface.GetComponentInChildren<MeshRenderer>();

            texture = new Texture2D(textureInit.width,
                                    textureInit.height,
                                    TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.SetPixels(textureInit.GetPixels());
            texture.Apply();

            meshRenderer.material.mainTexture = texture;
        }

        private void SurfaceInit((int, int) size)
        {
            meshRenderer = _surface.GetComponentInChildren<MeshRenderer>();

            texture = new Texture2D(size.Item1,
                                    size.Item2,
                                    TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Repeat;

            meshRenderer.material.mainTexture = texture;
        }

        // convert Unity texture bottom/left to pygame top,left
        public Vector2 BottomLeftToTopLeft(Vector3 worldPos)
        {
            Vector2 texSpaceCoord = new Vector2(worldPos.x, (texture.height - worldPos.y) - 1);
            return texSpaceCoord;
        }

        // convert Unity texture bottom/left to pygame top,left
        public Vector2 TopLeftToBottomLeft(Vector3 worldPos)
        {
            Vector2 texSpaceCoord = new Vector2(worldPos.x, (Mathf.Abs(worldPos.y - texture.height)) - 1);
            return texSpaceCoord;
        }

        public bool PositionInsideSurface((int, int) coords, ref Vector2 newPosition)
        {
            // this is if the Surface is scrolling...
            var xx = coords.Item1;
            var yy = coords.Item2;
            var ox = xx - X;
            var oy = yy - Y;

            newPosition.x = ox;
            newPosition.y = oy;

            // I.E. 0, 799 0, 599 if Surface is 800x600
            // if coords are inside the Surface RECT area
            return ox >= 0 && ox <= width - 1 && oy >= 0 && oy <= height - 1;
        }

        void SetPixelAt(Vector2 worldPosition, Color c)
        {
            Vector2 texturePosition = BottomLeftToTopLeft(worldPosition);
            texture.SetPixel((int)texturePosition.x, (int)texturePosition.y, c);
        }

        public void SetAt((int, int) coords, (byte, byte, byte, byte) color)
        {
            Vector2 pos = (new Vector2(coords.Item1, coords.Item2));
            SetPixelAt(pos, new Color32(color.Item1, color.Item2, color.Item3, color.Item4));
        }

        (byte, byte, byte, byte) GetPixelAt(Vector2 worldPosition)
        {
            Vector2 texturePosition = TopLeftToBottomLeft(worldPosition);
            Color32 c = texture.GetPixel((int)texturePosition.x, (int)texturePosition.y);
            return (c.r, c.g, c.b, c.a);
        }

        public (byte, byte, byte, byte) GetAt((int, int) coords)
        {
            Vector2 pos = (new Vector2(coords.Item1, coords.Item2));
            return GetPixelAt(pos);
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

        public void SurfaceColor(Color32 color)
        {
            for (int x = 0; x < _surface.transform.localScale.x; x++)
            {
                for (int y = 0; y < _surface.transform.localScale.y; y++)
                {
                    SetPixelAt(new Vector2(x, y), color);
                }
            }

            Apply();
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


        public void Apply()
        {
            texture.Apply();
        }

        public void Scroll(int x, int y, bool texture = false)
        {
            if (scrollCor != null) return;
            scrollCor = ti.StartCoroutine(texture ? _ScrollTexture() : _ScrollTransform());

            IEnumerator _ScrollTexture()
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

            IEnumerator _ScrollTransform()
            {
                Vector2 scrollOffset = Vector2.zero;
                while (true)
                {
                    yield return new WaitForEndOfFrame();

                    scrollOffset.x = x * Time.deltaTime;
                    scrollOffset.y = y * Time.deltaTime;

                    _surface.transform.Translate(scrollOffset);

                    //if (Mathf.Abs(_surface.transform.position.x) >= 800) _surface.transform.position = new Vector3(0, 0, 0);
                }

                ti.StopCoroutine(scrollCor);
                yield return null;
            }
        }

        public void StopScroll()
        {
            ti.StopCoroutine(scrollCor);
        }
    }
}

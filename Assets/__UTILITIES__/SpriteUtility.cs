using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtility
{
    SpriteRenderer _spriteRenderer;
    Vector3 _spriteSize;
    Texture2D _newTexture;
    Sprite _newSprite;
    Material _material;

    public enum SpritePosition { LEFT, RIGHT, TOP, BOTTOM };

    public Texture2D t2;

    public SpriteUtility(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
        _spriteSize = GetSpriteSize();
        _material = _spriteRenderer.material;

        InitSpriteAndTexureForPixelManipulation();
    }

    private void InitSpriteAndTexureForPixelManipulation()
    {
        var sprite = _spriteRenderer.sprite;
        _newTexture = new Texture2D(sprite.texture.width, sprite.texture.height);
        _newTexture.filterMode = FilterMode.Point;
        _newTexture.wrapMode = TextureWrapMode.Clamp; // important else texture shows strange lines
        _newTexture.SetPixels(sprite.texture.GetPixels());
        _newTexture.Apply();
        _newSprite = Sprite.Create(_newTexture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), sprite.pivot.normalized, sprite.pixelsPerUnit);
        _spriteRenderer.sprite = _newSprite;
    }

    public float GetSpriteSidePosition(SpritePosition spritePosition)
    {
        Vector3 centerPos = _spriteRenderer.bounds.center;
        float position;

        switch (spritePosition)
        {
            case SpritePosition.LEFT: position = centerPos.x - _spriteSize.x / 2; break;
            case SpritePosition.RIGHT: position = centerPos.x + _spriteSize.x / 2; break;
            case SpritePosition.TOP: position = centerPos.y + _spriteSize.y / 2; break;
            case SpritePosition.BOTTOM: position = centerPos.y - _spriteSize.y / 2; break;
            default: position = 0; break;
        }

        return position;
    }

    public Vector3 GetSpriteSize() { return _spriteRenderer.bounds.size; }

    // http://toqoz.fyi/unity-sprite-texture-coordinates.html
    // adapted by myself --
    public Vector2 WorldPosToLocalTexturePos(Vector3 worldPos)
    {
        var sprite = _spriteRenderer.sprite;

        float ppu = sprite.pixelsPerUnit;

        // Local position on the sprite in pixels.
        Vector2 localPos = _spriteRenderer.gameObject.transform.InverseTransformPoint(worldPos) * ppu;

        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;
        Vector2 texSpaceCoord = texSpacePivot + localPos;

        return texSpaceCoord;
    }

    // given the current sprite, convert worldPos of another gameobject
    // into current sprite UV cooord
    public Vector2 WorldPosToLocalTexturePosUV(Vector3 worldPos)
    {
        var sprite = _spriteRenderer.sprite;

        Texture2D tex = sprite.texture;
        Vector2 texSpaceCoord = WorldPosToLocalTexturePos(worldPos);

        // Pixels to UV(0-1) conversion.
        Vector2 uvs = texSpaceCoord;
        uvs.x /= tex.width;
        uvs.y /= tex.height;

        return uvs;
    }

    public enum PivotPosition
    {
        CENTER,
        TOPLEFT,
        TOPCENTER,
        TOPRIGHT,
        LEFTCENTER,
        RIGHTCENTER,
        BOTTOMLEFT,
        BOTTOMCENTER,
        BOTTOMRIGHT,
        CUSTOM
    }

    public void SetPivot(PivotPosition pivotPosition, Vector2 custom = default(Vector2))
    {
        float x = 0, y = 0;
        switch (pivotPosition)
        {
            case PivotPosition.CENTER: x = 0.5f; y = 0.5f; break;
            case PivotPosition.TOPLEFT: x = 0f; y = 1f; break;
            case PivotPosition.TOPCENTER: x = 0.5f; y = 1f; break;
            case PivotPosition.TOPRIGHT: x = 1f; y = 1f; break;
            case PivotPosition.LEFTCENTER: x = 0f; y = 0.5f; break;
            case PivotPosition.RIGHTCENTER: x = 1f; y = 0.5f; break;
            case PivotPosition.BOTTOMLEFT: x = 0f; y = 0f; break;
            case PivotPosition.BOTTOMCENTER: x = 0.5f; y = 0f; break;
            case PivotPosition.BOTTOMRIGHT: x = 1f; y = 0f; break;
            case PivotPosition.CUSTOM: x = custom.x; y = custom.y; break;
        }

        Vector2 newPivot = new Vector2(x, y);

        var sprite = _spriteRenderer.sprite;
        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp; // important else texture shows strange lines
        texture.SetPixels(sprite.texture.GetPixels());
        texture.Apply();
        var newSprite = Sprite.Create(texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), newPivot, sprite.pixelsPerUnit);
        _spriteRenderer.sprite = newSprite;
    }

    public PivotPosition GetPivotPosition()
    {
        var sprite = _spriteRenderer.sprite;

        // this makes the stuff because sprite.pivot returns datas in pixels not as normalized value
        Bounds bounds = sprite.bounds;
        var pivotX = -bounds.center.x / bounds.extents.x / 2 + 0.5f;
        var pivotY = -bounds.center.y / bounds.extents.y / 2 + 0.5f;
        //------------------

        Vector2 pivotNormalized = new Vector2(pivotX, pivotY);

        if (pivotNormalized.x == 0.5f && pivotNormalized.y == 0.5f) return PivotPosition.CENTER;
        else if (pivotNormalized.x == 0f && pivotNormalized.y == 1f) return PivotPosition.TOPLEFT;
        else if (pivotNormalized.x == 0.5f && pivotNormalized.y == 1f) return PivotPosition.TOPCENTER;
        else if (pivotNormalized.x == 1f && pivotNormalized.y == 1f) return PivotPosition.TOPRIGHT;
        else if (pivotNormalized.x == 0f && pivotNormalized.y == 0.5f) return PivotPosition.LEFTCENTER;
        else if (pivotNormalized.x == 1f && pivotNormalized.y == 0.5f) return PivotPosition.RIGHTCENTER;
        else if (pivotNormalized.x == 0f && pivotNormalized.y == 0f) return PivotPosition.BOTTOMLEFT;
        else if (pivotNormalized.x == 0.5f && pivotNormalized.y == 0f) return PivotPosition.BOTTOMCENTER;
        else if (pivotNormalized.x == 1f && pivotNormalized.y == 0f) return PivotPosition.BOTTOMRIGHT;
        else return PivotPosition.CUSTOM;
    }

    [Obsolete("WorldToPixelPoint is deprecated, please use WorldPosToLocalTexturePos instead.")]
    public Vector2 WorldToPixelPoint(Vector2 worldPosition)
    {
        var sprite = _spriteRenderer.sprite;

        Texture2D tex = sprite.texture;

        Vector2 pixelPosition = Vector2.zero;

        Vector2 localPosition = Vector2.zero;

        localPosition.x = worldPosition.x - _spriteRenderer.gameObject.transform.position.x;
        localPosition.y = worldPosition.y - _spriteRenderer.gameObject.transform.position.y;

        // When the sprite is part of an atlas, the rect defines its offset on the texture.
        // When the sprite is not part of an atlas, the rect is the same as the texture (x = 0, y = 0, width = tex.width, ...)
        var texSpacePivot = new Vector2(sprite.rect.x, sprite.rect.y) + sprite.pivot;

        pixelPosition = localPosition + texSpacePivot;

        return pixelPosition;
    }

    /// <summary>
    /// Get the pixel color inside this renderer, based on the coordinates of the texture itself
    /// </summary>
    /// <param name="posX">X coord</param>
    /// <param name="posY">Y coord</param>
    /// <returns></returns>
    public Color GetPixelAt(int posX, int posY)
    {
        return _spriteRenderer.sprite.texture.GetPixel(posX, posY);
    }

    /// <summary>
    /// Get the pixel color inside this renderer, based on the coordinates of the world position
    /// </summary>
    /// <param name="worldPosition">the world position</param>
    /// <returns></returns>
    public Color GetPixelAt(Vector2 worldPosition)
    {
        Vector2 texturePosition = WorldPosToLocalTexturePos(worldPosition);
        return _spriteRenderer.sprite.texture.GetPixel((int)texturePosition.x, (int)texturePosition.y);
    }

    [Obsolete("SetPixelAt(SpriteRenderer spriteRenderer, Vector2 worldPosition, Color c) is deprecated, please use public void SetPixelAt(Vector2 worldPosition, Color c) instead.")]
    public void SetPixelAt(SpriteRenderer spriteRenderer, Vector2 worldPosition, Color c)
    {
        var sprite = spriteRenderer.sprite;
        Vector2 texturePosition = WorldPosToLocalTexturePos(worldPosition);
        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp; // important else texture shows strange lines
        texture.SetPixels(sprite.texture.GetPixels());
        texture.Apply();
        texture.SetPixel((int)worldPosition.x, (int)worldPosition.y, c);
        texture.Apply();
        var newSprite = Sprite.Create(texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), sprite.pivot.normalized, sprite.pixelsPerUnit);
        spriteRenderer.sprite = newSprite;
    }

    // TO REMOVE
    /*public void SetPixelAt(Vector2 worldPosition, Color c, bool batch = false)
    {
        var sprite = _spriteRenderer.sprite;
        Vector2 texturePosition = WorldPosToLocalTexturePos(worldPosition);

        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp; // important else texture shows strange lines
        texture.SetPixels(sprite.texture.GetPixels());
        texture.Apply();
        texture.SetPixel((int)texturePosition.x, (int)texturePosition.y, c);
        texture.Apply();
        var newSprite = Sprite.Create(texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), sprite.pivot.normalized, sprite.pixelsPerUnit);
        _spriteRenderer.sprite = newSprite;
    }*/

    public void SetPixelAt(Vector2 worldPosition, Color c)
    {
        Vector2 texturePosition = WorldPosToLocalTexturePos(worldPosition);
        _newTexture.SetPixel((int)texturePosition.x, (int)texturePosition.y, c);
   
    }

    public void Apply()
    {
        _newTexture.Apply();
    }


    public void ErasePixels(Vector2 worldPosition, Sprite shape)
    {
        var sprite = _spriteRenderer.sprite;
        Vector2 texturePosition = WorldPosToLocalTexturePos(worldPosition);
        Texture2D texture = new Texture2D(sprite.texture.width, sprite.texture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp; // important else texture shows strange lines
        texture.SetPixels(sprite.texture.GetPixels());
        texture.Apply();

        var newSprite = Sprite.Create(texture, new Rect(0, 0, sprite.texture.width, sprite.texture.height), sprite.pivot.normalized, sprite.pixelsPerUnit);

        for (int x = 0; x < shape.texture.width; x++)
        {
            for (int y = 0; y < shape.texture.height; y++)
            {
                Color c = shape.texture.GetPixel(x, y);

                if (c.a == 1)
                    texture.SetPixel((int)texturePosition.x + x, (int)texturePosition.y - shape.texture.height + y, Color.clear);
            }
        }

        texture.Apply();
        _spriteRenderer.sprite = newSprite;

        // if there is a collider, recreate it after new texture setting, so the collider adapts to the new shape of the texure
        PolygonCollider2D coll = _spriteRenderer.gameObject.GetComponent<PolygonCollider2D>();
        if (coll != null)
        {
            GameObject.Destroy(_spriteRenderer.gameObject.GetComponent<PolygonCollider2D>());
            _spriteRenderer.gameObject.AddComponent<PolygonCollider2D>();
        }
    }

    public void SetSortingOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }

    public void Scroll(float speedX, int speedY)
    {
        //if(_material.GetTextureOffset("_MainTex").x >= 1)
          //  _material.SetTextureOffset("_MainTex", new Vector2(0, 0));

        _material.SetTextureOffset("_MainTex", new Vector2(speedX, 0));
    }
}

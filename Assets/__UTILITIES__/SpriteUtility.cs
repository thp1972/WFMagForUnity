using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteUtility
{
    SpriteRenderer _spriteRender;
    Vector3 _spriteSize;

    public enum SpritePosition { LEFT, RIGHT, TOP, BOTTOM };

    public SpriteUtility(SpriteRenderer spriteRenderer)
    {
        _spriteRender = spriteRenderer;
        _spriteSize = GetSpriteSize();
    }

    public float GetSpriteSidePosition(SpritePosition spritePosition)
    {
        Vector3 centerPos = _spriteRender.bounds.center;
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

    public Vector3 GetSpriteSize() { return _spriteRender.bounds.size; }
}

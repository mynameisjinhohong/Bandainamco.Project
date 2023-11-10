using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : ManagerBase
{
    public static DataManager Instance;
    [SerializeField] private SpriteRenderer bg;
    public Vector3 bgSize;

    public override void Init()
    {
        Instance = this;
        bgSize = GetBGSize();
        base.Init();
    }


    private Vector3 GetBGSize()
    {
        Vector2 bgSpriteSize = bg.sprite.rect.size;
        Vector2 localbGSize = bgSpriteSize / bg.sprite.pixelsPerUnit;
        Debug.Log("bgSpriteSize : " + bgSpriteSize);
        Debug.Log("bgSpriteSize / pixelsPerUnit : " + localbGSize);
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bg.transform.lossyScale.x;
        worldbGSize.y *= bg.transform.lossyScale.y;
        Debug.Log("bgSize : " + worldbGSize);
        return worldbGSize;
    }
}

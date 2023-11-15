using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusItem_LJH : BaseItem_LJH
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        ItemManager_LJH.Instance.SetLotusShield(true);
    }

    public override void Reset()
    {
        base.Reset();
    }
}

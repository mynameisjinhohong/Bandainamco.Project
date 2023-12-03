using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusItem_LJH : BaseItem_LJH
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TagStrings.PlayerTag) == false) return;
        if (!myItem.isVisited) ItemManager_LJH.Instance.StartLotusPetal();
        base.OnTriggerEnter2D(other);
        ItemManager_LJH.Instance.SetLotusShield(true);    
    }

    public override void Reset()
    {
        base.Reset();
    }
}

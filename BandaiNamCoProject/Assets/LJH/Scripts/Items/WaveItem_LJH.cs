using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveItem_LJH : BaseItem_LJH
{
    [SerializeField] private float zoomLerpSec = 3f;
    [SerializeField] private int delayTime = 3;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(other);
            //직접적인 아이템 기능
            ItemManager_LJH.Instance.SetWave(() =>
            {
                //Wave Finish callback
                //ItemManager_LJH.Instance.SetBubble(false);
                //WorldManager.Instance.NotifyItemEffect(ItemType.Wave, false);
                //gameObject.SetActive(false);
            });
            ItemManager_LJH.Instance.SetBubble(true);

            //점프 쿨타임 감소, 중력 제거 등 효과
            WorldManager.Instance.NotifyItemEffect(myItem.itemType, true);
        }
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveItem_LJH : BaseItem_LJH
{
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //�������� ������ ���
            ItemManager_LJH.Instance.SetWave(() =>
            {
                //Wave Finish callback
                //ItemManager_LJH.Instance.SetBubble(false);
                //WorldManager.Instance.NotifyItemEffect(ItemType.Wave, false);
                //gameObject.SetActive(false);
            });
            ItemManager_LJH.Instance.SetBubble(true);

            //���� ��Ÿ�� ����, �߷� ���� �� ȿ��
            WorldManager.Instance.NotifyItemEffect(myItem.itemType, true);
            base.OnTriggerEnter2D(other);
        }
    }
}

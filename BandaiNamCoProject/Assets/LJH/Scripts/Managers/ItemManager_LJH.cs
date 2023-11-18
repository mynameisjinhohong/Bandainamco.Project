using Bitgem.VFX.StylisedWater;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ItemManager_LJH : ManagerBase
{
    [SerializeField] private List<BaseItem_LJH> spawnItems;
    [SerializeField] private Transform itemParent;
    public BaseBackground_LJH[] backgrounds;
    public Item_HJH[] items;
    public float itemsDistance;
    public GameObject player;
    public int itemCount;
    public float xyLine; //�׸� ���̶� �ʹ� ����� �ʰ� �ϱ� ���ؼ�
    public static ItemManager_LJH Instance;
    public GameObject bubble;

    //yd ���� ����
    [SerializeField] private List<GameObject> mushroomImage = new List<GameObject>();

    private BaseItem_LJH currItem;
    public BaseItem_LJH CurrItem
    {
        get { return currItem; }
        set
        {
            prevItem = currItem;
            currItem = value;
        }
    }
    public BaseItem_LJH prevItem;


    public BaseBackground_LJH currBackground;

    [Header("Wave")]
    public WaveCollider_LJH wave;
    public Transform bubbleParent;
    private List<Bubble_LJH> bubbles;


    [Header("Lotus")]
    public LotusParticle_LJH lotus;
    public GameObject lotusShield;

    Vector3 Return_RandomPosition()
    {
        float x = UnityEngine.Random.Range(-DataManager.Instance.bgSize.x / 2 + itemsDistance, DataManager.Instance.bgSize.x / 2 - xyLine);
        float y = UnityEngine.Random.Range(-DataManager.Instance.bgSize.y / 2 + itemsDistance, DataManager.Instance.bgSize.y / 2 - xyLine);
        Vector3 randomPostion = new Vector3(x, y, 0);
        return randomPostion;
    }

/*    public int RandomMushroom()
    {
        int mu = UnityEngine.Random.Range(0, mushroomImage.Count);
        return mu;
    }*/

    public async override void Init()
    {
        await UniTask.WaitUntil(() => DataManager.Instance.isInit);

        Instance = this;

        bubbles = new List<Bubble_LJH>();
        bubbles.AddRange(bubbleParent.GetComponentsInChildren<Bubble_LJH>(true));

        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].itemCount; j++)
            {/*
                if(items[i].prefab.name.Contains("Mushroom"))
                    {
                    GameObject item = Instantiate(i)
                }*/
                GameObject item = Instantiate(items[i].prefab);
                GameObject bub = Instantiate(bubble);
                Vector3 pos;
                int su = 0; //���ѷ��� ������
                while (true)
                {
                    su++;
                    pos = Return_RandomPosition();
                    bool restart = false;
                    for (int k = 0; k < spawnItems.Count; k++)
                    {
                        if ((pos - spawnItems[k].transform.position).magnitude < itemsDistance)
                        {
                            restart = true;
                            break;
                        }
                    } // �ٸ� �������̶� �ʹ� ����� ��
                    if ((pos - player.transform.position).magnitude < itemsDistance)
                    {
                        restart = true;
                    } //�÷��̾�� �ʹ� ����� ��
                    if (su > 1000)
                    {
                        restart = false;
                        Debug.Log("������ ������ �Ǿ� ���ȴ�");
                    } // �ʹ� ���� �ݺ��� ��
                    if (!restart)
                    {
                        break;
                    }
                }
                item.transform.position = pos;
                item.transform.parent = itemParent;
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.parent.position.z);
                bub.transform.position = item.transform.position;
                bub.transform.parent = item.transform;
                BaseItem_LJH baseItem = item.GetComponent<BaseItem_LJH>();
                spawnItems.Add(baseItem);
                baseItem.bubble = bub;
                baseItem.myItem = items[i];

            }
        }
        base.Init();
    }

    public override void Reset()
    {
        if (prevItem != null) prevItem.Reset();
        //if (currBackground != null) currBackground.Reset();
        base.Reset();
    }

    public void SetActiveItems(bool isActive)
    {
        foreach (var i in spawnItems)
        {
            if (isActive)
            {
                if (GamePlayManager_HJH.Instance.ContainItem(i))
                {
                    i.gameObject.SetActive(false);
                    continue;
                }
            }
            i.gameObject.SetActive(isActive);
        }
    }

    public void SetWave(Action callback = null)
    {
        wave.StartWave();
    }

    public void SetBubble(bool isSet)
    {
        if (isSet)
        {
            foreach (var bubble in bubbles)
                bubble.StartBubble();
        }
        else
        {
            foreach (var bubble in bubbles)
                bubble.FinishBubble();
        }
    }

    public void SetLotus(bool start)
    {
        if (start)
            lotus.Play();
        else
            lotus.Stop();
    }

    public void SetLotusShield(bool start)
    {
        lotusShield.SetActive(start);
    }
}

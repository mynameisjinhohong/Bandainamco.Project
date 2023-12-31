using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem_LJH : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private bool itemsOff;
    public Item_HJH myItem;
    public GameObject bubble;
    protected bool already = false;
    public void Init(Item_HJH item)
    {
        myItem = item;
    }


    public virtual async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !already)
        {
            ItemManager_LJH.Instance.itemCount += 1;
            ItemManager_LJH.Instance.CurrItem = this;
            GamePlayManager_HJH.Instance.AddConsumedItem(this);

            if (itemSprite == null)
                GetComponent<SpriteRenderer>().enabled = false;
            else
                itemSprite.enabled = false;

            Animator bubbleAni;
            if (bubble.TryGetComponent<Animator>(out bubbleAni))
            {
                bubbleAni.SetTrigger("Pop");
            }
            AudioSource bubbleAudio = bubble.GetComponent<AudioSource>();
            bubbleAudio.Play();
            await UniTask.WaitUntil(() => !bubbleAudio.isPlaying);

            if (itemsOff)
                WorldManager.Instance.NotifyItemEffect(myItem.itemType, true);

            if (!myItem.isVisited)
            {
                WorldManager.Instance.MainState = MainState.Pause;
                if (myItem.needWholeCam)
                {
                    await CameraManager.Instance.CameraControlAfterItem(myItem.itemType.ToString(), true);
                }
                else
                {
                    await CameraManager.Instance.CameraControlAfterItem(myItem.itemType.ToString(), false);

                }
            }
            myItem.isVisited = true;
            ActiveFalse();
            already = true;
        }
    }
    async void ActiveFalse()
    {
        await UniTask.Delay(800, true);
        gameObject.SetActive(false);
    }

    public virtual void Reset() { }
}

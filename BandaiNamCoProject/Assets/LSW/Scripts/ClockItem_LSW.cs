using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ClockItem_LSW : BaseItem_LJH
{
    public float coolTimeReduce = 0.5f;
    public GameObject watchPrefab;
    //  public 
    public float rotSpeed = 10;
    public bool isRot;
    public float clockSizeTime = 1; //시계 작아지는 시간
    public float scale = 2;
    Animator anim;
    public GameObject obj;
    Vector3 oriScale = new Vector3(5, 5, 5);

    private WatchSecond_yd clockAnim;

    private void Awake()
    {
        obj = GameObject.FindWithTag("Clock");
        if (obj != null)
            clockAnim = obj.GetComponentInChildren<WatchSecond_yd>(true);
    }

    public override async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(other);
            await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);
            StartClock();
        }
    }

    public async void StartClock()
    {
        //카메라가 배경->플레이어로 이동하는 시간때문에 어쩔수 없이 Delay 추가
        await UniTask.Delay(400);

        GamePlayManager_HJH.Instance.characterMovement2D.coolTime -= coolTimeReduce;

        clockAnim.gameObject.SetActive(true);

        bool isFinished = false;

        clockAnim.StartClock(() =>
            {
                isFinished = true;
            });

        await UniTask.WaitUntil(() => isFinished);

        clockAnim.gameObject.SetActive(false);

        // if(ItemManager_LJH.Instance.)
        //await UniTask.Delay(1);
        //obj.transform.GetChild(0).gameObject.SetActive(true);
        //await UniTask.Delay(2 * 1000);
        //obj.transform.GetChild(0).gameObject.SetActive(false);


    }
}




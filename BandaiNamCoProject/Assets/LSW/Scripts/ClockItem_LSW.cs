using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ClockItem_LSW : BaseItem_LJH
{
    public float coolTimeReduce = 0.5f;
    //public GameObject watchPrefab;
    //  public 
   // public float rotSpeed = 10;
   // public bool isRot;
    //public float clockSizeTime = 1; //시계 작아지는 시간
  //  public float scale = 2;
    Animator anim;
    GameObject obj;
    Vector3 oriScale = new Vector3(5, 5, 5);
    public void Start()
    {
       // anim = watchPrefab.GetComponent<Animator>();
        //Vector3 oriScale = watchPrefab.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale;

    }
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Watch();
        }
        base.OnTriggerEnter2D(other);
    }

    public async void Watch()
    {
        await UniTask.Delay(1);
        obj.transform.GetChild(0).gameObject.SetActive(true);
         await UniTask.Delay(2*1000);
        obj.transform.GetChild(0).gameObject.SetActive(false);


        GamePlayManager_HJH.Instance.characterMovement2D.coolTime -= coolTimeReduce;

    }
    

    public void Update()
    {
        obj = GameObject.FindWithTag("Clock");
 

    }
}
    



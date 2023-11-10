using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mashroom_yd : BaseItem_LJH
{
    //크기
    public float scale = 2;
    //원래크기로 돌아오는 시간
    public int resetTime = 2;

    [SerializeField] private Vector3 originalScale;
    public bool isScale = false;
  //  [SerializeField] private int mashroomTrigger = 0;
    [SerializeField] private float mashroomTime = 0f;
    Transform tr;
    public GameObject mashroomEffect;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player") && !isScale)
        //{
        //    if (mashroomTrigger == 0)
        //    {
        //        mashroomTrigger = 1;
        //        originalScale = collision.transform.localScale;
        //        isScale = true;
        //        tr = collision.transform;
        //       StartCoroutine(PlayerScale(collision.transform));
        //    }
        //}
        base.OnTriggerEnter2D(collision);
        //StartCoroutine(PlayerScale(collision.transform,scale,resetTime));
        PlayerScale(collision.transform, scale, resetTime);
    }

    public async void PlayerScale(Transform targetTr, float scale, int resetTime)
    {
        await UniTask.Delay(1);

        Vector3 originalScale = targetTr.localScale;
        Vector3 targetScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
        float currentTime = 0;
        targetTr.localScale = targetScale;
        //GameObject me = Instantiate(mashroomEffect, targetTr.transform.parent, );
        // Vector3 newPosition = new Vector3(0, 1, -8.0f);
        GameObject me = Instantiate(mashroomEffect, targetTr.transform);
       // me.transform.parent = targetTr.transform;
        Debug.Log(me.transform.position);
        while (currentTime < mashroomTime)
        {
            targetTr.localScale = Vector3.Lerp(originalScale, targetScale,  currentTime / mashroomTime);
            currentTime += Time.deltaTime;
            //Debug.Log("커짐");
            await UniTask.Yield();
        }
        await UniTask.Delay(resetTime * 1000);
        //yield return new WaitForSeconds(resetTime);
        //Debug.Log("유지시간");
        currentTime = 0f;

        while (currentTime < mashroomTime)
        {
            targetTr.localScale = Vector3.Lerp(targetScale, originalScale, currentTime / mashroomTime);
            currentTime += Time.deltaTime;
            await UniTask.Yield();
            //yield return null;
            Debug.Log("작아짐");

        }
    }
   
  
    // Start is called before the first frame update
}

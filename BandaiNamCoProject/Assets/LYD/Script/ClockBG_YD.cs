using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class ClockBG_YD : MonoBehaviour
{
    private float currentTime = 0;
    public float secondTime = 2; //초침이 돌아가는시간
    public GameObject second;
    public GameObject min;
    public float bgRotTime = 1; //배경이 회전하는 시간
    public float speed = 30;
    public GameObject clock;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnEnable()
    {
        ClockBackground();
    }
    public async void ClockBackground()
    {
        while(true)
        {
            //await BGRotate(second, 90, secondTime);
            await BGRotate(clock, 90, bgRotTime);
            //await BGRotate(second, 180, secondTime);
            await BGRotate(clock, 180, bgRotTime);
            //await BGRotate(second, 270, secondTime);
            await BGRotate(clock, 270, bgRotTime);
            //await BGRotate(second, 360, secondTime);
            await BGRotate(clock, 360, bgRotTime);
            /* //초침이 돌아간다 -> 90도로
             while (currentTime < secondTime)
             {
                 Quaternion ori = second.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 90);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 second.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;
             while (currentTime < bgRotTime)
             {
                 Quaternion ori = clock.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 90);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 clock.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;

             while (currentTime < secondTime)
             {
                 Quaternion ori = second.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 180);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 second.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;
             while (currentTime < bgRotTime)
             {
                 Quaternion ori = clock.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 180);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 clock.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;

             while (currentTime < secondTime)
             {
                 Quaternion ori = second.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 270);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 second.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;
             while (currentTime < bgRotTime)
             {
                 Quaternion ori = clock.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 270);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 clock.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;

             while (currentTime < secondTime)
             {
                 Quaternion ori = second.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 360);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 second.transform.rotation = now;
                 await UniTask.Yield();
             }
             currentTime = 0;
             while (currentTime < bgRotTime)
             {
                 Quaternion ori = clock.transform.rotation;
                 Quaternion target = Quaternion.Euler(0, 0, 360);
                 Quaternion now = Quaternion.Lerp(ori, target, speed * Time.deltaTime);
                 clock.transform.rotation = now;
                 await UniTask.Yield();
             }*/
        }
       
    }

    async UniTask BGRotate(GameObject ob, float targetE, float obTime)
    {
        currentTime = 0;
        Quaternion ori = ob.transform.rotation;
        Quaternion target = Quaternion.Euler(0, 0, targetE);
        while(currentTime < obTime)
        {
            Quaternion now = Quaternion.Lerp(ori, target, currentTime/obTime);
            Debug.Log(currentTime/obTime);
            ob.transform.rotation = now;
            currentTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

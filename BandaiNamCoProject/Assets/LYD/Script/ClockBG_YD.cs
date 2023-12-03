using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;


public class ClockBG_YD : MonoBehaviour
{
    private float currentTime = 0;
    public float bgRotTime = 1; //배경이 회전하는 시간
    public GameObject clock;
    Animator animator;
    public GameObject[] options;
    public float waitTime = 2;
    public AudioSource clockSound;
    public AudioSource clockMoveSound;
    // Start is called before the first frame update
    void Start()
    {
        animator = transform.GetComponent<Animator>();
        animator.enabled = false;
    }
    public void OnEnable()
    {
        ClcockBackGround();
    }
    public async void ClcockBackGround()
    {
        await UniTask.Delay(400);
        animator.enabled = true;
    }
    public void ClockSound()
    {
        clockSound.Play();

    }
    public void Clock()
    {
        BGRotate(clock, 90, bgRotTime, "First");
        clockMoveSound.Play();
    }
    public void Clock1()
    {
        BGRotate(clock, 180, bgRotTime, "Second");
     //   clockSound.Play();
        clockMoveSound.Play();

    }
    public void Clock2()
    {
        BGRotate(clock, 270, bgRotTime, "Third");
   //     clockSound.Play();
        clockMoveSound.Play();

    }
    public void Clock3()
    {
      BGRotate(clock, 360, bgRotTime, "Ori");
     //   clockSound.Play();
        clockMoveSound.Play();

    }
    /* public async void ClockBackground()
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
              //초침이 돌아간다 -> 90도로

         }

     }*/

    /* async UniTask BGRotate(GameObject ob, float targetE, float obTime)
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
     }*/
    public void BGRotate(GameObject ob, float targetE, float obTime, string anim)
    {
        StartCoroutine(BGRotateEvent(ob, targetE, obTime, anim));
    }
    IEnumerator BGRotateEvent(GameObject ob, float targetE, float obTime, string anim)
    {
        ItemManager_LJH.Instance.isClockRotating = true;

        Time.timeScale = 0;
        currentTime = 0;
        Quaternion ori = ob.transform.rotation;
        Quaternion target = Quaternion.Euler(0, 0, targetE);
        while (currentTime < obTime)
        {
            Quaternion now = Quaternion.Lerp(ori, target, currentTime / obTime);
            //Debug.Log(currentTime / obTime);
            ob.transform.rotation = now;
            currentTime += Time.unscaledDeltaTime;
            yield return null;

        }
        ItemManager_LJH.Instance.isClockRotating = false;

        bool optionOff = true;
        for(int i =0; i<options.Length; i++)
        {
            if (options[i].activeInHierarchy)
            {
                optionOff = false;
            }
        }
        if(optionOff)
        {
            Time.timeScale = 1;
        }
        yield return new WaitForSeconds(waitTime);
        yield return null;
        animator.SetTrigger(anim);

    }
    /*  public void BGRotate(GameObject ob, float targetE, float obTime)
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

         }
     }*/

}

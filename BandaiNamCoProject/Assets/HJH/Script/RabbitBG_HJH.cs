using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBG_HJH : MonoBehaviour
{
    public Animator rabbitAni;
    public float aniTime; //애니메이션 지속시간
    public float aniCoolTime; // 애니메이션 쿨타임
    public float noteTime; //음표 생성 시간
    public Transform noteParent;

    List<GameObject> notes = new List<GameObject>();

    float currentTime;
    float currentTime2;
    bool aniNow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!aniNow)
        {
            if (currentTime > aniCoolTime)
            {
                currentTime = 0;
                //애니메이션 시작하는 스크립트
                aniNow = true;
            }
        }
        else
        {
            currentTime2 += Time.deltaTime;
            if(currentTime2 > noteTime)
            {
                notes.Add(noteParent.GetChild(0).gameObject);
                noteParent.GetChild(0).GetComponent<SmallRabbit_HJH>().start = true;
                //noteParent.GetChild(0).GetComponent<SmallRabbit_HJH>().rabbitBg = this;
                noteParent.GetChild(0).position = ScreenRandomPos();
                noteParent.GetChild(0).gameObject.SetActive(true);
                currentTime2 = 0;
            }
            if (currentTime> aniTime)
            {
                int no = notes.Count;
                for(int i = 0; i < no; i++)
                {
                    notes[0].SetActive(false);
                    notes.RemoveAt(0);
                }
                currentTime = 0;
                aniNow = false;
            }
        }
    }


    Vector3 ScreenRandomPos()
    {
        float x = Random.Range(Camera.main.ViewportToWorldPoint(Vector3.right).x, Camera.main.ViewportToWorldPoint(Vector3.zero).x);
        float y = Random.Range(Camera.main.ViewportToWorldPoint(Vector3.up).y, Camera.main.ViewportToWorldPoint(Vector3.zero).y);
        Vector3 pos = new Vector3(x,y,0);
        return pos;
    }
    public void Touch()
    {

    }
}

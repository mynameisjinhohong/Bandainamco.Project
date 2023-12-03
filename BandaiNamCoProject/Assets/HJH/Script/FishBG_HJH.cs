using System.Collections;
using UnityEngine;

public class FishBG_HJH : MonoBehaviour
{
    public GameObject smallFish;
    float currentTime;
    public float minTime; //�ּ��� �� �ð� �Ŀ� ���� ���� ����
    public float maxTime; //�ּ��� �� �ð� ������ ����
    public Transform fishOrigin;
    public Transform fishTarget;
    Animator myAni;
    bool fishAni = false;
    public float waitFishAniTime;
    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (!fishAni)
        {
            if (currentTime > waitFishAniTime)
            {
                currentTime = 0;
                myAni.SetTrigger("Fish");
                fishAni = true;
            }
        }
    }

    public void MakeFishStart()
    {
        if(fishAni)
        {
            StopAllCoroutines();
            currentTime = 0;
            StartCoroutine(FishGo());
        }

    }

    IEnumerator FishGo()
    {
        while (true)
        {
            float nextTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(nextTime);
            MakeFish();
        }
    }

    public void MakeFishEnd()
    {
        fishAni = false;
        StopAllCoroutines();
        currentTime = 0;
    }

    public void MakeFish()
    {
        GameObject fish = Instantiate(smallFish);
        fish.transform.position = fishTarget.position;
        fish.transform.parent = transform.parent;
        fish.GetComponent<SmallFish_HJH>().originTransform = fishOrigin;
    }
}

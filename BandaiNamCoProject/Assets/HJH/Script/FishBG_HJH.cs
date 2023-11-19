using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBG_HJH : MonoBehaviour
{
    public GameObject smallFish;
    public float aniTime; //애니메이션에서 물고기 도는 시간
    float currentTime;
    public float minTime; //최소한 이 시간 후에 다음 생선 나옴
    public float maxTime; //최소한 이 시간 전에는 나옴
    public Transform fishOrigin;
    public Transform fishTarget;
    bool start = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            currentTime += Time.deltaTime;
        }
    }
    
    public void MakeFishStart()
    {
        start = true;
        StopAllCoroutines();
        currentTime = 0;
        StartCoroutine(FishGo());
    }

    IEnumerator FishGo()
    {
        while (currentTime < aniTime)
        {
            float nextTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(nextTime);
            MakeFish();
        }
        start = false;
    }

    public void MakeFish()
    {
        GameObject fish = Instantiate(smallFish);
        fish.transform.position = fishTarget.position;
        fish.transform.parent = transform.parent;
        fish.GetComponent<SmallFish_HJH>().originTransform = fishOrigin;
    }
}

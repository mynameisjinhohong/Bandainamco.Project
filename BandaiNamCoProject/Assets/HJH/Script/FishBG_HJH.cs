using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBG_HJH : MonoBehaviour
{
    public GameObject smallFish;
    public float aniTime; //�ִϸ��̼ǿ��� ����� ���� �ð�
    float currentTime;
    public float minTime; //�ּ��� �� �ð� �Ŀ� ���� ���� ����
    public float maxTime; //�ּ��� �� �ð� ������ ����
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

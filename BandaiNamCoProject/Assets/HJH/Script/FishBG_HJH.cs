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

    async public void MakeFishStart()
    {
        start = true;
        while(currentTime < aniTime)
        {
            float nextTime = Random.Range(minTime,maxTime);
            await UniTask.Delay((int)(nextTime * 1000));
            MakeFish();
        }
        currentTime = 0;
        start = false;
    }

    public void MakeFish()
    {
        GameObject fish = Instantiate(smallFish);
        fish.transform.position = transform.position;
        fish.transform.parent = transform.parent;
        fish.GetComponent<SmallFish_HJH>().originTransform = fishOrigin;
    }
}

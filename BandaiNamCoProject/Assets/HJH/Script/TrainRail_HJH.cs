using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainRail_HJH : MonoBehaviour
{
    public bool start = false;
    public float railUpSpeed = 10f;
    public float railMaxY;
    public int railSu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            transform.localPosition += new Vector3(0, railUpSpeed * Time.deltaTime, 0);
            if(transform.localPosition.y > railMaxY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, railMaxY,transform.localPosition.z);
                start = false;
            }
        }
    }
    public async void StartMove()
    {
        await UniTask.Delay((railSu * 200));
        start = true;
    }

}

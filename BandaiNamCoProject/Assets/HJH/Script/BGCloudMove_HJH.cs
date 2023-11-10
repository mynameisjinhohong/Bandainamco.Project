using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCloudMove_HJH : MonoBehaviour
{
    public float startX;
    public float endX;
    public bool vertical;
    bool up = true;
    float currentTime = 0;
    public float coolTime = 1;
    //float speed;
    // Start is called before the first frame update
    void Start()
    {
        //speed = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (vertical)
        {
            if (up)
            {
                transform.position += Vector3.up * Time.deltaTime;
                currentTime += Time.deltaTime;
                if(currentTime > coolTime)
                {
                    currentTime = 0;
                    up = false;
                }
            }
            else
            {
                transform.position += Vector3.down * Time.deltaTime;
                currentTime += Time.deltaTime;
                if (currentTime > coolTime)
                {
                    currentTime = 0;
                    up = true;
                }
            }
        }
        else
        {
            transform.position += Vector3.right * 5 * Time.deltaTime;
            if (transform.position.x > endX)
            {
                transform.position = new Vector3(startX, transform.position.y, transform.position.z);
            }
        }
    }
}

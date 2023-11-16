using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFrame_HJH : MonoBehaviour
{
    public int myStage;
    bool one = false;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance != null)
        {
            for (int i = 0; i < GameManager.instance.userData.stageDatas[myStage].itemOnOff.Length; i++)
            {
                if (GameManager.instance.userData.stageDatas[myStage].itemOnOff[i])
                {
                    one = true;
                    transform.GetChild(i+1).gameObject.SetActive(true);
                }
            }
        }
        if (one)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

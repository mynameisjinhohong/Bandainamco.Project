using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBG_HJH : MonoBehaviour
{
    public Animator rabbitAni;
    public float aniTime; //애니메이션 지속시간
    public float aniCoolTime; // 애니메이션 쿨타임
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBG_HJH : MonoBehaviour
{
    public Animator rabbitAni;
    public float aniTime; //�ִϸ��̼� ���ӽð�
    public float aniCoolTime; // �ִϸ��̼� ��Ÿ��
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

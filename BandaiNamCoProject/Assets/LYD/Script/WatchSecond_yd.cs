using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchSecond_yd : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject second;
    public GameObject pos;
    public GameObject watch;
    public float rotSpeed = 10;
    public bool isRot;
    float targetRotation = 0;
    float currentRotation;
    void Start()
    {
        //currentRotation = second.transform.rotation.eulerAngles.z;
    }
    int num = 0;
    // Update is called once per frame
    void Update()
    {
/*
        if (isRot)
        {
            
            float rotationAmount = rotSpeed * Time.deltaTime;
            Debug.Log("µÇ´ÀÁß");
                second.transform.Rotate(0, 0, -rotationAmount);

            if (second.transform.rotation.z > currentRotation) isRot = false;

            Debug.Log("ÀÌ’ú¸¬");
        }*/
    }
   /* public async void Rotation()
    {
        while(isRot)
        {
            float currentRotation = second.transform.rotation.eulerAngles.z;
            float rotationAmount = rotSpeed * Time.deltaTime;
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarImage_yd : MonoBehaviour
{
    public float rotionSpeed = 30f;
    public bool isOn;
    public int starSpeed = 5; //별이 좌우아래로 움직일때 
    public int moveSpeed = 1; // 베개에서 별까지 움직이는 스피드

    Transform collisionTr;
    GameObject other;
    Vector3 oriPos;

    // Start is called before the first frame update
    void Start()
    {
     //   oriPos = new Vector3(oriPos.x, oriPos.y - 3, oriPos.z);    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if(collision.gameObject.CompareTag("Player"))
          {
            collision.transform.rotation = Quaternion.Euler(0, 0, 0);
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
            //플레이어가 점점 별한테 가까워지도록 이동
            collisionTr = collision.transform;
            other = collision.gameObject;
            isOn = true;
            
            
        }
    }

/*    public async void MoveToStar(GameObject other)
    {
        isOn = true;
        while(isOn)
        {

        }


    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;
            isOn =false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        //transform.Rotate(0, 0, rotionSpeed * Time.deltaTime);  
    

            Vector3 dir = new Vector3(h, v, 0);
            transform.position += dir * starSpeed * Time.deltaTime;
        Vector3 tarPos = new Vector3(transform.position.x, transform.position.y -5, transform.position.z);
       if(isOn)
            other.transform.position = Vector3.MoveTowards(collisionTr.position, tarPos, moveSpeed * Time.deltaTime);
    }
}

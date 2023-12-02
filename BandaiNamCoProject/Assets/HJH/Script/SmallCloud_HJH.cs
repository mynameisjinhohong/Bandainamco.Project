using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCloud_HJH : MonoBehaviour
{
    public Vector2 jumpPower;
    public Vector3 goTransform;
    public float moveRange;
    public float moveTime;
    public float remainTime;
    bool moveDone = false;
    public void Start()
    {
        StartCoroutine(RandomMove());
        StartCoroutine(TimeCheck());
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && moveDone)
        {
            Rigidbody2D rigid = collision.transform.gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpPower, ForceMode2D.Impulse);
            gameObject.SetActive(false);
        }
    }

    IEnumerator RandomMove()
    {
        //Vector2 ran = Random.insideUnitCircle * moveRange;
        //if(ran.y > 0)
        //{
        //    ran = new Vector2(ran.x, -ran.y);
        //}
        float currentTime = 0;
        Vector3 current = transform.position;
        while (currentTime < moveTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(current, goTransform, currentTime / moveTime);
            yield return null;
        }
        transform.position = goTransform;
        moveDone = true;
    }
    IEnumerator TimeCheck()
    {
        float currentTime = 0;
        while (currentTime < remainTime)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

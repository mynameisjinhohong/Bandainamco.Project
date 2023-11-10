using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCloud_HJH : MonoBehaviour
{
    public float jumpPower;
    public float moveRange;
    public float moveTime;
    public float remainTime;

    public void Start()
    {
        StartCoroutine(RandomMove());
        StartCoroutine(TimeCheck());
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigid = collision.transform.gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            rigid.AddForce(jumpPower * (Vector2)((collision.transform.position - transform.position).normalized) , ForceMode2D.Impulse);
            gameObject.SetActive(false);
        }
    }

    IEnumerator RandomMove()
    {
        Vector2 ran = Random.insideUnitCircle * moveRange;
        if(ran.y > 0)
        {
            ran = new Vector2(ran.x, -ran.y);
        }
        float currentTime = 0;
        Vector3 current = transform.position;
        while (currentTime < moveTime)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(current, current + (Vector3)ran, currentTime / moveTime);
            yield return null;
        }
        transform.position = current + (Vector3)ran;
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

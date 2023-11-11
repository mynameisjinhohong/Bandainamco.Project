using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrainBG_HJH : MonoBehaviour
{
    public float trainTime;
    public Transform startPos;
    public Transform endPos;
    public float trainSpeed;
    bool trainStart = true;
    float currentTime;
    public float trainPower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trainStart)
        {
            transform.position += new Vector3(-trainSpeed * Time.deltaTime, 0, 0);
            if(transform.position.x < endPos.position.x)
            {
                trainStart = false;
                transform.position = startPos.position;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime > trainTime)
            {
                trainStart = true;
                currentTime = 0;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigid = collision.transform.gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            Vector2 vec = (Vector2)((collision.transform.position - transform.position).normalized);
            if(vec.x > 0)
            {
                vec = new Vector2(-vec.x, vec.y);
            }
            rigid.AddForce(trainPower *vec , ForceMode2D.Impulse);
        }
    }
}

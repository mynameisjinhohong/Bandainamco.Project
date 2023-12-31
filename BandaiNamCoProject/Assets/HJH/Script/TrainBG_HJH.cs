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
    AudioSource myAudio;
    bool audioPlay = true;
    public GameObject rail;
    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trainStart)
        {
            transform.localPosition += new Vector3(-trainSpeed * Time.deltaTime, 0, 0);
            if(transform.localPosition.x < endPos.localPosition.x)
            {
                trainStart = false;
                transform.localPosition = startPos.localPosition;
            }
        }
        else
        {
            currentTime += Time.deltaTime;
            if (currentTime > trainTime)
            {
                audioPlay = true;
                trainStart = true;
                currentTime = 0;
            }
        }
        if(audioPlay)
        {
            if(Time.timeScale > 0)
            {
                myAudio.Play();
                audioPlay = false;
            }
        }
        if (ItemManager_LJH.Instance.items[0].isVisited)
        {
            rail.SetActive(true);
        }
        else
        {
            rail.SetActive (false);
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

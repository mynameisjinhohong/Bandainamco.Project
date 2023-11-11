using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloudBG_HJH : MonoBehaviour
{
    public float startX;
    public float endX;
    public GameObject CloudBGCanvas;
    public bool left;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (left)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x < startX)
            {
                transform.position = new Vector3(endX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x > endX)
            {
                transform.position = new Vector3(startX, transform.position.y, transform.position.z);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CloudBGCanvas.SetActive(true);
        }
    }

}

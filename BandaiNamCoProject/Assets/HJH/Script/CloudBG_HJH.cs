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
            transform.localPosition += Vector3.left * speed * Time.deltaTime;
            if (transform.localPosition.x < startX)
            {
                transform.localPosition = new Vector3(endX, transform.localPosition.y, transform.localPosition.z);
            }
        }
        else
        {
            transform.localPosition += Vector3.right * speed * Time.deltaTime;
            if (transform.localPosition.x > endX)
            {
                transform.localPosition = new Vector3(startX, transform.localPosition.y, transform.localPosition.z);
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

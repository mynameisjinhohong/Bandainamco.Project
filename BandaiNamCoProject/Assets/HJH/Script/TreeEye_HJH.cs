using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEye_HJH : MonoBehaviour
{
    public GameObject player;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cam = Camera.main;
        float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }
}

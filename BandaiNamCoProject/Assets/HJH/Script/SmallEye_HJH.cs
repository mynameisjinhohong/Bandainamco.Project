using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class SmallEye_HJH : MonoBehaviour
{
    public GameObject player;
    public float moveRange;
    Vector2 ran;
    Vector3 current;
    float currentTime;
    public float moveTime;
    Camera cam;
    // Start is called before the first frame update
    void OnEnable()
    {
        current = transform.position;
        ran = Random.insideUnitCircle * moveRange;
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        transform.position = Vector3.Lerp(current, current + (Vector3)ran, currentTime / moveTime);
        cam = Camera.main;
        Vector3 playerPos = cam.WorldToScreenPoint(player.transform.position);
        float angle = Mathf.Atan2(playerPos.y-transform.position.y,playerPos.x-transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }
}

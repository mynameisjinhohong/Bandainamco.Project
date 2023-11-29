using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusPetal_LJH : MonoBehaviour
{
    private float speed;
    private Vector3 originPos;

    private bool start = false;
    private Vector3 dir;

    public void Init(float speed)
    {
        this.speed = speed;
        originPos = transform.position;
    }

    private void Update()
    {
        if (!start) return;

        transform.position += speed * dir * Time.deltaTime;
    }

    public void Move()
    {
        if (start) return;

        dir = GamePlayManager_HJH.Instance.characterMovement2D.transform.position - transform.position;
        dir.Normalize();
        dir.z = 0f;

        start = true;
    }

    public void Stop()
    {
        start = false;
        transform.position = originPos;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
        {
            GamePlayManager_HJH.Instance.MulJumpPower(0.5f);
            Stop();
        }
        else if (collision.CompareTag("Edge"))
        {
            Stop();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_LJH : MonoBehaviour
{
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    private float mySpeed;
    private Vector3 initPosition;
    private bool isActive;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        mySpeed = Random.Range(minSpeed, maxSpeed);
        float myScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3 (myScale, myScale, myScale);
        initPosition = transform.localPosition;
        isActive = false;
    }

    public void StartBubble()
    {
        gameObject.SetActive(true);
        isActive = true;
    }

    public void FinishBubble()
    {
        transform.localPosition = initPosition;
        gameObject.SetActive(false);
        isActive = false;
    }

    private void ResetBubble()
    {
        transform.localPosition = initPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Edge"))
        {
            ResetBubble();
            Debug.Log("Bubble Collided Edge");
        }
        else if (collision.gameObject.CompareTag(TagStrings.PlayerTag))
        {
            GamePlayManager_HJH.Instance.characterMovement2D.jumpPower *= 0.8f;
            ResetBubble();
            Debug.Log("Bubble Collided Player");
        }
    }

    //플레이어 충돌은 Trigger? Collider?

    private void Update()
    {
        if (!isActive) return;

        transform.localPosition -= new Vector3(0f, mySpeed * Time.deltaTime, 0f);
    }
}

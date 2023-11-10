using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBackground_LJH : MonoBehaviour
{
    enum MoveState
    {
        Right, Left
    }

    [SerializeField] private float moveOffset;
    [SerializeField] private Vector2 moveSpeedMinMax;
    [SerializeField] private MoveState moveState;
    [SerializeField] private float addVelocity;

    CharacterMovement2D_LSW attachedPlayer = null;
    Vector3 originPos;
    Vector3 rightPos;
    Vector3 leftPos;

    float elapsedOffset = 0f;
    float moveSpeed;

    private void Awake()
    {
        originPos = transform.localPosition;
        rightPos = originPos + new Vector3(moveOffset, 0f, 0f);
        leftPos = originPos + new Vector3(-moveOffset, 0f, 0f);

        moveSpeed = Random.Range(moveSpeedMinMax.x, moveSpeedMinMax.y);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (CheckDirection())
        {
            moveState = moveState == MoveState.Right ? MoveState.Left : MoveState.Right;
            if (attachedPlayer != null)
                AddVelocity();
        }
        switch (moveState)
        {
            case MoveState.Left:
                transform.localPosition += new Vector3(-moveSpeed * Time.unscaledDeltaTime , 0f, 0f);
                break;
            case MoveState.Right:
                transform.localPosition += new Vector3(moveSpeed * Time.unscaledDeltaTime, 0f, 0f);
                break;
        }
    }

    private bool CheckDirection()
    {
        if(moveState == MoveState.Left)
        {
            if (transform.localPosition.x < leftPos.x)
                return true;
        }
        else if(moveState == MoveState.Right)
        {
            if (transform.localPosition.x > rightPos.x)
                return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
        {
            if (attachedPlayer == null)
                attachedPlayer = GamePlayManager_HJH.Instance.characterMovement2D;

            AddVelocity();
        }
    }

    private void AddVelocity()
    {
        float vel = moveState == MoveState.Right ? addVelocity : -addVelocity;
        attachedPlayer.AddVelocity(new Vector2(vel, 0f), true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
            attachedPlayer = null;
    }
}

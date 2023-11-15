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
    [SerializeField] private Vector2 moveSecMinMax;
    [SerializeField] private MoveState moveState;
    [SerializeField] private float addVelocity;

    CharacterMovement2D_LSW attachedPlayer = null;
    Vector3 originPos;
    Vector3 rightPos;
    Vector3 leftPos;

    float elapsedTime = 0f;
    float moveSec;

    private void Awake()
    {
        originPos = transform.localPosition;
        rightPos = originPos + new Vector3(moveOffset, 0f, 0f);
        leftPos = originPos + new Vector3(-moveOffset, 0f, 0f);

        moveSec = Random.Range(moveSecMinMax.x, moveSecMinMax.y);
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (CheckFinish())
        {
            elapsedTime = 0f;
            moveState = moveState == MoveState.Right ? MoveState.Left : MoveState.Right;
            if (attachedPlayer != null)
                AddVelocity();
        }

        elapsedTime += Time.unscaledDeltaTime;

        switch (moveState)
        {
            case MoveState.Left:
                transform.localPosition = Vector3.Lerp(rightPos, leftPos, elapsedTime / moveSec);
                break;
            case MoveState.Right:
                transform.localPosition = Vector3.Lerp(leftPos, rightPos, elapsedTime / moveSec);
                break;
        }
    }

    private bool CheckFinish()
    {
        if (elapsedTime >= moveSec) return true;
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

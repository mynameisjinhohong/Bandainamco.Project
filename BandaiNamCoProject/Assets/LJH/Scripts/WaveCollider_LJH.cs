using Bitgem.VFX.StylisedWater;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollider_LJH : MonoBehaviour
{
    [SerializeField] private float targetY;
    [SerializeField] private float moveSec;
    private float downMoveSec;
    private Vector3 originPos;
    private Vector3 targetPos;
    private bool isCollided = false;

    private void Awake()
    {
        downMoveSec = moveSec * 0.5f;
        originPos = transform.localPosition;
        targetPos = new Vector3(originPos.x, targetY, originPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollided) return;

        if (collision.transform.CompareTag(TagStrings.PlayerTag))
        {
            //파도 닿았으면 파도 종료
            Debug.Log("Wave Collision : " + collision.name);
            Finish();
        }
    }

    public void Finish()
    {
        isCollided = true;
        ItemManager_LJH.Instance.SetBubble(false);
        WorldManager.Instance.NotifyItemEffect(ItemType.Wave, false);
        FinishWave();
    }

    public async void StartWave()
    {
        gameObject.SetActive(true);

        await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);

        float elapsedTime = 0f;
        
        while(elapsedTime < moveSec)
        {
            if (isCollided) break;

            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(originPos, targetPos, elapsedTime / moveSec);
            await UniTask.Yield();
        }

        //파도 끝났으면 파도 종료
        isCollided = true;
        Finish();
    }

    public async void FinishWave()
    {
        float elapsedTime = 0f;
        Vector3 currPos = transform.localPosition;
        while   (elapsedTime < downMoveSec)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currPos, originPos, elapsedTime / downMoveSec);
            await UniTask.Yield();
        }
        isCollided = false;
        gameObject.SetActive(false);
    }
}

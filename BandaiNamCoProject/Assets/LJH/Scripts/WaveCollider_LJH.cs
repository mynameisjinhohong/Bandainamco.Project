using Bitgem.VFX.StylisedWater;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollider_LJH : MonoBehaviour
{
    private enum WaveState
    {
        None, Up, Down
    }

    [SerializeField] private float targetY;
    [SerializeField] private float moveSec;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool customMaxVolume;
    [Range(0f, 1f)]
    [SerializeField] private float maxVolume;
    [SerializeField] private Particles_LJH particle;
    [SerializeField] private GameObject particlePrefab;
    private float downMoveSec;
    private Vector3 originPos;
    private Vector3 targetPos;
    //private bool isCollided = false;
    private WaveState waveState;

    private void Awake()
    {
        downMoveSec = moveSec * 0.5f;
        originPos = transform.localPosition;
        targetPos = new Vector3(originPos.x, targetY, originPos.z);

        particle.Init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (isCollided) return;

        if (collision.transform.CompareTag(TagStrings.PlayerTag))
        {
            //파도 닿았으면 파도 종료
            Debug.Log("Wave Collision : " + collision.name);
            if (particle == null)
            {
                particle = Instantiate(particlePrefab).GetComponent<Particles_LJH>();
            }
            particle.gameObject.SetActive(true);
            particle.transform.position = collision.transform.position;
            particle.Play();
            ItemManager_LJH.Instance.PlayWaveCollisionClip();
            Finish();
        }
    }

    public async void Finish()
    {
        //isCollided = true;
        ItemManager_LJH.Instance.SetBubble(false);
        WorldManager.Instance.NotifyItemEffect(ItemType.Wave, false);
        await FinishWave();
    }

    public async void StartWave(bool isVisited)
    {
        waveState = WaveState.Up;
        await UniTask.Yield();

        gameObject.SetActive(true);

        if (!isVisited)
            await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);

        float elapsedTime = 0f;
        audioSource.Play();
        float myMaxVolume = customMaxVolume == true ? maxVolume : 1f;

        while (elapsedTime < moveSec)
        {
            //if (isCollided) break;

            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(originPos, targetPos, elapsedTime / moveSec);
            audioSource.volume = Mathf.Lerp(0f, myMaxVolume, elapsedTime / moveSec);
            await UniTask.Yield();
        }

        waveState = WaveState.None;
        //파도 끝났으면 파도 종료
        //isCollided = true;
        //Finish();
    }

    public async UniTask FinishWave()
    {
        waveState = WaveState.Down;

        float elapsedTime = 0f;
        Vector3 currPos = transform.localPosition;

        float currVolume = audioSource.volume;

        while (elapsedTime < downMoveSec)
        {
            //파도가 내려가는 중간에 아이템을 다시 먹었을 경우
            if (waveState == WaveState.Up)
                return;

            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(currPos, originPos, elapsedTime / downMoveSec);
            audioSource.volume = Mathf.Lerp(currVolume, 0f, elapsedTime / downMoveSec);
            await UniTask.Yield();
        }
        waveState = WaveState.None;
        audioSource.Stop();
        //particle.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}

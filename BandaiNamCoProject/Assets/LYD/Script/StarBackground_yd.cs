using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using System;

public class StarBackground_yd : BaseBackground_LJH
{
    [SerializeField] private GameObject starPrefab;
    [SerializeField] private GameObject player;
    [Range(3f, 15f)]
    [SerializeField] private double shootingStarDelayTime = 10;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float twinkleTime; //몇초에껐다켜지는지
    [SerializeField] private AudioSource starBGSound;
    [SerializeField] private AudioSource shootingStarSound;

    //임펄스
    private CinemachineImpulseSource impulse;
    private GameObject twinkleStar;
    private bool isFinished = false;
    private bool isOn;

    void Start()
    {
        impulse = transform.GetComponent<CinemachineImpulseSource>();
        twinkleStar = transform.GetChild(1).gameObject;
    }
    private void OnEnable()
    {
        starBGSound.Play();
        isOn = true;
        StartCoroutine(TwinkleStar());
        
        SetEffect();
    }
    IEnumerator TwinkleStar()
    {
        while (true)
        {
            yield return new WaitForSeconds(twinkleTime);
            twinkleStar.SetActive(false);
            yield return new WaitForSeconds(twinkleTime);
            twinkleStar.SetActive(true);
        }
    }

    async void Update()
    {
        if (!isOn) return;
        if (isFinished)
            await SetEffect(shootingStarDelayTime);
    }

    private async UniTask SetEffect(double delayTime = 0)
    {
        if (CameraManager.Instance.isReturnedToPlayer == false)
        {
            await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);
            await UniTask.Delay(400);
        }
        isFinished = false;
        await UniTask.Delay(TimeSpan.FromSeconds(delayTime));

        Vector3 camPos = player.transform.position;
        camPos += new Vector3(x, y, 0f);

        GameObject star = Instantiate(starPrefab, camPos, starPrefab.transform.rotation, player.transform);
        shootingStarSound.Play();

        impulse.GenerateImpulse();

        isFinished = true;
    }
}

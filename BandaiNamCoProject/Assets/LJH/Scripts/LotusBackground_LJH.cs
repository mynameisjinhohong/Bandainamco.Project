using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusBackground_LJH : MonoBehaviour
{
    [SerializeField] private Transform petalsGroup;
    [SerializeField] private int delaySec;
    [SerializeField] private int numOfPetalsToSpawn;
    [SerializeField] private float petalSpeed;
    [SerializeField] private float petalDisappearTime;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float audioFadeTime;
    [SerializeField] private bool customMaxVolume;
    [Range(0f,1f)]
    [SerializeField] private float maxVolume;

    private LotusPetalController controller;

    private void Awake()
    {
        controller = new LotusPetalController(petalsGroup.GetComponentsInChildren<LotusPetal_LJH>(true),
                                                delaySec,
                                                numOfPetalsToSpawn,
                                                petalSpeed,
                                                petalDisappearTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
        {
            SetAudio(true);
            controller.StartPetal();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagStrings.PlayerTag))
        {
            SetAudio(false);
            controller.StopPetal();
        }
    }

    private async void SetAudio(bool isActive)
    {
        float currVolume = audioSource.volume;
        float myMaxVolume = customMaxVolume == true ? maxVolume : 1.0f;
        float destVolume = isActive == true ? myMaxVolume : 0.0f;
        float elapsedTime = 0f;

        if(isActive) audioSource.Play();

        while(elapsedTime < audioFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float tempVolume = Mathf.Lerp(currVolume, destVolume, elapsedTime / audioFadeTime);
            audioSource.volume = tempVolume;
            await UniTask.Yield();
        }

        if (!isActive) audioSource.Stop();
    }
}

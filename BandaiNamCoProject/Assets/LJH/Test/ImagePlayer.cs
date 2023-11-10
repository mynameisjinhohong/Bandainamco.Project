using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImagePlayer : MonoBehaviour
{
    [SerializeField] private List<Sprite> images;
    [SerializeField] private Image target;
    [SerializeField] private double secPer1Img;

    private int currIndex = 0;
    private bool stop = false;


    public async void PlayImages(Action callback = null)
    {
        target.gameObject.SetActive(true);

        for (int i = 0; i < images.Count; i++)
        {
            target.sprite = images[i];
            await UniTask.Delay(TimeSpan.FromSeconds(secPer1Img), ignoreTimeScale: false); //10 s
            await UniTask.Yield();
        }


        for (int i = images.Count - 1; i >= 0; i--)
        {
            target.sprite = images[i];
            await UniTask.Delay(TimeSpan.FromSeconds(secPer1Img), ignoreTimeScale: false); //10 s
            await UniTask.Yield();
        }

        target.gameObject.SetActive(false);

        callback?.Invoke();
    }

    private async UniTask PlayImages(bool isActive, Action callback = null)
    {
        if (isActive)
        {
            target.gameObject.SetActive(isActive);

            for (int i = 0; i < images.Count; i++)
            {
                if (stop) break;
                currIndex = i;
                target.sprite = images[i];
                await UniTask.Delay(TimeSpan.FromSeconds(secPer1Img), ignoreTimeScale: false); //10 s
                await UniTask.Yield();
            }
        }
        else
        {
            stop = true;

            for (int i = currIndex; i >= 0; i--)
            {
                target.sprite = images[i];
                await UniTask.Delay(TimeSpan.FromSeconds(secPer1Img), ignoreTimeScale: false); //10 s
                await UniTask.Yield();
            }

            stop = false;
            target.gameObject.SetActive(isActive);
        }

        callback?.Invoke();
    }



    #region TEST
    public void Play()
    {
        PlayImages(true);
    }

    public void Stop()
    {
        PlayImages(false);
    }
    #endregion
}
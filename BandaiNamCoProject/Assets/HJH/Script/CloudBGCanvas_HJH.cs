using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBGCanvas_HJH : MonoBehaviour
{
    public float waitTime;
    private void OnEnable()
    {
        CloseCanvas();
    }

    async void CloseCanvas()
    {
        await UniTask.Delay((int)(1000*waitTime));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(WorldManager.Instance.MainState == MainState.Pause)
        {
            gameObject.SetActive(false);
        }
    }
}

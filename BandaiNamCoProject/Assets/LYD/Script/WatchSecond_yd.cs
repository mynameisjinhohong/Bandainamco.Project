using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class WatchSecond_yd : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    private string animParam = "Rotate";

    public async void StartClock(System.Action callback = null)
    {
        audioSource.Play();
        animator.SetTrigger(animParam);
        await UniTask.WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);
        callback?.Invoke();
    }

   /* public async void Rotation()
    {
        while(isRot)
        {
            float currentRotation = second.transform.rotation.eulerAngles.z;
            float rotationAmount = rotSpeed * Time.deltaTime;
        }
    }*/
}

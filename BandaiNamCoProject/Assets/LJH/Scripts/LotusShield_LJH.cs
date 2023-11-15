using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusShield_LJH : MonoBehaviour
{
    [SerializeField] private MeshRenderer mr;
    [SerializeField] private float sec;

    private Material mat;

    private void Awake()
    {
        mat = mr.sharedMaterial;
    }

    public void Play()
    {
        gameObject.SetActive(true);

        float elapsedTime = 0f;
        float alpha = 0f;
        while(elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Lerp(0f, 1f, elapsedTime / sec);
            mat.SetFloat("_Alpha", alpha);
        }
    }

    public void Stop()
    {
        float elapsedTime = 0f;
        float alpha = 0f;
        while (elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Lerp(1f, 0f, elapsedTime / sec);
            mat.SetFloat("_Alpha", alpha);
        }

        gameObject.SetActive(false);
    }
}

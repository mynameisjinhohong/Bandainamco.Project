using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloudBGCanvas_HJH : MonoBehaviour
{
    public float waitTime;
    public bool smoke;
    List<Image> smokes;
    public float smokeFadeSpeed;
    private void OnEnable()
    {
        CloseCanvas();
        if (smoke)
        {
            if(smokes == null)
            {
                smokes = new List<Image>();
                for (int i = 0; i < transform.childCount; i++)
                {
                    smokes.Add(transform.GetChild(i).GetComponent<Image>());
                }
            }
            if (smoke && gameObject.activeInHierarchy)
            {
                for(int i = 0; i < smokes.Count; i++)
                {
                    StartCoroutine(FadeOut(smokes[i]));
                }
            }
        }
    }

    private void OnDisable()
    {
        if(smokes != null)
        {
            for (int i = 0; i < smokes.Count; i++)
            {
                Color c = smokes[i].color;
                c.a = 1;
                smokes[i].color = c;
            }
        }

    }

    async void CloseCanvas()
    {
        await UniTask.Delay((int)(1000*waitTime));
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(WorldManager.Instance.MainState == MainState.Pause || WorldManager.Instance.MainState == MainState.GameFinish)
        {
            gameObject.SetActive(false);
        }

    }
    IEnumerator FadeOut(Image img)
    {
        float alpha = 1;
        Color color = img.color;
        while (alpha > 0f)
        {
            alpha -= 0.01f;
            yield return new WaitForSeconds(smokeFadeSpeed);
            color.a = alpha;
            img.color = color;
        }
    }
}

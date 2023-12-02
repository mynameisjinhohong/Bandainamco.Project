using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver_HJH : MonoBehaviour
{
    public GameObject bg;
    public GameObject wave;
    public GameObject clock;
    public GameObject mushroom;
    public GameObject cloud;
    public GameObject eye;
    public GameObject star;
    public GameObject fish;
    public GameObject lotus;
    public GameObject rabbit;
    public GameObject train;
    public float fadeSpeed;
    public void GameOver()
    {
        bg.SetActive(true);
        StartCoroutine(FadeIn(bg));
        for(int i = 0; i< ItemManager_LJH.Instance.items.Length; i++)
        {
            if (ItemManager_LJH.Instance.items[i].isVisited)
            {
                switch (ItemManager_LJH.Instance.items[i].itemType)
                {
                    case ItemType.Wave:
                        wave.SetActive(true);
                        StartCoroutine(FadeIn(wave));
                        break;
                    case ItemType.Clock:
                        clock.SetActive(true);
                        StartCoroutine(FadeIn(clock));
                        break;
                    case ItemType.Mushroom:
                        mushroom.SetActive(true);
                        StartCoroutine(FadeIn(mushroom));
                        break;
                    case ItemType.Rabbit:
                        rabbit.SetActive(true);
                        StartCoroutine(FadeIn(rabbit));
                        break;
                    case ItemType.Lotus:
                        lotus.SetActive(true);
                        StartCoroutine(FadeIn(lotus));
                        break;
                    case ItemType.Cloud:
                        cloud.SetActive(true);
                        StartCoroutine(FadeIn(cloud));
                        break;
                    case ItemType.Eye:
                        eye.SetActive(true);
                        StartCoroutine(FadeIn(eye));
                        break;
                    case ItemType.Fish:
                        fish.SetActive(true);
                        StartCoroutine(FadeIn(fish));
                        break;
                    case ItemType.Star:
                        star.SetActive(true);
                        StartCoroutine(FadeIn(star));
                        break;
                    case ItemType.Train:
                        train.SetActive(true);
                        StartCoroutine(FadeIn(train));
                        break;
                }
            }
        }
    }
    IEnumerator FadeIn(GameObject img)
    {
        Image image = img.GetComponent<Image>();
        float alpha = 0;
        Color color = image.color;
        color.a = 0;
        image.color = color;
        while (alpha < 1f)
        {
            alpha += 0.01f;
            yield return new WaitForSecondsRealtime(fadeSpeed);
            color.a = alpha;
            image.color = color;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

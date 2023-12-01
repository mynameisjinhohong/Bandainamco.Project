using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using KoreanTyper;
using System;
using Unity.Mathematics;
using UnityEngine.UI;
using Bitgem.VFX.StylisedWater;

[Serializable]
public class Item_HJH
{
    //오브젝트 프리팹
    public GameObject prefab;
    //오브젝트 개수
    public int itemCount;
    //부딪혔는지
    public bool isVisited;
    //줌 될 위치
    //public Transform zoomPosition;
    //줌 될때 나올 텍스트
    public string korText;
    public string engText;
    public string japText;
    public Sprite itemSprite;
    public string korExplain;
    public string engExplain;
    public string japExplain;

    //줌 될때 카메라 사이즈
    //public int camSize;
    public ItemType itemType;
    public bool needWholeCam;
    public bool transparentMaterial; //Cloud
    public bool opaqueAlphaClipMaterial; //WAve
    public GameObject bgObject;
    [HideInInspector] public List<SpriteRenderer> renderers = null;
    [HideInInspector] public bool isShown = false;
}

public class ItemManager_LSW : MonoBehaviour
{
    public static ItemManager_LSW Instance;
    public Item_HJH currItem;

    public GameObject player;
    public int itemCount;
    public Item_HJH[] items;
    public List<GameObject> spawnItems;
    public float itemsDistance;
    public GameObject bg;
    public CamFollowe_HJH camFollow;
    public float zoomOutSpeed;
    public float zoomInSpeed;
    public TMP_Text subText;
    public bool nowText = false;
    public bool skip = false;
    public bool endText = false;
    public float typingSpeed = 0.1f;
    Vector3 firstCamPos;
    public Vector3 bgSize;
    public GameObject[] zoomInOffObject;// 카메라 줌인줌아웃할때 꺼지는 오브젝트들
    public GameObject bubble;
    // Start is called before the first frame update
    #region 일단 구름 테스트
    public GameObject zoomCanvas;
    public float fadeSpeed;
    public GameObject[] clouds;
    public GameObject cloudParent;
    public float moveSpeed;
    #endregion

    [Header("버섯")]
    public float mashroomTime = 1;  //버섯 커지거나 작아지는 시간

    [Header("파도")]
    public WaterVolumeTransforms water;


    void Awake()
    {
        Instance = this;

        bgSize = GetBGSize(bg);
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items[i].itemCount; j++)
            {
                GameObject item = Instantiate(items[i].prefab);
                Vector3 pos;
                int su = 0; //무한루프 방지용
                while (true)
                {
                    su++;
                    pos = Return_RandomPosition();
                    bool restart = false;
                    for (int k = 0; k < spawnItems.Count; k++)
                    {
                        if ((pos - spawnItems[k].transform.position).magnitude < itemsDistance)
                        {
                            restart = true;
                            break;
                        }
                    }
                    if ((pos - player.transform.position).magnitude < itemsDistance)
                    {
                        restart = true;
                    }
                    if (su > 100)
                    {
                        restart = false;
                    }
                    if (!restart)
                    {
                        break;
                    }
                }
                item.transform.position = pos;
                item.transform.parent = bg.transform;
                item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y, item.transform.parent.position.z - 5);
                item.GetComponent<BaseItem_LSW>().itemNum = i;
                item.GetComponent<BaseItem_LSW>().itemManager = this;

                spawnItems.Add(item);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (nowText)
        {
            if (Input.GetMouseButtonDown(0))
            {
                skip = true;
            }
        }
        if (endText)
        {
            if (Input.GetMouseButtonDown(0))
            {
                endText = false;
                subText.gameObject.SetActive(false);
                StartCoroutine(CameraZoomIn(camFollow.firstCamSize));
            }
        }

    }
    #region WAVE
    public void SetWave(bool isStart)
    {
        //if (isStart)
        //    water.StartWave();
        //else
        //    water.FinishWave();

        //GamePlayManager_HJH.Instance.SetGravity(isStart);
    }

    #endregion

    public void TriggerCount(int su)
    {
        if (items[su].isVisited == false)
        {
            CameraZoomOutFuncStart(su);
            itemCount++;
        }
        items[su].isVisited = true;
    }
    Vector3 Return_RandomPosition()
    {

        float x = UnityEngine.Random.Range(-bgSize.x / 2, bgSize.x / 2);
        float y = UnityEngine.Random.Range(-bgSize.y / 2, bgSize.y / 2);
        Vector3 randomPostion = new Vector3(x, y, 0);
        return randomPostion;
    }

    #region CameraZoomOut
    public void CameraZoomOutFuncStart(int itemIdx)
    {
        Time.timeScale = 0f;
        Camera.main.cullingMask = ~((1 << 7) | (1 << 8));
        firstCamPos = Camera.main.transform.position;
        camFollow.camFollow = false;
        float bigSize = Mathf.Max(bgSize.x, bgSize.y);
        for (int i = 0; i < zoomInOffObject.Length; i++)
        {
            zoomInOffObject[i].SetActive(false);
        }
        StartCoroutine(CameraZoomOut(itemIdx));
    }
    public Vector3 GetBGSize(GameObject bg)
    {
        Vector2 bgSpriteSize = bg.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localbGSize = bgSpriteSize / bg.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Debug.Log("bgSpriteSize : " + bgSpriteSize);
        Debug.Log("bgSpriteSize / pixelsPerUnit : " + localbGSize);
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bg.transform.lossyScale.x;
        worldbGSize.y *= bg.transform.lossyScale.y;
        return worldbGSize;
    }
    IEnumerator CameraZoomOut(int itemIdx)
    {
        Camera cam = Camera.main;
        if (itemIdx == 2 || itemIdx == 4)
        {
            Camera.main.cullingMask = -1;
            float camSize = Mathf.Max(bgSize.x, bgSize.y) / 2;
            while (cam.orthographicSize < camSize || (cam.transform.position - Vector3.zero).magnitude > 1f)
            {
                if (cam.orthographicSize < camSize)
                {
                    cam.orthographicSize += zoomOutSpeed * Time.unscaledDeltaTime;
                }
                if ((cam.transform.position - Vector3.zero).magnitude > 0.1f)
                {
                    cam.transform.position = Vector3.Lerp(cam.transform.position, Vector3.zero, Time.fixedDeltaTime * 0.15f);
                }
                yield return null;
            }
        }
        else
        {
            //while (cam.orthographicSize < items[itemIdx].camSize || (cam.transform.position - items[itemIdx].zoomPosition.position).magnitude > 1f)
            //{
            //    if ((cam.transform.position - items[itemIdx].zoomPosition.position).magnitude > 0.1f)
            //    {
            //        cam.transform.position = Vector3.Lerp(cam.transform.position, items[itemIdx].zoomPosition.position, Time.fixedDeltaTime * 0.15f);
            //    }
            //    if (cam.orthographicSize < items[itemIdx].camSize)
            //    {
            //        cam.orthographicSize += zoomOutSpeed * Time.unscaledDeltaTime;
            //    }
            //    yield return null;
            //}
        }
        zoomCanvas.SetActive(true);
        //StartCoroutine(TextAni(items[itemIdx].zoomText));
        for (int i = 0; i < clouds.Length; i++)
        {
            StartCoroutine(FadeIn(clouds[i]));
        }
        StartCoroutine(CloudMove());
    }


    //IEnumerator CameraZoomOut(float camSize) 구버전
    //{
    //    Camera cam = Camera.main;
    //    while (cam.orthographicSize < camSize || (cam.transform.position - Vector3.zero).magnitude > 0.1f)
    //    {
    //        if(cam.orthographicSize < camSize)
    //        {
    //            cam.orthographicSize += zoomOutSpeed * Time.unscaledDeltaTime;
    //        }
    //        if((cam.transform.position - Vector3.zero).magnitude > 0.1f)
    //        {
    //            cam.transform.position = Vector3.Lerp(cam.transform.position, Vector3.zero, Time.fixedDeltaTime * 0.15f);
    //        }
    //        yield return null;
    //    }
    //    StartCoroutine(TextAni(whatText));
    //}
    IEnumerator CloudMove()
    {
        RectTransform rect = cloudParent.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(-Screen.width, 0, 0);
        float move = 0;
        while (rect.anchoredPosition.x < 0)
        {
            move += moveSpeed * Time.unscaledDeltaTime;
            rect.anchoredPosition = new Vector3(-Screen.width + move, 0, 0);
            yield return null;
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
            alpha += 0.001f;
            yield return new WaitForSecondsRealtime(fadeSpeed);
            color.a = alpha;
            image.color = color;
        }
    }


    IEnumerator TextAni(string text)
    {
        subText.gameObject.SetActive(true);
        int typeLength = text.GetTypingLength();
        nowText = true;
        for (int i = 0; i < typeLength + 1; i++)
        {
            subText.text = text.Typing(i);
            if (skip)
            {
                skip = false;
                nowText = false;
                subText.text = text;
                break;
            }
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
        nowText = false;
        endText = true;
    }
    IEnumerator CameraZoomIn(float camSize)
    {
        Camera cam = Camera.main;
        Camera.main.cullingMask = ~((1 << 7));
        zoomCanvas.SetActive(false);
        while (cam.orthographicSize > camSize || (cam.transform.position - firstCamPos).magnitude > 1f)
        {
            if ((cam.transform.position - firstCamPos).magnitude > 1f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, firstCamPos, Time.fixedDeltaTime * 0.15f);
            }
            if (cam.orthographicSize > camSize)
            {
                cam.orthographicSize -= zoomInSpeed * Time.unscaledDeltaTime;
            }
            yield return null;
        }
        Camera.main.cullingMask = -1;
        for (int i = 0; i < zoomInOffObject.Length; i++)
        {
            zoomInOffObject[i].SetActive(true);
        }
        Time.timeScale = 1f;
        Camera.main.transform.position = firstCamPos;
        camFollow.camFollow = true;
    }
    //IEnumerator CameraZoomIn(float camSize) 구버전
    //{
    //    Camera cam = Camera.main;
    //    while (cam.orthographicSize > camSize || (cam.transform.position - firstCamPos).magnitude < 0.1f)
    //    {
    //        if (cam.orthographicSize > camSize)
    //        {
    //            cam.orthographicSize -= zoomInSpeed * Time.unscaledDeltaTime;
    //        }
    //        if ((cam.transform.position - firstCamPos).magnitude > 0.1f)
    //        {
    //            cam.transform.position = Vector3.Lerp(cam.transform.position, firstCamPos, Time.fixedDeltaTime * 0.15f);
    //        }

    //        yield return null;
    //    }
    //    Camera.main.cullingMask = -1;
    //    Time.timeScale = 1f;
    //    Camera.main.transform.position = firstCamPos;
    //    camFollow.camFollow = true;
    //}
    #endregion

    #region 버섯 오브젝트

    //public IEnumerator PlayerScale(Transform targetTr, float scale, float resetTime)
    //{
    //    Vector3 originalScale = targetTr.localScale;
    //    Vector3 targetScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z * scale);
    //    float currentTime = 0;
    //    targetTr.localScale = targetScale;
    //    while (currentTime < mashroomTime)
    //    {
    //        targetTr.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / mashroomTime);
    //        currentTime += Time.deltaTime;
    //        Debug.Log("커짐");
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(resetTime);
    //    Debug.Log("유지시간");
    //    currentTime = 0;

    //    // currentTime = 0;
    //    while (currentTime < mashroomTime)
    //    {
    //        targetTr.localScale = Vector3.Lerp(targetScale, originalScale, currentTime / mashroomTime);
    //        currentTime += Time.deltaTime;
    //        yield return null;
    //        Debug.Log("작아짐");

    //    }
    //    targetTr.localScale = originalScale;

    //    yield return null;
    //}
    #endregion
}

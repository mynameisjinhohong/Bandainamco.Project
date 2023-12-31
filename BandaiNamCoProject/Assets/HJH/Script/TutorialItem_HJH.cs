using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialItem_HJH : MonoBehaviour
{
    public enum TutoType
    {
        Click,
        Wasd,
        Space,
    }
    public TutoType tutoType;
    public bool isItem;
    public ItemType myType;
    public float fadeSpeed;
    bool tuToOn = false;
    bool idxOn = false;
    public int myIndex = 0;
    float currentTime = 0;
    public float wasdTime = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isItem)
        {
            if (!idxOn && ItemManager_LJH.Instance != null)
            {
                for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                {
                    if (ItemManager_LJH.Instance.items[i].itemType == myType)
                    {
                        myIndex = i;
                    }
                }
                idxOn = true;
            }
            if (ItemManager_LJH.Instance.items[myIndex].isVisited)
            {
                if (!tuToOn)
                {
                    if(tutoType == TutoType.Wasd)
                    {
                        Invoke("FadeInStart", 1f);
                    }
                    else
                    {
                        FadeInStart();
                    }
                    tuToOn = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
                if(i != transform.childCount - 1)
                {
                    StartCoroutine(FadeOut(transform.GetChild(i).GetComponent<SpriteRenderer>()));

                }
            }
            tuToOn = true;
        }
        if (tuToOn)
        {
            switch (tutoType)
            {
                case TutoType.Click:
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case TutoType.Wasd:
                    currentTime += Time.deltaTime;
                    if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || currentTime  > wasdTime)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case TutoType.Space:
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

    }

    public void FadeInStart()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            if (i != transform.childCount - 1)
            {
                StartCoroutine(FadeOut(transform.GetChild(i).GetComponent<SpriteRenderer>()));
            }
        }
    }

    IEnumerator FadeIn(SpriteRenderer img)
    {
        float alpha = 0;
        Color color = img.color;
        color.a = 0;
        img.color = color;
        while (alpha < 1f)
        {
            transform.position = new Vector3(transform.position.x,transform.position.y,0);  
            alpha += 0.005f;
            yield return new WaitForSeconds(fadeSpeed);
            color.a = alpha;
            img.color = color;
        }
        StartCoroutine(FadeOut(img));
    }
    IEnumerator FadeOut(SpriteRenderer img)
    {
        float alpha = 1;
        Color color = img.color;
        color.a = 1;
        img.color = color;
        while (alpha > 0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            alpha -= 0.005f;
            yield return new WaitForSeconds(fadeSpeed);
            color.a = alpha;
            img.color = color;
        }
        StartCoroutine(FadeIn(img));
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI_HJH : MonoBehaviour
{
    public GameObject quitPopUp;           
    public AudioSource aud;
    public GameObject[] mouseOverImage;
    public float fadeSpeed;
    public GameObject optionCanvas;
    public Slider volumeSlider;
    public TMP_Dropdown langaugeDropdown;
    public Button conti;

    // Start is called before the first frame update
    void Start()
    {

        volumeSlider.value = GameManager.instance.userData.volume;
        volumeSlider.onValueChanged.AddListener(VolumeChange);
        if(GameManager.instance != null)
        {
            if(GameManager.instance.userData.stage == 0)
            {
                bool first = false;
                for(int i =0; i< GameManager.instance.userData.stageDatas[0].itemOnOff.Length; i++)
                {
                    if (GameManager.instance.userData.stageDatas[0].itemOnOff[i])
                    {
                        first = true;
                        break;
                    }
                }
                if (!first)
                {
                    if(conti != null)
                    {
                        conti.interactable = false;
                        conti.GetComponent<EventTrigger>().enabled = false;
                    }

                }
            }
            langaugeDropdown.value = GameManager.instance.userData.langaugeSet;
        }
    }

    public void LangaugeChange()
    {
        int val = langaugeDropdown.value;
        GameManager.instance.LangaugeSet(val);
        GameManager.instance.SaveUserData();
    }
    void VolumeChange(float value)
    {
        GameManager.instance.Volume = value;
        GameManager.instance.SaveUserData();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionCanvas.activeInHierarchy)
            {
                OptionOffButton();
            }
            //else if(quitPopUp != null)
            //{
            //    quitPopUp.SetActive(true);
            //}
            else if (quitPopUp.activeInHierarchy)
            {
                QuitOffButton();
            }
        }
    }

    public void StartButton()
    {
        if(aud != null)
        {
            aud.Play();
        }
        Invoke("MoveScene", 0.3f);
    }

    public void NewGameButton()
    {
        GameManager.instance.userData.stage = 0;
        GameManager.instance.userData.stageDatas = new StageData_HJH[4];
        if (GameManager.instance.userData.stageDatas[0] == null)
        {
            for (int i = 0; i < GameManager.instance.userData.stageDatas.Length; i++)
            {
                GameManager.instance.userData.stageDatas[i] = new StageData_HJH();
                GameManager.instance.userData.stageDatas[i].itemOnOff = new bool[GameManager.instance.itemCount[i]];
            }
        }
        if (aud != null)
        {
            aud.Play();
        }
        Invoke("MoveScene", 0.3f);
    }
    public void MoveScene()
    {

        LoadingManager_HJH.LoadScene("StageScene");
    }
    public void QuitApp()
    {
        Application.Quit();
    }

    public void QuitButton()
    {
        aud.Play();
        if (!quitPopUp.activeInHierarchy) {
            quitPopUp.SetActive(true);
        }
    }

    public void QuitOffButton()
    {
        aud.Play();
        if (quitPopUp.activeInHierarchy)
        {
            quitPopUp.SetActive(false);
        }
    }

    public void OptionButton()
    {
        aud.Play();
        if (!optionCanvas.activeInHierarchy)
        {
            optionCanvas.SetActive(true);
        }
    }

    public void OptionOffButton()
    {
        aud.Play();
        if (optionCanvas.activeInHierarchy)
        {
            optionCanvas.SetActive(false);
        }
    }

    public void PointOver(int a)
    {
        StopAllCoroutines();
        for(int i =0; i<mouseOverImage.Length; i++)
        {
            if(i == a)
            {
                StartCoroutine(FadeIn(mouseOverImage[i]));
            }
            else
            {
                StartCoroutine(FadeOut(mouseOverImage[i]));
            }
        }
    }

    IEnumerator FadeIn(GameObject img)
    {
        Image image = img.GetComponent<Image>();
        float alpha = 0;
        Color color = image.color;
        while (alpha < 0.6f)
        {
            alpha += 0.1f;
            yield return new WaitForSeconds(fadeSpeed);
            color.a = alpha;
            image.color = color;
        }
    }

    IEnumerator FadeOut(GameObject img)
    {
        Image image = img.GetComponent<Image>();
        Color color = image.color;
        float alpha = color.a;
        while (alpha > 0f)
        {
            alpha -= 0.1f;
            yield return new WaitForSeconds(fadeSpeed);
            color.a = alpha;
            image.color = color;
        }
    }


}
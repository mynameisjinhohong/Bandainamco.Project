using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI_HJH : MonoBehaviour
{
    public float fadeSpeed;
    public GameObject[] mouseOverImage;
    public GameObject pauseCanvas;
    public GameObject optionCanvas;
    public GameObject gameOverCanvas;
    public GameObject tutoObj;
    bool tuto = false;
    public Slider volumeSlider;
    public TMP_Dropdown langaugeDropdown;
    public AudioSource buttonAudio;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance != null)
        {
            volumeSlider.value = GameManager.instance.userData.volume;
            langaugeDropdown.value = GameManager.instance.userData.langaugeSet;
        }
        volumeSlider.onValueChanged.AddListener(VolumeChange);
    }
    public void LangaugeChange()
    {
        int val = langaugeDropdown.value;
        GameManager.instance.LangaugeSet(val);
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
            else if (!pauseCanvas.activeInHierarchy && WorldManager.Instance.MainState != MainState.UiOn )
            {
                PauseOnButton();
            }
            else if (pauseCanvas.activeInHierarchy)
            {
                PauseOffButton();
            }
        }
    }
    public void PauseOnButton()
    {
        if(WorldManager.Instance.MainState != MainState.GameFinish)
        {
            if (tutoObj.activeInHierarchy)
            {
                tuto = true;
                tutoObj.SetActive(false);
            }
            buttonAudio.Play();
            WorldManager.Instance.MainState = MainState.UiOn;
            pauseCanvas.SetActive(true);
        }
    }

    public void PauseOffButton()
    {
        if (tuto)
        {
            tuto = false;
            tutoObj.SetActive(true);
        }
        if (CameraManager.Instance.currCamera == CamValues.Character || GamePlayManager_HJH.Instance.start == false)
        {
            WorldManager.Instance.MainState = MainState.Play;
        }
        else
        {
            WorldManager.Instance.MainState =MainState.Pause;
        }
        buttonAudio.Play();
        pauseCanvas.SetActive(false);
    }

    public void PointOver(int a)
    {
        StopAllCoroutines();
        for (int i = 0; i < mouseOverImage.Length; i++)
        {
            if (i == a)
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
        while (alpha < 0.7f)
        {
            alpha += 0.01f;
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
            alpha -= 0.01f;
            yield return new WaitForSeconds(fadeSpeed);
            color.a = alpha;
            image.color = color;
        }
    }

    public void OptionOnButton()
    {

        buttonAudio.Play();
        optionCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void OptionOffButton()
    {
        if (tuto)
        {
            tuto = false;
            tutoObj.SetActive(true);
        }
        buttonAudio.Play();
        optionCanvas.SetActive(false);
        if (CameraManager.Instance.currCamera == CamValues.Character || GamePlayManager_HJH.Instance.start == false)
        {
            WorldManager.Instance.MainState = MainState.Play;
        }
        else
        {
            WorldManager.Instance.MainState = MainState.Pause;
        }
    }

    public void RestartButton()
    {
        buttonAudio.Play();
        StartCoroutine(Restart());
    }
    IEnumerator Restart()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    void VolumeChange(float value)
    {
        GameManager.instance.Volume = value;
    }

    public void QuitButton()
    {
        buttonAudio.Play();
        WorldManager.Instance.MainState = MainState.Play;
        StartCoroutine(GoStart());
    }
    IEnumerator GoStart()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        LoadingManager_HJH.LoadScene("StartScene");

    }
    public void EndingQuit()
    {
        LoadingManager_HJH.LoadScene("StageScene");
    }
  
}

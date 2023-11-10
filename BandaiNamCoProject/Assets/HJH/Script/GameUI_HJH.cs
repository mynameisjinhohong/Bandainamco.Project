using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_HJH : MonoBehaviour
{
    public float fadeSpeed;
    public GameObject[] mouseOverImage;
    public GameObject pauseCanvas;
    public GameObject optionCanvas;
    public GameObject gameOverCanvas;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance != null)
        {
            volumeSlider.value = GameManager.instance.userData.volume;
        }
        volumeSlider.onValueChanged.AddListener(VolumeChange);
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

            WorldManager.Instance.MainState = MainState.UiOn;
            pauseCanvas.SetActive(true);
        }
    }

    public void PauseOffButton()
    {
        if (CameraManager.Instance.currCamera == CamValues.Character || GamePlayManager_HJH.Instance.start == false)
        {
            WorldManager.Instance.MainState = MainState.Play;
        }
        else
        {
            WorldManager.Instance.MainState =MainState.Pause;
        }
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
        optionCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    public void OptionOffButton()
    {
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

    void VolumeChange(float value)
    {
        GameManager.instance.Volume = value;
    }

    public void QuitButton()
    {
        WorldManager.Instance.MainState = MainState.Play;
        
        LoadingManager_HJH.LoadScene("StartScene");
    }

  
}

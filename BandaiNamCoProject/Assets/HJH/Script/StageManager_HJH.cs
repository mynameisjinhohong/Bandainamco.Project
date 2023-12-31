using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager_HJH : MonoBehaviour
{
    public List<GameObject> doors; // ������ ����
    public GameObject optionDoor;
    public Sprite closeDoorSprite; //������ , ������
    public GameObject bg;
    public int doorNum;

    public AudioSource doorOpenSound;
    public AudioSource doorCantOpenSound;
    public AudioSource buttonSound;
    public AudioSource openingCutScene;

    public GameObject optionCanvas;
    public Slider volumeSlider;
    public float cameraZoomInSpeed;
    public GameObject tuto;
    public GameObject yetCanvas;

    public GameObject firstScene;
    public GameObject skipButton;
    bool cutScene = false;
    bool yetOn = false;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            volumeSlider.value = GameManager.instance.userData.volume;
        }
        for (int i = 0; i < doors.Count; i++)
        {
            if (GameManager.instance != null)
            {
                if (GameManager.instance.userData.stage < i)
                {
                    doors[i].transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = closeDoorSprite;
                }
            }
        }
        //for(int i =0; i< doorCount; i++)
        //{
        //    GameObject newDoor = GameObject.Instantiate(doorPrefab);
        //    doors.Add(newDoor);
        //    newDoor.transform.position = startPoint.position;
        //    newDoor.transform.parent = bg.transform;
        //    newDoor.transform.position = new Vector3(newDoor.transform.position.x + (i*doorInterval),newDoor.transform.position.y,newDoor.transform.position.z);
        //    if(GameManager.instance != null )
        //    {
        //        if(GameManager.instance.userData.stage < i)
        //        {
        //            newDoor.GetComponent<SpriteRenderer>().sprite = doorsSprite[1];
        //        }
        //        else
        //        {
        //            //newDoor.GetComponent<SpriteRenderer>().sprite = doorsSprite[0];
        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        bool findDoor = false;
        for (int i = 0; i < doors.Count; i++)
        {
            if (Mathf.Abs(doors[i].transform.position.x - Camera.main.transform.position.x) < 2f)
            {
                doors[i].transform.GetChild(0).gameObject.SetActive(true);
                doorNum = i;
                findDoor = true;
                if (GameManager.instance != null)
                {
                    if (!GameManager.instance.userData.stageTuto)
                    {
                        tuto.SetActive(true);
                    }
                }
            }
            else
            {
                doors[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (Mathf.Abs(optionDoor.transform.position.x - Camera.main.transform.position.x) < 2f)
        {
            doorNum = -1;
            optionDoor.transform.GetChild(0).gameObject.SetActive(true);
            findDoor = true;
            if (GameManager.instance != null)
            {
                if (!GameManager.instance.userData.stageTuto)
                {
                    tuto.SetActive(true);
                }
            }
        }
        else
        {
            optionDoor.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (findDoor)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (GameManager.instance != null)
                {
                    if (doorNum <= GameManager.instance.userData.stage)
                    {
                        if (!cutScene)
                        {
                            if (doorNum < 0)
                            {
                                optionDoor.GetComponent<Animator>().SetTrigger("Open");
                                doorOpenSound.Play();
                                if (GameManager.instance != null)
                                {
                                    GameManager.instance.userData.stageTuto = true;
                                    tuto.SetActive(false);
                                    GameManager.instance.SaveUserData();
                                }
                                Invoke("OptionOn", 2f);
                            }
                            else
                            {
                                if(doorNum == 0)
                                {
                                    doors[doorNum].GetComponent<Animator>().SetTrigger("Open");
                                    doorOpenSound.Play();
                                    StartCoroutine(CameraZoomIn());
                                    if (GameManager.instance != null)
                                    {
                                        GameManager.instance.userData.stageTuto = true;
                                        tuto.SetActive(false);
                                        GameManager.instance.SaveUserData();
                                    }
                                    Invoke("MoveScene", 2f);
                                }
                                else
                                {
                                    if (!yetOn)
                                    {
                                        doorCantOpenSound.Play();
                                        yetOn = true;
                                        yetCanvas.SetActive(true);
                                        Time.timeScale = 0f;
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (!yetOn)
                        {
                            doorCantOpenSound.Play();
                            yetOn = true;
                            yetCanvas.SetActive(true);
                            Time.timeScale = 0f;
                        }

                    }
                }
            }
        }
        else
        {
            tuto.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionCanvas.activeInHierarchy)
            {
                OptionOff();
            }
            else if(yetCanvas.activeInHierarchy)
            {
                YetOff();
            }
        }
    }

    IEnumerator CameraZoomIn()
    {
        Camera cam = Camera.main;
        while (true)
        {
            if (cam.orthographicSize > 0)
            {
                cam.orthographicSize -= cameraZoomInSpeed;
            }
            if (Mathf.Abs(cam.transform.position.x - doors[doorNum].transform.position.x) > 0.5f)
            {
                cam.transform.position += new Vector3((doors[doorNum].transform.position.x - cam.transform.position.x) * 0.005f, 0, 0);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void OptionOn()
    {
        optionCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void OptionOff()
    {
        Time.timeScale = 1f;
        buttonSound.Play();
        optionCanvas.SetActive(false);
        doorOpenSound.Play();
        optionDoor.GetComponent<Animator>().SetTrigger("Close");
    }
    public void MoveScene()
    {
        switch (doorNum)
        {
            case 0:
                firstScene.SetActive(true);
                cutScene = true;
                openingCutScene.Play();
                GameManager.instance.AudioOff();
                skipButton.SetActive(true);
                break;
        }
    }

    public void CutSceneSkip()
    {
        LoadingManager_HJH.LoadScene("GameScene");
    }

    public void YetOff()
    {
        buttonSound.Play();
        yetCanvas.SetActive(false) ;
        yetOn = false;
        Time.timeScale = 1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager_HJH : MonoBehaviour
{
    public List<GameObject> doors; // 생성된 문들
    public GameObject optionDoor;
    public Sprite closeDoorSprite; //열린문 , 닫힌문
    public GameObject bg;
    int doorNum;

    public GameObject optionCanvas;
    public Slider volumeSlider;
    public float cameraZoomInSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            volumeSlider.value = GameManager.instance.userData.volume;
        }
        volumeSlider.onValueChanged.AddListener(VolumeChange);
        for(int i =0; i< doors.Count; i++)
        {
            if(GameManager.instance != null)
            {
                if(GameManager.instance.userData.stage < i)
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
    void VolumeChange(float value)
    {
        GameManager.instance.Volume = value;
    }

    // Update is called once per frame
    void Update()
    {
       for(int i =0; i<doors.Count; i++)
        {
            if (Mathf.Abs(doors[i].transform.position.x - Camera.main.transform.position.x )< 2f)
            {
                doors[i].transform.GetChild(0).gameObject.SetActive(true);
                doorNum = i;
            }
            else
            {
                doors[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
       if(Mathf.Abs(optionDoor.transform.position.x - Camera.main.transform.position.x) < 2f)
       {
            doorNum = -1;
            optionDoor.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            optionDoor.transform.GetChild(0).gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(GameManager.instance != null)
            {
                if(doorNum <= GameManager.instance.userData.stage)
                {
                    if(doorNum < 0)
                    {
                        optionDoor.GetComponent<Animator>().SetTrigger("Open");
                        Invoke("OptionOn", 2f);
                    }
                    else
                    {
                        doors[doorNum].GetComponent<Animator>().SetTrigger("Open");
                        StartCoroutine(CameraZoomIn());
                        Invoke("MoveScene", 2f);
                    }
                }
            }
        }

        if(optionDoor.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            OptionOff();
        }
    }

    IEnumerator CameraZoomIn()
    {
        Camera cam = Camera.main;
        while (true)
        {
            if(cam.orthographicSize > 0)
            {
                cam.orthographicSize -= cameraZoomInSpeed;
            }
            if(Mathf.Abs(cam.transform.position.x - doors[doorNum].transform.position.x) > 0.5f)
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
        optionCanvas.SetActive(false);
        optionDoor.GetComponent<Animator>().SetTrigger("Close");
    }
    public void MoveScene()
    {
        LoadingManager_HJH.LoadScene("GameScene");
    }
}

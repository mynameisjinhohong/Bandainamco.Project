using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_HJH : MonoBehaviour
{
    public GameObject startCloud;
    public GameObject tutoCanvas;
    public CharacterMovement2D_LSW player;
    public Rigidbody2D playerRigid;
    public GameObject tutoBG;
    public GameObject item;
    public Transform itemPos;
    public RectTransform itemText;
    float currentTime = 0;
    int tutoidx = 0;
    bool itemTuto = false;
    void Start()
    {
        player.enabled = false;
        playerRigid.gravityScale = 0;
        startCloud.SetActive(true);
        tutoCanvas.SetActive(true);
        TutoOnOff(tutoidx);
    }

    void TutoOnOff(int su)
    {
        for(int i =0; i<tutoCanvas.transform.childCount; i++)
        {
            if(i == su)
            {
                tutoCanvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                tutoCanvas.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tutoidx < 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tutoidx++;
                TutoOnOff(tutoidx);
            }
            if(tutoidx == 2) 
            {
                player.enabled = true;
                playerRigid.gravityScale = 1;
                tutoCanvas.SetActive(false);
                tutoBG.SetActive(false);
            }
        }
        else if(tutoidx == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tutoidx++;
                startCloud.SetActive(false);
            }
        }
        else if (tutoidx == 3)
        {
            if (GamePlayManager_HJH.Instance.start)
            {
                currentTime += Time.deltaTime;
                playerRigid.velocity = new Vector3(0,playerRigid.velocity.y,0);
                if (!itemTuto)
                {
                    item = ItemManager_LJH.Instance.spawnItems[0].gameObject;
                    for (int i = 0; i < item.gameObject.transform.childCount; i++)
                    {
                        item.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = "Tutorial";
                    }
                    item.transform.position = itemPos.position;
                    itemTuto = true;
                }
                if(currentTime > 0.5f)
                {
                    tutoBG.SetActive(true);
                    tutoCanvas.SetActive(true);
                    TutoOnOff(tutoidx);
                    itemText.position = new Vector3(itemText.position.x,Camera.main.WorldToScreenPoint(item.transform.position).y,0);
                    Time.timeScale = 0;
                    if (Input.GetMouseButtonDown(0))
                    {
                        tutoidx++;
                        Time.timeScale = 1;
                        tutoBG.SetActive(false) ;
                        tutoCanvas.SetActive(false );
                    }
                }
            }
        }

    }
}

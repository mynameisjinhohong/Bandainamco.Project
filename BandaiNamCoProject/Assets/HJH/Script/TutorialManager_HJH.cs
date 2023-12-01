using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager_HJH : MonoBehaviour
{
    public GameObject startCloud;
    public GameObject tutoCanvas;
    public CharacterMovement2D_LSW player;
    public Rigidbody2D playerRigid;
    public GameObject emptyBG;
    int tutoidx = 0;
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
        if(tutoidx < 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tutoidx++;
                TutoOnOff(tutoidx);
            }
            if(tutoidx == 3) 
            {
                player.enabled = true;
                playerRigid.gravityScale = 1;
                tutoCanvas.SetActive(false);
            }
        }
        else if(tutoidx == 3)
        {
            if (Input.GetMouseButtonDown(0))
            {
                tutoidx++;
                startCloud.SetActive(false);
            }
        }
        else if (tutoidx == 4)
        {
            if (GamePlayManager_HJH.Instance.start)
            {

            }
        }

    }
}

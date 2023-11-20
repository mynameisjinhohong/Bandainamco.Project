using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBG_HJH : MonoBehaviour
{
    public float eyeCoolTime;
    public float eyeRemainTime;
    float currentTime;
    public CharacterMovement2D_LSW player;

    bool nowEye = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldManager.Instance.MainState == MainState.Play && CameraManager.Instance.currCamera == CamValues.Character)
        {
            if (ItemManager_LJH.Instance.items[4].isVisited)
            {
                currentTime += Time.deltaTime;
                if (nowEye)
                {
                    if (currentTime > eyeRemainTime)
                    {
                        Debug.Log("EyeEnd");
                        player.EyeStart(false);
                        nowEye = false;
                        Camera.main.cullingMask = -1;
                        currentTime = 0;
                    }
                }
                else
                {
                    if (currentTime > eyeCoolTime)
                    {
                        Debug.Log("EyeStart");
                        nowEye = true;
                        player.EyeStart(true);
                        Camera.main.cullingMask = ~(1 << 7);
                        currentTime = 0;
                    }
                }
            }
        }
        else
        {
            nowEye = false;
            player.EyeStart(false);
            currentTime = 0;
        }
    }
}

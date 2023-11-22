using System.Collections;
using UnityEngine;

public class StageCamera_HJH : MonoBehaviour
{
    public GameObject upButton;
    public GameObject downButton;
    public AudioSource[] walkAudio;
    public AudioSource elevatorSound;
    public AudioSource buttonSound;
    int walkCount = 0;
    public float cameraMoverSpeed;
    public float startPoint;
    public float endPoint;

    public float upPower;
    public bool walk;
    public float walkTime;
    public float walkWaitTime;
    float currentTime;

    public GameObject background;
    public Vector3 backgroundSize;

    #region ÄÆ¾À
    [Header("ÄÆ¾À")]
    public float cutSceneSpeed;
    public float cameraSize;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float doorSpeed;
    public GameObject elevator;
    public enum ElevatorStage
    {
        OpenStart,
        Opening,
        Opened,
        Up,
        Down,
        CloseStart,
        Closing,
        Closed,
    }
    public ElevatorStage elevatorStage;
    public Sprite[] elevatorNum;
    public Sprite[] elevatorUpDown;
    public SpriteRenderer elevatorNumSprite;
    public SpriteRenderer elevatorUpDownSprite;
    public int eleNum;
    float elevatorTime;
    public GameObject quitPopUp;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCutScene());
        eleNum = 8;
        elevatorTime = 0;
    }
    IEnumerator StartCutScene()
    {
        Time.timeScale = 0f;
        Camera cam = Camera.main;
        float x = cameraSize - cam.orthographicSize; 
        elevatorStage = ElevatorStage.Opened;
        while (true)
        {
            cam.orthographicSize += x*(cutSceneSpeed *0.01f) ;
            leftDoor.transform.position -= new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            rightDoor.transform.position += new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            yield return new WaitForSecondsRealtime(0.01f);
            if(cam.orthographicSize >= cameraSize)
            {
                cam.orthographicSize = cameraSize;
                leftDoor.transform.position = new Vector3(-2.6f, leftDoor.transform.position.y, 0);
                rightDoor.transform.position = new Vector3(2.6f, rightDoor.transform.position.y, 0);
                break;
            }
        }
        Time.timeScale = 1f;
        transform.position = new Vector3(0, 0, -10);
        //FirstSetting();
    }
    public void FirstSetting()
    {
        Vector2 bgSpriteSize = background.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localBGSize = bgSpriteSize / background.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        backgroundSize = localBGSize;
        backgroundSize.x *= background.transform.lossyScale.x;
        backgroundSize.y *= background.transform.lossyScale.y;
        float cameraWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        startPoint = background.transform.position.x - backgroundSize.x / 2.0f + cameraWidth;
        endPoint = background.transform.position.x + backgroundSize.x / 2.0f - cameraWidth;
        transform.position = new Vector3(0, 0, -10);
    }

    // Update is called once per frame
    void Update()
    {
        float stageMove = Input.GetAxis("Horizontal");
        if (Mathf.Abs(stageMove) < 0.5)
        {
            if (stageMove < 0)
            {
                stageMove = -0.5f;
            }
            else if (stageMove > 0)
            {
                stageMove = 0.5f;
            }
        }
        if (!walk && stageMove != 0 && Time.timeScale != 0)
        {
            StartCoroutine(Walk(stageMove));
            walkCount++;
            if(walkCount > 1)
            {
                walkCount = 0;
            }
        }
        Elevator();

        if(quitPopUp.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            QuitNo();
        }
    }


    IEnumerator Walk(float stageMove)
    {
        walk = true;
        walkAudio[walkCount].Play();
        while (true)
        {
            currentTime += 0.01f;
            if(currentTime > walkTime)
            {
                break;
            }
            yield return new WaitForSeconds(0.01f);
            if(currentTime < walkTime / 2)
            {
                gameObject.transform.position += new Vector3(stageMove * cameraMoverSpeed, upPower, -10);
            }
            else
            {
                gameObject.transform.position += new Vector3(stageMove * cameraMoverSpeed, -upPower, -10);
            }
            gameObject.transform.position = new Vector3(Mathf.Clamp(gameObject.transform.position.x, startPoint, endPoint), gameObject.transform.position.y, -10);
        }
        yield return new WaitForSeconds(walkWaitTime);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, -10);
        currentTime = 0;
        walk = false;

    }
    #region ¿¤¸®º£ÀÌÅÍ °ü·Ã ÇÔ¼ö
    public void Elevator()
    {
        if (elevatorStage == ElevatorStage.OpenStart)
        {
            upButton.SetActive(false);
            downButton.SetActive(false);
            StartCoroutine(ElevatorOpen());
        }
        else if (elevatorStage == ElevatorStage.Opened)
        {
            if (Mathf.Abs(elevator.transform.position.x - Camera.main.transform.position.x) > 2f)
            {
                elevatorStage = ElevatorStage.CloseStart;
            }
        }
        else if (elevatorStage == ElevatorStage.CloseStart)
        {
            StartCoroutine(ElevatorClose());
        }
        else if (elevatorStage == ElevatorStage.Closed)
        {
            if (Mathf.Abs(elevator.transform.position.x - Camera.main.transform.position.x) > 2f)
            {
                elevatorStage = ElevatorStage.Down;
            }
            else
            {
                elevatorStage = ElevatorStage.OpenStart;
            }
        }
        else if (elevatorStage == ElevatorStage.Down)
        {
            upButton.SetActive(false);
            downButton.SetActive(true);
            elevatorTime += Time.deltaTime;
            elevatorUpDownSprite.sprite = elevatorUpDown[1];
            if (elevatorTime > 1f && eleNum > 0)
            {
                elevatorTime = 0;
                eleNum--;
                elevatorNumSprite.sprite = elevatorNum[eleNum];
            }
            if (Mathf.Abs(elevator.transform.position.x - Camera.main.transform.position.x) < 2f)
            {
                elevatorTime = 0;
                elevatorStage = ElevatorStage.Up;
                elevatorSound.Play();
            }
        }
        else if (elevatorStage == ElevatorStage.Up)
        {
            upButton.SetActive(true);
            downButton.SetActive(false);
            elevatorTime += Time.deltaTime;
            elevatorUpDownSprite.sprite = elevatorUpDown[0];
            if (elevatorTime > 1f && eleNum < 8)
            {
                elevatorTime = 0;
                eleNum++;
                elevatorNumSprite.sprite = elevatorNum[eleNum];
            }
            if (eleNum == 8)
            {
                elevatorTime = 0;
                elevatorStage = ElevatorStage.OpenStart;
                elevatorSound.Stop();
            }
            if (Mathf.Abs(elevator.transform.position.x - Camera.main.transform.position.x) > 2f)
            {
                elevatorTime = 0;
                elevatorStage = ElevatorStage.Down;
                elevatorSound.Stop();
            }
        }
    }
    IEnumerator ElevatorOpen()
    {
        elevatorStage = ElevatorStage.Opening;
        while (true)
        {
            leftDoor.transform.position -= new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            rightDoor.transform.position += new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            yield return new WaitForSecondsRealtime(0.01f);
            if (Mathf.Abs(elevator.transform.position.x - Camera.main.transform.position.x) > 2f)
            {
                elevatorStage = ElevatorStage.CloseStart;
                break;
            }
            if (leftDoor.transform.position.x < -2.6f)
            {
                leftDoor.transform.position = new Vector3(-2.6f, leftDoor.transform.position.y, 0);
                rightDoor.transform.position = new Vector3(2.6f, rightDoor.transform.position.y, 0);
                break;
            }
        }
        if (elevatorStage == ElevatorStage.Opening)
        {
            elevatorStage = ElevatorStage.Opened;
            quitPopUp.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    public void QuitYes()
    {
        buttonSound.Play();
        Invoke("GotoStartScene", 0.3f);
    }

    void GotoStartScene()
    {
        LoadingManager_HJH.LoadScene("StartScene");

    }

    public void QuitNo()
    {
        buttonSound.Play();
        quitPopUp.SetActive(false);
        Time.timeScale = 1f;
    }

    IEnumerator ElevatorClose()
    {
        elevatorStage = ElevatorStage.Closing;
        while (true)
        {
            leftDoor.transform.position += new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            rightDoor.transform.position -= new Vector3(1.7f * 0.01f * doorSpeed, 0, 0);
            yield return new WaitForSecondsRealtime(0.01f);
            if (leftDoor.transform.position.x > -0.9f)
            {
                leftDoor.transform.position = new Vector3(-0.89f, leftDoor.transform.position.y, 0);
                rightDoor.transform.position = new Vector3(0.89f, rightDoor.transform.position.y, 0);
                break;
            }
        }
        elevatorStage = ElevatorStage.Closed;
    }
    #endregion
}

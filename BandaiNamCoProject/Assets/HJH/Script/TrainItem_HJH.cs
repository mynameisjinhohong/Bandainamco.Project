using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainItem_HJH : BaseItem_LJH
{
    public CharacterMovement2D_LSW player;
    public float trainSpeed;
    public float railSpeed;
    public GameObject train;
    public GameObject trainIcon;
    public GameObject[] trainRail;
    public Transform playerPos;
    bool already2 = false;
    bool trainStart = false;
    bool railStart = false;
    bool left = false;
    bool playerOn = true;
    bool railReady = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !already2)
        {
            already2 = true;
            ItemManager_LJH.Instance.itemCount += 1;
            ItemManager_LJH.Instance.CurrItem = this;

            if (!myItem.isVisited)
            {
                WorldManager.Instance.MainState = MainState.Pause;
                if (myItem.needWholeCam)
                {
                    CameraManager.Instance.CameraControlAfterItem(myItem.itemType.ToString(), true);
                }
                else
                {
                    CameraManager.Instance.CameraControlAfterItem(myItem.itemType.ToString(), false);

                }
            }
            Animator bubbleAni;
            if (bubble.TryGetComponent<Animator>(out bubbleAni))
            {
                bubbleAni.SetTrigger("Pop");
            }
            myItem.isVisited = true;
            player = other.GetComponent<CharacterMovement2D_LSW>();
            TrainActivate();
        }
    }

    async void TrainActivate()
    {
        int ran = Random.Range(0, 2);
        player.gameObject.tag = "Untagged";
        player.SetGravity(false);
        Vector2 trainSize = trainRail[0].GetComponent<SpriteRenderer>().sprite.rect.size / trainRail[0].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        trainSize.x *= trainRail[0].transform.lossyScale.x;
        if(ran == 0)
        {
            left = true;
            train.transform.rotation = Quaternion.identity;
            for(int i =0; i< trainRail.Length; i++)
            {
                trainRail[i].transform.position += new Vector3(-i * (trainSize.x), 0 , 0);
            }
        }
        else 
        {
            left = false;
            playerPos.transform.localPosition = new Vector3(playerPos.transform.localPosition.x, playerPos.transform.localPosition.y, -10);
            train.transform.rotation = Quaternion.Euler(0,180,0);
            for (int i = 0; i < trainRail.Length; i++)
            {
                trainRail[i].transform.position += new Vector3(i * (trainSize.x), 0, 0);
            }
        }
        railStart = true;
        await UniTask.WaitUntil(() => Mathf.Abs(trainRail[0].transform.position.y - trainRail[0].GetComponent<TrainRail_HJH>().railMaxY)< 1);
        trainStart = true;
        railStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (railStart && WorldManager.Instance.MainState == MainState.Play)
        {
            if (!railReady)
            {
                player.gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 50);
                trainIcon.gameObject.SetActive(false);
                train.gameObject.SetActive(true);
                for (int i =0; i< trainRail.Length; i++)
                {
                    TrainRail_HJH train = trainRail[i].GetComponent<TrainRail_HJH>();
                    trainRail[i].transform.position = new Vector3(trainRail[i].transform.position.x, Camera.main.ViewportToWorldPoint(new Vector2(0, 0f)).y, 0);
                    train.railMaxY = (GetComponent<BoxCollider2D>().bounds.center.y - GetComponent<BoxCollider2D>().bounds.extents.y);
                    train.railSu = i;
                    train.StartMove();
                    trainRail[i].gameObject.SetActive(true);
                }
                railReady = true;
            }
        }
        if (!trainStart)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetGravity(true);
            player.gameObject.tag = "Player";
            playerOn = false;
        }
        if (left)
        {
            train.transform.position += Vector3.left * trainSpeed * Time.deltaTime;
        }
        else
        {
            train.transform.position += Vector3.right * trainSpeed * Time.deltaTime;
        }

    }
    private void LateUpdate()
    {
        if (railStart)
        {
            player.transform.GetChild(0).localScale = Vector3.one;
            player.transform.position = playerPos.position;
            player.transform.GetChild(0).rotation = Quaternion.identity;
        }
        if (!trainStart)
        {
            return;
        }
        if (Mathf.Abs(playerPos.transform.position.x) > DataManager.Instance.bgSize.x / 2)
        {
            gameObject.SetActive(false);
        }
        if (playerOn)
        {
            player.transform.position = playerPos.position;
            player.transform.GetChild(0).rotation = Quaternion.identity;
        }

    }
}

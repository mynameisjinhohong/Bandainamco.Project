using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public enum EndingType
{
    Over, Good
}

public class GamePlayManager_HJH : ManagerBase
{
    public static GamePlayManager_HJH Instance;
    public AudioSource gameOverSound;
    public AudioSource endingSound;
    public CharacterMovement2D_LSW characterMovement2D;
    public GameObject player;
    public SpriteRenderer emptyBgSpriteRenderer;
    public Material backgroundMat;
    public Material opaqueAlphaClipMat;
    public Material transparentMat;
    public float bgFadeinSec;
    // public ItemManager_LSW itemManager;
    public float currentTime;
    public TMP_Text timeText;
    public bool gameOver = false;
    public GameObject mainCanvas;
    public TMP_Text endingAllText;
    public GameObject allStoryObj;
    public string koEndingText;
    public string enEndingText;
    public string jaEndingText;

    public GameObject[] endings; //임시 나중에 지울것
    private EndingType endingType;
    private bool gameEnd = false;
    private List<BaseItem_LJH> consumedItems;

    #region 시작부분

    public bool start = false;
    #endregion

    private void Awake()
    {
        Instance = this;
        gameOver = false;
    }

    public override void Init()
    {
        consumedItems = new List<BaseItem_LJH>();
        base.Init();
    }


    public override void GameOver()
    {
        switch (endingType)
        {
            case EndingType.Good:
                endings[1].SetActive(true);
                if (GameManager.instance != null)
                {
                    GameManager.instance.AudioOff();
                    GameManager.instance.userData.stage++;
                    endingSound.Play();
                }
                if (GameManager.instance != null)
                {
                    //이전에 저장된 진행도랑 이번 게임 진행도 비교해서 더 크면 이번 게임 진행도로 교체
                    int su = 0;
                    for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                    {
                        if (ItemManager_LJH.Instance.items[i].isVisited)
                        {
                            su++;
                        }
                    }
                    int su2 = 0;
                    for (int i = 0; i < GameManager.instance.userData.stageDatas[GameManager.instance.userData.stage - 1].itemOnOff.Length; i++)
                    {
                        if(GameManager.instance.userData.stageDatas[GameManager.instance.userData.stage - 1].itemOnOff[i])
                        {
                            su2++;
                        }
                    }
                    if (su >= su2)
                    {
                        for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                        {
                            if (ItemManager_LJH.Instance.items[i].isVisited)
                            {
                                GameManager.instance.userData.stageDatas[GameManager.instance.userData.stage - 1].itemOnOff[i] = true;
                            }
                            else
                            {
                                GameManager.instance.userData.stageDatas[GameManager.instance.userData.stage - 1].itemOnOff[i] = false;
                            }
                        }
                    }


                    GameManager.instance.SaveUserData();
                    mainCanvas.SetActive(false);
                    StringBuilder end = new StringBuilder();
                    switch (GameManager.instance.userData.langaugeSet)
                    {
                        case 0:
                            for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                            {
                                end.Append(ItemManager_LJH.Instance.items[i].engText);
                            }
                            end.Append(enEndingText);
                            endingAllText.text = end.ToString();
                            break;
                        case 1:
                            for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                            {
                                end.Append(ItemManager_LJH.Instance.items[i].japText);
                            }
                            end.Append(jaEndingText);
                            endingAllText.text = end.ToString();
                            break;
                        case 2:
                            for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                            {
                                end.Append(ItemManager_LJH.Instance.items[i].korText);
                            }
                            end.Append(koEndingText);
                            endingAllText.text = end.ToString();
                            break;
                    }
                }
                break;
            case EndingType.Over:
                endings[0].SetActive(true);
                if (GameManager.instance != null)
                {
                    for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
                    {
                        if (ItemManager_LJH.Instance.items[i].isVisited)
                        {
                            GameManager.instance.userData.stageDatas[GameManager.instance.userData.stage].itemOnOff[i] = true;
                        }
                    }
                    GameManager.instance.SaveUserData();
                }
                break;
        }

        base.GameOver();
    }
    private void Update()
    {

        if (player.transform.position.y < DataManager.Instance.bgSize.y / 2 && !start)
        {
            start = true;
            CameraManager.Instance.SetCamera(CamValues.Character);
            SetEmptyBG();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (WorldManager.Instance.MainState != MainState.Play)
        {
            return;
        }
        Vector3 pos = Camera.main.WorldToViewportPoint(player.transform.position);
        if (gameOver)
        {
            if (ItemManager_LJH.Instance.CurrItem != null)
            {
                if (ItemManager_LJH.Instance.CurrItem.myItem.itemType == ItemType.Lotus)
                {
                    characterMovement2D.SetPosition(Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0.5f)));

                    ItemManager_LJH.Instance.SetLotus(true);
                    ItemManager_LJH.Instance.SetLotusShield(false);
                    ItemManager_LJH.Instance.PlayLotusClip();
                    SetPlayerVelocity(Vector2.zero);
                    ItemManager_LJH.Instance.CurrItem = null;
                    gameOver = false;
                    return;
                }
            }
            endingType = EndingType.Over;
            gameEnd = true;
            gameOverSound.Play();
            gameOver = false;
            //Time.timeScale = 0f;
            //GameOver();
        }
        int itemCount = 0;
        for (int i = 0; i < ItemManager_LJH.Instance.items.Length; i++)
        {
            if (ItemManager_LJH.Instance.items[i].isVisited)
            {
                itemCount++;
            }
        }
        if (itemCount >= ItemManager_LJH.Instance.items.Length)
        {
            endingType = EndingType.Good;
            gameEnd = true;
        }
        if (gameEnd)
        {
            WorldManager.Instance.MainState = MainState.GameFinish;
        }
    }

    private async void SetEmptyBG()
    {
        emptyBgSpriteRenderer.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < bgFadeinSec)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = Mathf.Lerp(0f, 1f, elapsedTime / bgFadeinSec);

            emptyBgSpriteRenderer.sharedMaterial.SetFloat("_Progress", progress);

            await UniTask.Yield();
        }
        emptyBgSpriteRenderer.sharedMaterial = backgroundMat;
    }

    //아이템 먹은 후, 플레이어에게 나타날 효과를 switch문으로 정리
    //아이템 먹었을 때 : start = true
    //아이템 효과 끝날 때 : start = false
    public override void ItemEffect(ItemType itemType, bool start)
    {
        if (start)
        {
            switch (itemType)
            {
                case ItemType.Wave:
                    WorldManager.Instance.NotifyReset();
                    ItemManager_LJH.Instance.SetActiveItems(false);
                    SetPlayerGravity(false);
                    SetPlayerJumpPower(0.6f);
                    SetPlayerJumpCoolTime(0f);
                    break;
            }
        }
        else
        {
            switch (itemType)
            {
                case ItemType.Wave:
                    ItemManager_LJH.Instance.SetActiveItems(true);
                    SetPlayerGravity(true);
                    break;
            }

        }
        base.ItemEffect(itemType, start);
    }
    public void TurnOnfullStory()
    {
        allStoryObj.SetActive(true);
    }


    private void SetPlayerVelocity(Vector2 velocity)
    {
        characterMovement2D.SetVelocity(velocity);
    }

    private void SetPlayerGravity(bool hasGravity)
    {
        characterMovement2D.SetGravity(hasGravity);
    }

    private void SetPlayerJumpPower(float multiplier)
    {
        characterMovement2D.jumpPower *= multiplier;
    }

    private void SetPlayerJumpCoolTime(float coolTime)
    {
        characterMovement2D.coolTime = coolTime;
    }

    public override void BackgroundEffect(ItemType itemType, bool start)
    {
        base.BackgroundEffect(itemType, start);
    }

    public void AddConsumedItem(BaseItem_LJH item)
    {
        if (consumedItems.Contains(item)) return;
        consumedItems.Add(item);
    }

    public bool ContainItem(BaseItem_LJH item)
    {
        return consumedItems.Contains(item);
    }

    public override void Reset()
    {
        characterMovement2D.Reset();
        base.Reset();
    }

    public void MulJumpPower(float multiplier)
    {
        characterMovement2D.SetVelocity(Vector2.zero);
        characterMovement2D.jumpPower *= multiplier;
    }

    public void SetBackgroundMat(Item_HJH item, SpriteRenderer renderer)
    {
        if (item.opaqueAlphaClipMaterial)
            renderer.sharedMaterial = opaqueAlphaClipMat;
        else if (item.transparentMaterial)
            renderer.sharedMaterial = transparentMat;
        else
            renderer.sharedMaterial = backgroundMat;

    }

    /*    public void GameOver()
        {
            endings[0].SetActive(true);
        }

        public void GoodEnding()
        {
            endings[1].SetActive(true);
        }

        public void BadEnding()
        {
            endings[2].SetActive(true);
        }*/
}

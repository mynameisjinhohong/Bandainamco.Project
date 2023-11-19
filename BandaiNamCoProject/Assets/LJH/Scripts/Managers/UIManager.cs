using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class CloudInfo
{
    public RectTransform cloudRT;
    public Vector3 endPos;
    private Vector3 firstPos;
    private Image image;

    public Vector3 FirstPos { get { return firstPos; } set { firstPos = value; } }
    public Image MyImage { get { return image; } set { image = value; } }
}

public class UIManager : ManagerBase
{
    public static UIManager Instance;
    private bool isGameOver = false;

    [SerializeField] private CloudInfo[] clouds;
    [SerializeField] private Ease ease;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI timeText;

    public Animator uiani;
    public GameObject itemCanvas;

    private float currTime = 0f;
    public bool isCloud = false;
    bool isFinished = false;
    private void Update()
    {
        if (isGameOver) return;

        currTime += Time.deltaTime;
        timeText.text = currTime.ToString();
        if (isCloud && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                isCloud = false;
                isFinished = true;
            }
        }
    }


    public override void Init()
    {
        Instance = this;

        foreach (CloudInfo cloud in clouds)
        {
            cloud.FirstPos = cloud.cloudRT.anchoredPosition;
            cloud.MyImage = cloud.cloudRT.GetComponent<Image>();
        }

        base.Init();
    }

    public override void GameOver()
    {
        isGameOver = true;
        base.GameOver();
    }

    public async void ControlCloud(Action finishCallback = null)
    {
        //for (int i = 0; i < clouds.Length; i++)
        //{
        //    clouds[i].cloudRT.anchoredPosition = clouds[i].FirstPos;
        //    clouds[i].cloudRT.gameObject.SetActive(true);
        //}

        //DG.Tweening.Sequence sequence = DOTween.Sequence();

        //sequence.Append(clouds[0].cloudRT.DOAnchorPos(clouds[0].endPos, 2f).SetEase(ease)).SetUpdate(true);
        //for (int i = 1; i < clouds.Length; i++)
        //{
        //    sequence.Join(clouds[i].cloudRT.DOAnchorPos(clouds[i].endPos, 2f).SetEase(ease)).SetUpdate(true);
        //}
        //sequence.onComplete = (async () =>
        //{
        //    text.gameObject.SetActive(true);
        //    text.text = "";
        //    string str = "";
        //    string originText = ItemManager_LJH.Instance.CurrItem.myItem.zoomText;

        //    for (int i = 0; i < originText.Length; i++)
        //    {
        //        str += originText[i];
        //        text.text = str;
        //        await UniTask.Yield();
        //        await UniTask.Delay(100,true);
        //    }
        //});

        itemCanvas.SetActive(true);
        if (GameManager.instance != null)
        {
            switch (GameManager.instance.userData.langaugeSet)
            {
                case 0:
                    text.text = ItemManager_LJH.Instance.CurrItem.myItem.engText;
                    break;
                case 1:
                    text.text = ItemManager_LJH.Instance.CurrItem.myItem.japText;
                    break;
                case 2:
                    text.text = ItemManager_LJH.Instance.CurrItem.myItem.korText;
                    break;
            }

        }
        await UniTask.WaitUntil(() => isFinished);
        uiani.SetTrigger("Fadeout");

        finishCallback?.Invoke();
        isFinished = false;
        text.gameObject.SetActive(false);

        //for (int i = 0; i < clouds.Length; i++)
        //{
        //    clouds[i].cloudRT.gameObject.SetActive(false);
        //}
    }
    public async void EndingText(Action finishCallback = null)
    {
        //for (int i = 0; i < clouds.Length; i++)
        //{
        //    clouds[i].cloudRT.anchoredPosition = clouds[i].FirstPos;
        //    clouds[i].cloudRT.gameObject.SetActive(true);
        //}

        //DG.Tweening.Sequence sequence = DOTween.Sequence();

        //sequence.Append(clouds[0].cloudRT.DOAnchorPos(clouds[0].endPos, 2f).SetEase(ease)).SetUpdate(true);
        //for (int i = 1; i < clouds.Length; i++)
        //{
        //    sequence.Join(clouds[i].cloudRT.DOAnchorPos(clouds[i].endPos, 2f).SetEase(ease)).SetUpdate(true);
        //}
        //sequence.onComplete = (async () =>
        //{
        //    text.gameObject.SetActive(true);
        //    text.text = "";
        //    string str = "";
        //    string originText = ItemManager_LJH.Instance.CurrItem.myItem.zoomText;

        //    for (int i = 0; i < originText.Length; i++)
        //    {
        //        str += originText[i];
        //        text.text = str;
        //        await UniTask.Yield();
        //        await UniTask.Delay(100,true);
        //    }
        //});

        itemCanvas.SetActive(true);
        if (GameManager.instance != null)
        {
            switch (GameManager.instance.userData.langaugeSet)
            {
                case 0:
                    text.text = GamePlayManager_HJH.Instance.enEndingText;
                    break;
                case 1:
                    text.text = GamePlayManager_HJH.Instance.jaEndingText;
                    break;
                case 2:
                    text.text = GamePlayManager_HJH.Instance.koEndingText;
                    break;
            }

        }
        await UniTask.WaitUntil(() => isFinished);
        uiani.SetTrigger("Fadeout");

        finishCallback?.Invoke();
        isFinished = false;
        text.gameObject.SetActive(false);

        //for (int i = 0; i < clouds.Length; i++)
        //{
        //    clouds[i].cloudRT.gameObject.SetActive(false);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Video;

public class CamValues
{
    public const string Character = "Character";
    public const string Whole = "Whole";
    public const string Wave = "Wave";

    public const int priorityOn = 1;
    public const int priorityOff = 0;
}

public class CameraManager : ManagerBase
{
    public static CameraManager Instance;
    [SerializeField] private SpriteRenderer bgSprite;
    [SerializeField] private CinemachineBrain brainCam;
    [SerializeField] private float dissolveSec;
    private CinemachineVirtualCamera[] virtualCams;
    private Dictionary<string, CinemachineVirtualCamera> virtualCamDic;
    public CinemachineVirtualCamera virtualCamera;
    public float orthographicSizeWhole;
    public string currCamera = CamValues.Character;

    public bool isReturnedToPlayer;
    public bool endFadeOut = false;
    public GameObject bg;
    public override void Init()
    {

        Instance = this;
        isReturnedToPlayer = false;
        SetOrthographicSizeWhole();
        virtualCams = brainCam.GetComponentsInChildren<CinemachineVirtualCamera>();
        virtualCamDic = new Dictionary<string, CinemachineVirtualCamera>();

        foreach (var cam in virtualCams)
        {
            string camName = cam.name.Split("_")[1];
            virtualCamDic.Add(camName, cam);
        }
        base.Init();
    }

    public void SetCamera(string cameraName)
    {
        if (cameraName == CamValues.Whole)
        {
            if ((bg.transform.rotation.z / 90) % 2 == 1)
            {
                virtualCamDic[cameraName].m_Lens.OrthographicSize = Mathf.Min(DataManager.Instance.bgSize.x, DataManager.Instance.bgSize.y) / 2;
            }
            else
            {
                virtualCamDic[cameraName].m_Lens.OrthographicSize = Mathf.Max(DataManager.Instance.bgSize.x, DataManager.Instance.bgSize.y) / 2;
            }
        }
        foreach (var cam in virtualCams)
            cam.Priority = CamValues.priorityOff;
        currCamera = cameraName;
        virtualCamDic[cameraName].Priority = CamValues.priorityOn;
    }

    public async void CameraControlAfterItem(string cameraName, bool isWhole)
    {
        isReturnedToPlayer = false;
        if (isWhole)
        {
            SetCamera(CamValues.Whole);
        }
        else
        {
            SetCamera(cameraName);
            Camera.main.cullingMask = ~((1 << 7) | (1 << 8));
        }
        await UniTask.Delay(1000, true);
        if (ItemManager_LJH.Instance.CurrItem.myItem.bgObject != null)
        {
            MakeBG(ItemManager_LJH.Instance.CurrItem.myItem); //배경에 오브젝트 생기는거 연출할 함수 일단 임의로 만들어 놓음
        }
        UIManager.Instance.ControlCloud(async () =>
        {
            await UniTask.WaitUntil(() => endFadeOut == true);
            endFadeOut = false;
            UIManager.Instance.itemCanvas.SetActive(false);
            SetCamera(CamValues.Character);
            StartCoroutine(AfterCameraChange());
            isReturnedToPlayer = true;
        });
    }

    public async void MakeBG(Item_HJH item)
    {
        if (item.isShown) return;

        item.bgObject.SetActive(true);

        item.renderers = new List<SpriteRenderer>();
        item.renderers.AddRange(item.bgObject.GetComponentsInChildren<SpriteRenderer>(true));

        float elapsedTime = 0f;
        while (elapsedTime < dissolveSec)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = Mathf.Lerp(0f, 1f, elapsedTime / dissolveSec);
            foreach (var r in item.renderers)
            {
                if (r.CompareTag(TagStrings.NoDissolveTag)) continue;
                r.sharedMaterial.SetFloat("_Progress", progress);
            }
            await UniTask.Yield();
        }

        foreach(var r in item.renderers)
        {
            if (r.CompareTag(TagStrings.NoDissolveTag)) continue;
            GamePlayManager_HJH.Instance.SetBackgroundMat(item, r);
        }

        item.isShown = true;
        //if (item.isShown) return;

        //item.bgObject.SetActive(true);

        //if (item.renderers == null)
        //{
        //    item.renderers = new List<Renderer>();
        //    item.renderers.AddRange(item.bgObject.GetComponentsInChildren<Renderer>(true));
        //}

        //float elapsedTime = 0f;
        //while (elapsedTime < dissolveSec)
        //{
        //    elapsedTime += Time.unscaledDeltaTime;
        //    float progress = Mathf.Lerp(0f, 1f, elapsedTime / dissolveSec);
        //    foreach (var r in item.renderers)
        //    {
        //        r.sharedMaterial.SetFloat("_Progress", progress);
        //    }
        //    await UniTask.Yield();
        //}
        //item.isShown = true;
    }

    IEnumerator AfterCameraChange()
    {
        yield return new WaitForSecondsRealtime(2f);
        Camera.main.cullingMask = -1;
        if(WorldManager.Instance.MainState != MainState.UiOn)
        {
            WorldManager.Instance.MainState = MainState.Play;
        }
    }

    private void SetOrthographicSizeWhole()
    {
        Vector2 bgSpriteSize = bgSprite.sprite.rect.size;
        Vector2 localbGSize = bgSpriteSize / bgSprite.sprite.pixelsPerUnit;
        Debug.Log("bgSpriteSize : " + bgSpriteSize);
        Debug.Log("bgSpriteSize / pixelsPerUnit : " + localbGSize);
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bgSprite.transform.lossyScale.x;
        worldbGSize.y *= bgSprite.transform.lossyScale.y;

        orthographicSizeWhole = Mathf.Min(worldbGSize.x, worldbGSize.y);
    }
}

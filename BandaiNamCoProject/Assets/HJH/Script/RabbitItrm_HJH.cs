using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RabbitItrm_HJH : BaseItem_LJH
{
    public bool end = false;
    public CharacterMovement2D_LSW player;
    public float duringTime;
    float currentTime;
    float currentTime2;
    public float makeTime;
    bool not = true;
    bool startAudio = false;
    public Animator animator;
    public AudioSource rabbitAudio;
    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (not)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                ItemManager_LJH.Instance.itemCount += 1;
                ItemManager_LJH.Instance.CurrItem = this;
                GamePlayManager_HJH.Instance.AddConsumedItem(this);
                player = other.GetComponent<CharacterMovement2D_LSW>();
                other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

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
                AudioSource bubbleAudio = bubble.GetComponent<AudioSource>();
                bubbleAudio.Play();
                myItem.isVisited = true;
                RabbitAni();
                not = false;
            }
        }

    }
    private void Start()
    {

    }
    private void Update()
    {
        if (startAudio)
        {
            if(Time.timeScale > 0)
            {
                rabbitAudio.Play();
                startAudio = false;
            }
        }
    }

    async void RabbitAni()
    {
        int su = 0;
        animator.SetTrigger("Rabbit");
        startAudio = true;
        transform.GetChild(su).gameObject.SetActive(true);
        su++;
        while (true)
        {
            currentTime += Time.deltaTime;
            currentTime2 += Time.deltaTime;
            if (currentTime2 > makeTime)
            {
                currentTime2 = 0;
                transform.GetChild(su).gameObject.SetActive(true);
                su++;
            }
            if (currentTime > duringTime)
            {
                end = true;
            }
            if (end)
            {
                gameObject.SetActive(false);
                break;
            }
            await UniTask.Yield();
        }
    }
}

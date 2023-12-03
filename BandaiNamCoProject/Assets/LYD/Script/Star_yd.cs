using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Star_yd : BaseItem_LJH
{
    [SerializeField] private GameObject starImage;
    [SerializeField] private GameObject starEffect;
    public int StarMoveTime = 3; //별이 움직일수있는 시간
    GameObject star;
    public int starDistance = 50;
    public int resetTime = 1;
    public float starTime = 1; //별이 starPos까지 가는데 걸리는 시간

    public GameObject starAudio;
    AudioSource starEffectSound;
    bool isStar;
    // Start is called before the first frame update
    void Awake()
    {
        starAudio = GameObject.FindWithTag("StarSound");
        starEffectSound = starAudio.GetComponent<AudioSource>();
    }
    public override async void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            base.OnTriggerEnter2D(collision);

            await UniTask.WaitUntil(() => CameraManager.Instance.isReturnedToPlayer);


            Star(collision.gameObject);
        }
        

        //   itemManager.StartCoroutine(itemManager.PlayerScale(collision.transform, scale, resetTime));

    }

    public async void Star(GameObject collision)
    {
       
        //별빛이 생성
        //캐릭터의 중력 없애기
        collision.GetComponent<Rigidbody2D>().gravityScale = 0;
       // collision.transform.rotation = Quaternion.Euler(0, 0, 0);
        //Debug.Log(collision.GetComponent<Rigidbody2D>().gravityScale);
       /* float currentTime = 0;

        while (currentTime < 1)
        {
            currentTime += Time.deltaTime;
        star.transform.position = Vector3.Lerp(oriPos, startPos, currentTime);
            await UniTask.Yield();

        }*/

        await UniTask.Delay(600);

        Debug.Log("별");
        collision.transform.rotation = Quaternion.Euler(0, 0, 0
            );
        Debug.Log(collision.name);
       // collision.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector3 oriPos = collision.transform.position;
        Vector3 starEffectPos = new Vector3(oriPos.x, oriPos.y + starDistance +54, oriPos.z);

        GameObject effect = Instantiate(starEffect, starEffectPos, Quaternion.Euler(90, 0, 0), collision.transform.parent); //Quaternion.Euler(64, 64, 64));
        isStar = true;
        starEffectSound.Play();
        //Debug.Log(starEffectPos);
       await UniTask.Delay(1 * 1000);
        Vector3 startPos = new Vector3(oriPos.x, oriPos.y + starDistance, oriPos.z);

        star = Instantiate(starImage, startPos, Quaternion.identity, collision.transform.parent); //, collision.transform);
        collision.GetComponent<CharacterMovement2D_LSW>().AddStar(star);
        // collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        //starImage.transform.GetComponent<StarImage_yd>().isOn = true;
        await UniTask.Delay(resetTime * 1000);
       // collision.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        collision.GetComponent<Rigidbody2D>().gravityScale = 1;
        if(star != null)
        Destroy(star);
        collision.GetComponent<CharacterMovement2D_LSW>().RemoveStar();

        //Destroy(starEffect);
        Debug.Log(collision.GetComponent<Rigidbody2D>().gravityScale);

        await UniTask.Yield();

        //Invoke("StopMove", StarMoveTime);

        //Debug.Log(oriPos + "ori");
        //Debug.Log(startPos + "start");

        //러프로 올라갈때 별이 움직이면서 올라가도록 
        //star.transform.GetChild(0).gameObject.SetActive(true); //라이트 혹은 불빛
    }
   /* public void StopMove()
    {
       // starImage.transform.GetComponent<StarImage_yd>().isOn = false;
       // Destroy(star);

    }*/
    // Update is called once per frame
    void Update()
    {
        /*if (isStar)
        {
            starEffectSound.Play();
            isStar = false;
        }*/
    }
}

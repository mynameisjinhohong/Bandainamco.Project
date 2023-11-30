using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Star_yd : BaseItem_LJH
{
    [SerializeField] private GameObject starImage;
    [SerializeField] private GameObject starEffect;
    public int StarMoveTime = 3; //���� �����ϼ��ִ� �ð�
    GameObject star;
    public int starDistance = 50;
    public int resetTime = 1;
    public float starTime = 1; //���� starPos���� ���µ� �ɸ��� �ð�
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Star(collision.gameObject);
        }
        

        //   itemManager.StartCoroutine(itemManager.PlayerScale(collision.transform, scale, resetTime));
        base.OnTriggerEnter2D(collision);

    }

    public async void Star(GameObject collision)
    {
       
        //������ ����
        //ĳ������ �߷� ���ֱ�
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

        await UniTask.Delay(1);
        collision.transform.rotation = Quaternion.Euler(0, 0, 0
            );
        Debug.Log(collision.name);
       // collision.transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
        collision.transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Vector3 oriPos = collision.transform.position;
        Vector3 starEffectPos = new Vector3(oriPos.x, oriPos.y + starDistance +54, oriPos.z);

        GameObject effect = Instantiate(starEffect, starEffectPos, Quaternion.Euler(90, 0, 0)); //Quaternion.Euler(64, 64, 64));
        //Debug.Log(starEffectPos);
       await UniTask.Delay(1 * 1000);
        Vector3 startPos = new Vector3(oriPos.x, oriPos.y + starDistance, oriPos.z);

        star = Instantiate(starImage, startPos, Quaternion.identity); //, collision.transform);
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

        //������ �ö󰥶� ���� �����̸鼭 �ö󰡵��� 
        //star.transform.GetChild(0).gameObject.SetActive(true); //����Ʈ Ȥ�� �Һ�
    }
   /* public void StopMove()
    {
       // starImage.transform.GetComponent<StarImage_yd>().isOn = false;
       // Destroy(star);

    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}

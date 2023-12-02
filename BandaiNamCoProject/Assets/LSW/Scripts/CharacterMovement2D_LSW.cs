using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cinemachine;
public class CharacterMovement2D_LSW : MonoBehaviour
{
    //������
    public GameObject jumpEffect;
    public Transform effectTransform;
    public float jumpPower = 100.0f;
    public float firstJumpPower;
    //���� ������
    public Image jumpIcon;
    public float coolTime = 1f;
    public bool jump = false;
    //������ ���������� ���� �Ұ�
    public ItemManager_LSW itemManager;
    // ������ ������ Ȯ�ο�
    public int? lastUsedItem;
    public float rotateSpeed;
    public AudioSource audioSource;

    //���� ��Ÿ��
    private float firstCoolTime = 0;
    private bool jumpReady = true;
    private Rigidbody2D rb;
    private Animator ani;

    //yd
    public ItemManager_LJH itemMan; //�����۸Ŵ���
    #region ���ɿ�
    public Vector2 minBoundary;
    public Vector2 maxBoundary;
    #endregion
    #region ������
    public List<bool> fish;
    public float fishSpeed = 0f;
    #endregion
    #region �䳢��
    bool rabbitGoing;
    #endregion
    #region ���� ���
    public List<bool> mashroom = new List<bool>();
    public GameObject stunEffect;
    #endregion
    #region ��
    public List<GameObject> starItem = new List<GameObject>();
    #endregion
    #region ����
    public List<int> mushNums = new List<int>();
    [SerializeField] private int mushNum = 0;
    // public List<Vector3> playerScale = new List<Vector3>();*/
    public bool isCoroutine;
    Coroutine coroutine;
    public float mTime;
    public AudioSource mush;
    #endregion
    #region ����
    public bool eyeNow;
    public GameObject idleEye;
    public GameObject fishEye;
    public Animator eyeAni1;
    public Animator eyeAni2;
    bool eyeOn = false;
    #endregion
    public CinemachineVirtualCamera characterCam;
    public StarBackground_yd starBack;
    public float targetOrtho = 70;
    public Vector3 startScale;
    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        ani = GetComponentInChildren<Animator>();
        minBoundary = new Vector2(-(DataManager.Instance.bgSize.x / 2), -(DataManager.Instance.bgSize.y / 2));
        maxBoundary = new Vector2((DataManager.Instance.bgSize.x / 2), (DataManager.Instance.bgSize.y / 2));
        lastUsedItem = null;
        firstCoolTime = coolTime;
        firstJumpPower = jumpPower;
        startScale = transform.GetChild(0).localScale;
    }
    IEnumerator Go_DownDown(Animator an)
    {
        // 0.5�� ���� ��ٸ��� ����.
        yield return new WaitForSeconds(1.0f);
        an.SetTrigger("do_Down");
        //�����Ų �� ��Ȱ��ȭ
    }
    IEnumerator Go_Jump_Second(Animator an)
    {
        // 0.56�� ���� ��ٸ��� ����.
        yield return new WaitForSeconds(0.4f);
        an.SetTrigger("doJump");
        an.SetBool("isJump", false);
        //�����Ų �� ��Ȱ��ȭ
    }

    public void EyeStart(bool eye)
    {
        if (eye)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Fix_Idle_2"))
            {
                eyeNow = true;
                idleEye.SetActive(true);
                fishEye.SetActive(true);
            }
            else
            {
                eyeOn = true;
            }

        }
        else
        {
            eyeNow = false;
            idleEye.SetActive(false);
            fishEye.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Instantiate(jumpEffect, effectTransform);
            //ani.CrossFade("Fly", 0.1f);
            if (mashroom.Count <1)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.GetChild(0).rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                dir.Normalize();
                if (dir.y > 0)
                {
                    rb.velocity = Vector3.zero;
                }
                if (dir != Vector2.zero)
                {
                    rb.AddForce(dir * jumpPower, ForceMode2D.Impulse);

                }
                jumpIcon.fillAmount = 0;
                //jumpCoolText.gameObject.SetActive(true);
                jump = false;
                StartCoroutine(Go_Jump_Second(ani));
                //Fix_Second�� ����
                //Ʈ���� do_Down����
                //�� �� �Ŀ� ������ ���� ����
                StartCoroutine(Go_DownDown(ani));
            }
            else
            {

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.GetChild(0).rotation = Quaternion.AngleAxis(angle - 270, Vector3.forward);
                dir.Normalize();
                rb.velocity = Vector2.zero;
                if (dir != Vector2.zero)
                {
                    rb.AddForce(-dir * jumpPower, ForceMode2D.Impulse);
                }
                jumpIcon.fillAmount = 0;
                jump = false;
                StartCoroutine(Go_Jump_Second(ani));
                StartCoroutine(Go_DownDown(ani));
            }
            if (eyeNow)
            {
                if (idleEye.activeInHierarchy)
                {
                    eyeAni1.SetTrigger("doJump");
                    eyeAni1.SetBool("isJump", true);
                    StartCoroutine(Go_Jump_Second(eyeAni1));
                    StartCoroutine(Go_DownDown(eyeAni1));
                }
                if (fishEye.activeInHierarchy)
                {
                    eyeAni2.SetTrigger("doJump");
                    eyeAni2.SetBool("isJump", true);
                    StartCoroutine(Go_Jump_Second(eyeAni2));
                    StartCoroutine(Go_DownDown(eyeAni2));
                }

            }
        }

    }

    void Update()
    {
        if (rb.velocity.x != rb.velocity.y)
        {
            if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
            {
                transform.GetChild(0).Rotate(Vector3.up * Time.deltaTime * rotateSpeed * rb.velocity.x);
            }
            else
            {
                transform.GetChild(0).Rotate(Vector3.right * Time.deltaTime * rotateSpeed * rb.velocity.y);
            }
        }
        if (eyeOn)
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Fix_Idle_2") || fish.Count > 0)
            {
                eyeNow = true;
                eyeOn = false;
                idleEye.SetActive(true);
                fishEye.SetActive(true);
            }
        }

        // transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //Debug.Log(transform.rotation + "ȸ��?");

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) && jumpReady && fish.Count < 1) //���� ��Ÿ���� ������ ����� ��Ÿ�� ���� ��
            {
                jump = true;
                jumpReady = false;
                ani.SetTrigger("doJump");
                ani.SetBool("isJump", true);
                //ani.CrossFade("Jump", 0.1f);
                StartCoroutine(JumpCoolTime());
            }

        }
        if (fish.Count > 0)
        {
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Fish)
            {
                fish = new List<bool>();
                SetGravity(true);
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                transform.rotation = Quaternion.identity;
                StartCoroutine(RollBackRotation());
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle2 = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            if (Time.timeScale != 0f && (mousePos - (Vector2)transform.position).magnitude > 1f)
            {
                transform.rotation = Quaternion.AngleAxis(angle2 - 90, Vector3.forward);
            }
            transform.position += new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized * Time.deltaTime * fishSpeed;
        }
        if (ItemManager_LJH.Instance.CurrItem != null)
        {
            /*if (ItemManager_LJH.Instance.CurrItem.myItem.itemType == ItemType.Lotus)
            {
                Lotus();
            }*/
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Clock)
            {
                coolTime = firstCoolTime;
            }
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Rabbit)
            {
                jumpPower = firstJumpPower;
            }
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Star)
            {
                if (GameObject.FindWithTag("StarItemPre"))
                {
                    GameObject ob = GameObject.FindWithTag("StarItemPre").gameObject;
                    Destroy(ob);
                }

            }
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType == ItemType.Star)
            {
                //starBack.isOn = true;
                if (starItem.Count > 1)
                {
                    int itemsToRemove = starItem.Count - 1; // ���ܵ� ������ ���� ���

                    for (int i = 0; i < itemsToRemove; i++)
                    {
                        Destroy(starItem[i]);
                        starItem.RemoveAt(i);
                    }
                }

            }
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Mushroom)
            {
                //Debug.Log("�ӽ�����");
                if (isCoroutine)
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                        mushNum = 0;
                        mushNums.Clear();
                    }
                    isCoroutine = false;

                    Vector3 nowScale = transform.GetChild(0).localScale;
                    float now = characterCam.m_Lens.OrthographicSize;
                    if (ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Train)
                    {
                        StartCoroutine(OriginScale(nowScale, 4, now)); //�ӽ��� Ÿ���� �ӽ��� ��ũ��Ʈ���� ���ڹٲ𶧰��� �����������
                    }
                    else
                    {
                        transform.GetChild(0).localScale = startScale;
                        float oriOrtho = 50;
                        characterCam.m_Lens.OrthographicSize = oriOrtho;
                    }

                }


            }

        }

    }
    IEnumerator OriginScale(Vector3 nowScale, float mTime, float now)
    {
        //Debug.Log("????????????");
        float currentTime = 0;
        float oriOrtho = 50;
        Vector3 oriScale = startScale;
        while (currentTime < mTime)
        {
            transform.GetChild(0).localScale = Vector3.Lerp(nowScale, oriScale, currentTime / mTime);
            characterCam.m_Lens.OrthographicSize = Mathf.Lerp(now, oriOrtho, currentTime / mTime);

            //yield return null;
            currentTime += Time.deltaTime;
            yield return null;

            //Debug.Log("�۾���");

        }
        characterCam.m_Lens.OrthographicSize = oriOrtho;

        transform.GetChild(0).localScale = oriScale;
        //Debug.Log(transform.localScale + "local");
        yield return null;
    }
    public void PlayerScale(Transform tr, float scale, int resetTime, float mashroomTime, GameObject mashroomEffect)
    {
        if (isCoroutine)
        {
            isCoroutine = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        coroutine = StartCoroutine(PlayerScaleEvent(tr, scale, resetTime, mashroomTime, mashroomEffect));
    }
    IEnumerator PlayerScaleEvent(Transform targetTr, float scale, int resetTime, float mashroomTime, GameObject mashroomEffect)
    {
        isCoroutine = true;
        Vector3 oriScale = startScale;
        targetTr.GetChild(0).localScale = oriScale;
        Vector3 targetScale = new Vector3(oriScale.x * scale, oriScale.y * scale, oriScale.z * scale);
        float currentTime = 0;
        float oriOrtho = 50;
        mushNum++;
        mushNums.Add(mushNum);
        //ó������ 
        if (mushNums.Count < 2)
        {

            yield return new WaitForSeconds(0.6f);
            yield return new WaitUntil(()=> CameraManager.Instance.isReturnedToPlayer);
            targetTr.GetChild(0).localScale = targetScale;
            //  float targetOrtho = 70;
            //GameObject me = Instantiate(mashroomEffect, targetTr.transform.parent, );
            // Vector3 newPosition = new Vector3(0, 1, -8.0f);
            GameObject me = Instantiate(mashroomEffect, targetTr.transform);
            mush.Play();
            Debug.Log("��������");
            // me.transform.parent = targetTr.transform;
            while (currentTime < mashroomTime)
            {
                targetTr.GetChild(0).localScale = Vector3.Lerp(oriScale, targetScale, currentTime / mashroomTime);
                characterCam.m_Lens.OrthographicSize = Mathf.Lerp(oriOrtho, targetOrtho, currentTime / mashroomTime);
                currentTime += Time.deltaTime;
                //Debug.Log("Ŀ��");
                yield return null;

            }
        }

        targetTr.GetChild(0).localScale = targetScale;
        characterCam.m_Lens.OrthographicSize = targetOrtho;
        resetTime *= mushNums.Count;
        //  targetTr.GetComponent<CharacterMovement2D_LSW>().AddMushroom(oriScale, false);
        //Vector3 originalScale = targetTr.localScale;

        yield return new WaitForSeconds(resetTime);


        /* if (targetTr.GetComponent<CharacterMovement2D_LSW>().isDone[targetTr.GetComponent<CharacterMovement2D_LSW>().isDone.Count - 1] == false)
         {
             Debug.Log("�ߴ�");
             return;
         }*/
        //yield return new WaitForSeconds(resetTime);
        //Debug.Log("�����ð�");
        currentTime = 0f;


        while (currentTime < mashroomTime)
        {
            targetTr.GetChild(0).localScale = Vector3.Lerp(targetScale, oriScale, currentTime / mashroomTime);
            characterCam.m_Lens.OrthographicSize = Mathf.Lerp(targetOrtho, oriOrtho, currentTime / mashroomTime);

            currentTime += Time.deltaTime;
            //yield return null;
            yield return null;

            //Debug.Log("�۾���");

        }
        targetTr.GetChild(0).localScale = oriScale;
        characterCam.m_Lens.OrthographicSize = oriOrtho;
        yield return null;
        mushNums.Clear();
        mushNum = 0;
        isCoroutine = false;

    }
    public IEnumerator RollBackRotation()
    {
        Vector3 first = transform.eulerAngles;
        float cur = 0;
        while (true)
        {
            cur += Time.deltaTime;
            yield return null;
            transform.eulerAngles = Vector3.Lerp(first, Vector3.zero, cur / 2f);
            if (transform.eulerAngles == Vector3.zero)
            {
                break;
            }
        }
    }

    public void AddStar(GameObject obj)
    {
        starItem.Add(obj);
    }
    public void RemoveStar()
    {
        if (starItem.Count > 1)
            starItem.RemoveAt(0);
    }

    public void Rabbit(float moreJump, float time)
    {
        jumpPower *= moreJump;
        StopCoroutine("RabbitCo");
        StartCoroutine(RabbitCo(time));

    }
    IEnumerator RabbitCo(float time)
    {
        yield return new WaitForSeconds(time);
        jumpPower = firstJumpPower;
    }


    void Lotus()//���� ���
    {
        // Get the player's position
        if (transform.position.x < minBoundary.x || transform.position.x > maxBoundary.x || transform.position.y < minBoundary.y || transform.position.y > maxBoundary.y)
        {
            transform.position = Vector3.zero;
            rb.velocity = Vector3.zero;
            //���� �ִϸ��̼� �߰��ؾߵ�.
        }
    }
    IEnumerator JumpCoolTime()
    {
        float currentTime = 0;
        while (true)
        {
            yield return null;
            currentTime += Time.deltaTime;
            jumpIcon.fillAmount = currentTime / coolTime;
            if (currentTime > coolTime)
            {
                break;
            }

        }
        jumpReady = true;
    }

    public void SetGravity(bool hasGravity)
    {
        rb.gravityScale = hasGravity == true ? 1 : 0;
    }

    public void AddVelocity(Vector2 vel, bool reset = false)
    {
        //if (reset) ResetVelocity();
        rb.velocity += vel;
    }

    public void ResetVelocity()
    {
        rb.velocity = Vector2.zero;
    }


    public void Reset()
    {
        coolTime = firstCoolTime;
        jumpPower = firstJumpPower;
        SetGravity(true);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetVelocity(Vector2 vel)
    {
        rb.velocity = vel;
    }

    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    public void PlayAudio()
    {
        audioSource.Stop();
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}

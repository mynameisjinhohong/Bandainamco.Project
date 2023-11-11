using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterMovement2D_LSW : MonoBehaviour
{
    //������
    public float jumpPower = 100.0f;
    public float firstJumpPower;
    //���� ������
    public Image jumpIcon;
    public TMP_Text jumpCoolText;
    //���� ��Ÿ��
    public float coolTime = 1f;
    float firstCoolTime = 0;
    //������ ���������� ���� �Ұ�
    bool jumpReady = true;
   public bool jump = false;
    private Rigidbody2D rb;
    Animator ani;
    public ItemManager_LSW itemManager;
    // ������ ������ Ȯ�ο�
    public int? lastUsedItem;

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
    public bool mashroom, mashroomBach;
    #endregion
    #region ��
    public List<GameObject> starItem = new List<GameObject>();
    #endregion
    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        ani =GetComponentInChildren<Animator>();
        minBoundary = new Vector2(-(itemManager.bgSize.x / 2) , -(itemManager.bgSize.y / 2));
        maxBoundary = new Vector2((itemManager.bgSize.x / 2), (itemManager.bgSize.y / 2));
        lastUsedItem = null;
        firstCoolTime = coolTime;
        firstJumpPower = jumpPower;
    }

    private void FixedUpdate()
    {
        if (jump)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            dir.Normalize();
            if(dir.y > 0)
            {
                rb.velocity = Vector3.zero;
            }
            if(dir!= Vector2.zero)
            {
                rb.AddForce(dir * jumpPower,ForceMode2D.Impulse);
                                
            }
            jumpIcon.fillAmount = 0;
            jumpCoolText.gameObject.SetActive(true);
            jump = false;
            ani.SetBool("jump",false);
            //ani.CrossFade("Fly", 0.1f);
        }
      if(mashroom)
        {
            if(mashroomBach)
            {
                Debug.Log("����");
                Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                //  Vector2 direction = dir - (Vector2)transform.position;
                //  direction = -direction;
               // dir = -dir;
                dir.Normalize();
                // ���� ������ �ݴ�� �ٲٱ�

                if (dir.y > 0)
                {
                    rb.velocity = Vector2.zero;
                }

                if (dir != Vector2.zero)
                {
                    rb.AddForce(-dir * jumpPower, ForceMode2D.Impulse);
                }
                jumpIcon.fillAmount = 0;
                jumpCoolText.gameObject.SetActive(true);
                mashroomBach = false;
                ani.SetBool("jump", false);
                mashroomBach = false;
            }
           


        }
    }

    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;

        // transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        //Debug.Log(transform.rotation + "ȸ��?");

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) && jumpReady && fish.Count <1) //���� ��Ÿ���� ������ ����� ��Ÿ�� ���� ��
            {
                if(!mashroom) //�������ƴҶ� �߰�
                {
                    jump = true;
                    jumpReady = false;
                    ani.SetBool("jump", true);
                    //ani.CrossFade("Jump", 0.1f);
                    StartCoroutine(JumpCoolTime());
                }
                
            }
            if(Input.GetMouseButton(0) && mashroom)
            {
                mashroomBach = true;
                jumpReady = false;
                ani.SetBool("jump", true);
                StartCoroutine(JumpCoolTime());

            }
        }
        if (fish.Count >0)
        {
            if(ItemManager_LJH.Instance.CurrItem.myItem.itemType != ItemType.Fish)
            {
                fish = new List<bool>();
                SetGravity(true);
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(false);
                gameObject.transform.rotation = Quaternion.identity;
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle2 = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            if(Time.timeScale != 0f && (mousePos - (Vector2)transform.position).magnitude > 1f )
            {
                transform.rotation = Quaternion.AngleAxis(angle2 - 180, Vector3.forward);
            }
            transform.position += new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized * Time.deltaTime * fishSpeed;
        }
        if(ItemManager_LJH.Instance.CurrItem != null)
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
                if(GameObject.FindWithTag("StarItemPre"))
                 {
                    GameObject ob = GameObject.FindWithTag("StarItemPre").gameObject;
                    Destroy(ob);
                }

            }
            if (ItemManager_LJH.Instance.CurrItem.myItem.itemType == ItemType.Star)
            {
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

        }
       
    }

    public void AddStar(GameObject obj)
    {
        starItem.Add(obj);
    }
    public void RemoveStar()
    {
        if(starItem.Count>1)
        starItem.RemoveAt(0);
    }
    
    public void Rabbit(float moreJump,float time)
    {
        jumpPower += moreJump;
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
            jumpIcon.fillAmount = currentTime/coolTime;
            jumpCoolText.text = string.Format("{0:N1}",coolTime - currentTime);
            if(currentTime > coolTime)
            {
                break;
            }

        }
        jumpCoolText.gameObject.SetActive(false);
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

}

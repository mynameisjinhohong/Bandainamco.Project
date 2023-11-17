using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow_Animation : MonoBehaviour
{
    //������ ������ ��, Animator���� 8���� �ִϸ��̼��� �־�� ��
    // ����, Fly, Fly2, Fly3, 0_Idle, Idle, Jump, Land
    //Jump�� Land�� �� ��Ʈ��� ���� �˴ϴ�

    //���� ������ �κ�: �����̽� �ٸ� ������ ���:  ���� -> ������ ����(���� ���ڸ� ������ ����)
    //������ AnyState �� �����ϴ� ���� ���� ���̰�. ū ������ ������.

    //���� �ܿ� �ٸ� �ִϸ��̼��� �ִϸ��̼Ǹ� �־���Ұ� ���� �����ų ������ Animator���� �������� ����.
    //Entry�� ����Ǿ� �ִ� '����'�ִϸ��̼��� ��¥ �ƹ� �ִϸ��̼ǵ� ���� ���̴� �����ּ���...

    // +) ������ ���..!!


    //�ִϸ��̼� ������ ������ ���, ��Ȱ��ȭ �ص� Sphere Collider�� Ȱ��ȭ��Ű�� ���� �� ������ ��...!

    //public float speed;
    //float hAxis;
    //float vAxis;
    //bool wDown;
    bool jDown;//������ ���� �Ȱ�. ������ ���� �ƴ϶�� ������ �ʴ� ���� ����

    //Vector3 moveVec;
    Animator animator;

    bool isJump;
    Rigidbody rigid;


    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();
        //Move();
        //Turn();
        Jump();
    }

    void GetInput()
    {
        // hAxis = Input.GetAxisRaw("Horizontal");
        //vAxis = Input.GetAxisRaw("Vertical");
        // wDown = Input.GetButton("Walk");//�� �� �� ���Ƽ� �ּ�ó��
        jDown = Input.GetButton("Jump");//���� �̰͸� ����� �Ǵ� ����

    }

    /* void Move()
     {

         moveVec = new Vector3(hAxis, 0, vAxis).normalized;
         transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;//�� �� �� ���Ƽ� �ּ�ó��

         animator.SetBool("isRun", moveVec != Vector3.zero); 

         //animator.SetBool("isWalk", wDown); //�� �� �� ���Ƽ� �ּ�ó��
     }*/

    /*void Turn()
    {//ĳ���Ͱ� �ٶ󺸴� ������ �޶����� ���� ��Ÿ���� ����.
     //(��, ��, ��, ���������� �̵��ϴ� �Ϳ� ���� ĳ���Ͱ� �ٶ󺸴� ������ �޶���)
       
        transform.LookAt(transform.position + moveVec);
    }*/

    void Jump()//���� �����Ű�� �ڵ�
    {
        //������ ������ && �������� �ƴҶ�
        if (jDown && !isJump)
        {
            //animator.SetTrigger("Jump_Ready");
            //animator.SetBool("isJump", true);
            //isJump = true;

            //���� AddForce
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);// ���� 20�� �����غ��� �����غ��� �ɷ�
            //���ڰ� Ŀ������ �����ϴ� ���̰� ������...

            animator.SetTrigger("doJump");
            animator.SetBool("isJump", true);
            isJump = true;


            //���� ���� ���� �������� �ִϸ��̼� ����
            //do_Jump��
            //isJump�� false��
            //�� �� �Ŀ� ������ ���� ����
            StartCoroutine(Go_Jump_Second());
            IEnumerator Go_Jump_Second()
            {
                // 0.56�� ���� ��ٸ��� ����.
                yield return new WaitForSeconds(0.4f);
                animator.SetTrigger("doJump");
                animator.SetBool("isJump", false);
                //�����Ų �� ��Ȱ��ȭ
            }

            //Fix_Second�� ����
            //Ʈ���� do_Down����
            //�� �� �Ŀ� ������ ���� ����
            StartCoroutine(Go_DownDown());
            IEnumerator Go_DownDown()
            {
                // 0.5�� ���� ��ٸ��� ����.
                yield return new WaitForSeconds(1.0f);
                animator.SetTrigger("do_Down");
                //�����Ų �� ��Ȱ��ȭ
                isJump = false;
            }
        }
    }

    //�ٴ� �浹 �˻�(�ٴ��� Cube�� ���� ��������� �ǳ� �غ���. �����ؾ� �� ��?)
    private void OnCollisionEnter(Collision collision)
    {//�ٴڿ� ����� ��(Floor �±װ� �Ǿ��ִ� ������Ʈ�� ����� ��) �����ϴ� �ִϸ��̼��� ��������

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJump", false);
            isJump = false;
            Debug.Log("��Ҵ�");//Floor������Ʈ�� ��Ҵ��� Ȯ���ϱ� ���� Debug.Log
        }
    }
}

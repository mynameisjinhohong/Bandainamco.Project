using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillow_Animation : MonoBehaviour
{
    //파일을 열었을 때, Animator에서 8개의 애니메이션이 있어야 함
    // 없음, Fly, Fly2, Fly3, 0_Idle, Idle, Jump, Land
    //Jump와 Land는 한 세트라고 보면 됩니다

    //현재 구현한 부분: 스페이스 바를 눌렀을 경우:  점프 -> 착지가 실행(현재 제자리 점프만 가능)
    //점프는 AnyState 와 연결하는 것이 좋아 보이고. 큰 오류가 없었음.

    //점프 외에 다른 애니메이션은 애니메이션만 넣어놓았고 언제 실행시킬 건지는 Animator에서 만들어놓지 않음.
    //Entry와 연결되어 있는 '없음'애니메이션은 진짜 아무 애니메이션도 없는 것이니 지워주세요...

    // +) 베개는 흰색..!!


    //애니메이션 동작을 수정할 경우, 비활성화 해둔 Sphere Collider를 활성화시키면 조금 더 수월할 것...!

    //public float speed;
    //float hAxis;
    //float vAxis;
    //bool wDown;
    bool jDown;//점프와 관련 된것. 수정할 것이 아니라면 지우지 않는 편이 좋음

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
        // wDown = Input.GetButton("Walk");//안 쓸 것 같아서 주석처리
        jDown = Input.GetButton("Jump");//지금 이것만 제대로 되는 상태

    }

    /* void Move()
     {

         moveVec = new Vector3(hAxis, 0, vAxis).normalized;
         transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;//안 쓸 것 같아서 주석처리

         animator.SetBool("isRun", moveVec != Vector3.zero); 

         //animator.SetBool("isWalk", wDown); //안 쓸 것 같아서 주석처리
     }*/

    /*void Turn()
    {//캐릭터가 바라보는 방향이 달라지는 것을 나타내고 있음.
     //(앞, 뒤, 왼, 오른쪽으로 이동하는 것에 따라 캐릭터가 바라보는 방향이 달라짐)
       
        transform.LookAt(transform.position + moveVec);
    }*/

    void Jump()//점프 실행시키는 코드
    {
        //점프가 눌릴때 && 점프중이 아닐때
        if (jDown && !isJump)
        {
            //animator.SetTrigger("Jump_Ready");
            //animator.SetBool("isJump", true);
            //isJump = true;

            //위로 AddForce
            rigid.AddForce(Vector3.up * 20, ForceMode.Impulse);// 숫자 20은 실행해보고 수정해보는 걸로
            //숫자가 커질수록 점프하는 높이가 높아짐...

            animator.SetTrigger("doJump");
            animator.SetBool("isJump", true);
            isJump = true;


            //점프 이후 이제 내려가는 애니메이션 실행
            //do_Jump만
            //isJump는 false로
            //몇 초 후에 실행할 건지 보기
            StartCoroutine(Go_Jump_Second());
            IEnumerator Go_Jump_Second()
            {
                // 0.56초 동안 기다리고 실행.
                yield return new WaitForSeconds(0.4f);
                animator.SetTrigger("doJump");
                animator.SetBool("isJump", false);
                //실행시킨 거 비활성화
            }

            //Fix_Second로 가기
            //트리거 do_Down실행
            //몇 초 후에 실행할 건지 보기
            StartCoroutine(Go_DownDown());
            IEnumerator Go_DownDown()
            {
                // 0.5초 동안 기다리고 실행.
                yield return new WaitForSeconds(1.0f);
                animator.SetTrigger("do_Down");
                //실행시킨 거 비활성화
                isJump = false;
            }
        }
    }

    //바닥 충돌 검사(바닥은 Cube를 만들어서 실험용으로 되나 해봤음. 수정해야 할 듯?)
    private void OnCollisionEnter(Collision collision)
    {//바닥에 닿았을 때(Floor 태그가 되어있는 오브젝트와 닿았을 때) 착지하는 애니메이션이 나오도록

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool("isJump", false);
            isJump = false;
            Debug.Log("닿았다");//Floor오브젝트에 닿았는지 확인하기 위한 Debug.Log
        }
    }
}

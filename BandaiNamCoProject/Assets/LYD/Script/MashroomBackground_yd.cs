using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashroomBackground_yd : MonoBehaviour
{
    float currentTime = 0;
    public float createTime = 2;
    Animator myAni;
    bool now = false;
    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!now)
            {
                //Debug.Log("?");
                //(collision.gameObject);
                now = true;
                myAni.SetTrigger("Act");
                //Debug.Log("1");
                collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom.Add(true);
                if(collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom.Count == 1)
                {
                    collision.gameObject.GetComponent<CharacterMovement2D_LSW>().stunEffect.SetActive(true);
                }
                //Debug.Log("2");
                StartCoroutine(BackgroundTime(collision.gameObject));
            }
        }

        //   itemManager.StartCoroutine(itemManager.PlayerScale(collision.transform, scale, resetTime));
      //  base.OnTriggerEnter2D(collision);

    }
    IEnumerator BackgroundTime(GameObject collision)
    {
       // Debug.Log("3");
       yield return new WaitForSeconds(createTime);
    /*  while(currentTime < createTime)
        {
            currentTime += Time.deltaTime;
            //collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom = true;
            //collision.gameObject.GetComponent<CharacterMovement2D_LSW>().jump = false;

            yield return null;
            Debug.Log("4");
          
        }*/
    //Debug.Log("Z");
       collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom.RemoveAt(0);
    if(collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom.Count < 1)
        {
            collision.gameObject.GetComponent<CharacterMovement2D_LSW>().stunEffect.SetActive(false);
        }
       now = false;
    }

    //public 
    // Update is called once per frame
    void Update()
    {
      /*  if(collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom))
        {

        }*/

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashroomBackground_yd : MonoBehaviour
{
    float currentTime = 0;
    public float createTime = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("?");
            //(collision.gameObject);
            collision.gameObject.GetComponent<CharacterMovement2D_LSW>().jump = false;
            //Debug.Log("1");
            collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom = true;
            collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroomBach = true;

            //Debug.Log("2");
            StartCoroutine(BackgroundTime(collision.gameObject));
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
       collision.gameObject.GetComponent<CharacterMovement2D_LSW>().jump = true;
    //Debug.Log("Z");
       collision.gameObject.GetComponent<CharacterMovement2D_LSW>().mashroom = false;
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

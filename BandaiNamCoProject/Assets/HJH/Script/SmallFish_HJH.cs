using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFish_HJH : MonoBehaviour
{
    public Transform originTransform;
    public float speed;
    public float power;
    Vector3 foward;
    AudioSource myAudio;
    // Start is called before the first frame update
    void Start()
    {
        foward = transform.position - originTransform.position; 
        float angle = Mathf.Atan2(foward.y, foward.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle-90, Vector3.forward);
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += foward.normalized * speed * Time.deltaTime;
        if(Mathf.Abs(transform.position.x) > DataManager.Instance.bgSize.x || Mathf.Abs(transform.position.y) > DataManager.Instance.bgSize.y)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&collision.gameObject.GetComponent<CharacterMovement2D_LSW>().fish.Count <1)
        {
            myAudio.Play();
            Rigidbody2D rigid = collision.transform.gameObject.GetComponent<Rigidbody2D>();
            rigid.velocity = Vector3.zero;
            rigid.AddForce(power * (Vector2)((collision.transform.position - transform.position).normalized), ForceMode2D.Impulse);
        }
    }
}

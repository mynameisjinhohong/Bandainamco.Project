using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class SmallRabbit_HJH : MonoBehaviour
{
    public float duringTime;
    public float jumpPowerPlus;
    public float speedMax;
    public float speedMin;
    public  bool start = false;
    public bool Item;
    public RabbitBG_HJH rabbitBg;
    // Start is called before the first frame update
    void Start()
    {
        transform.SetSiblingIndex(Random.Range(0, transform.parent.childCount));
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(start)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.GetComponent<CharacterMovement2D_LSW>().Rabbit(jumpPowerPlus,duringTime);
                if (Item)
                {
                    transform.parent.GetComponent<RabbitItrm_HJH>().end = true;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {

        if (Item)
        {
            StartCoroutine(Move());
        }
        else
        {
            transform.SetSiblingIndex(Random.Range(transform.parent.childCount - 5, transform.parent.childCount));
        }
    }

    public IEnumerator Move()
    {
        Vector2 ran = Random.insideUnitCircle;
        ran.Normalize();
        float speed = Random.Range(speedMin, speedMax);
        float currentTime = 0;
        while (true)
        {
            currentTime += Time.deltaTime;
            if(currentTime > 0.5f)
            {
                start = true;
            }
            transform.position += (Vector3)ran * Time.deltaTime * speed;
            yield return null;
        }
    }

    
}

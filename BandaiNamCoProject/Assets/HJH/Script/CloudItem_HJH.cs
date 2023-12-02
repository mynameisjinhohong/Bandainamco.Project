using UnityEngine;

public class CloudItem_HJH : BaseItem_LJH
{
    public GameObject smallCloud;
    public int cloudNum;
    public Vector2 jumpPower;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CloudItemActivate(other.gameObject);
        }
        base.OnTriggerEnter2D(other);
    }

    public void CloudItemActivate(GameObject player)
    {
        for (int i = 0; i < cloudNum; i++)
        {
            GameObject small = Instantiate(smallCloud, transform.position, Quaternion.identity);
            small.GetComponent<SmallCloud_HJH>().goTransform = transform.GetChild(i).position;  
        }
        audioSource.Play();
        Rigidbody2D rigidbody2D = player.GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.AddForce(jumpPower, ForceMode2D.Impulse);
    }
}

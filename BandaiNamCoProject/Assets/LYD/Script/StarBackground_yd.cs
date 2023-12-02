using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class StarBackground_yd : MonoBehaviour
{
 //   [SerializeField] float m_force = 50f;
    // [SerializeField] Vector3 m_offset = Vector3.zero;
  //  [SerializeField] Vector3 m_offset = new Vector3(10, 10, 10);

   // [SerializeField] private 

    Quaternion m_originRot;
    [SerializeField] private GameObject starPre;
    float currentTime = 0;
    public float createTime = 10;
    public bool isOn;
    public CinemachineVirtualCamera characterCam;
    public bool isFirst;
    public float waitTime;
    public bool isMake;
    public float starTime; //스타유지시간
    bool isCoroutine;
    //임펄스
     CinemachineImpulseSource impulse;
    [SerializeField] private GameObject player;
    public float x;
    public float y;
    public float twinkleTime; //몇초에껐다켜지는지
    private GameObject twinkleStar;
    public AudioSource starBGSound;
    // Start is called before the first frame update
    void Start()
    {
        impulse = transform.GetComponent <CinemachineImpulseSource>();
        twinkleStar = transform.GetChild(1).gameObject;
    }
    private void OnEnable()
    {
        isFirst = true;
        starBGSound.Play();
        StartCoroutine(TwinkleStar());
    }
    IEnumerator TwinkleStar()
    {
        while(true)
        {
            yield return new WaitForSeconds(twinkleTime);
            twinkleStar.SetActive(false);
            yield return new WaitForSeconds(twinkleTime);
            twinkleStar.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (isOn)

        {
            //별 생성
            if(isFirst)
            {
                Vector3 camPos = player.transform.position;
                camPos.x += 40;
                camPos.y += 50;
                GameObject star = Instantiate(starPre, camPos, starPre.transform.rotation, player.transform);
                isFirst = false;
                impulse.GenerateImpulse();

              /*  isMake = true;
                isFirst = false;*/
            }

            currentTime += Time.deltaTime;
            if (currentTime > createTime)
            {
                Vector3 camPos = player.transform.position;
                camPos.x += x;
                camPos.y += y;
                GameObject star = Instantiate(starPre, camPos, starPre.transform.rotation, player.transform);
                impulse.GenerateImpulse();

                Debug.Log("별생성");
                    currentTime = 0;
                
            }
       
        }

    }
    IEnumerator ShakeCoroutine()
    {
        isCoroutine = true;
        
        if(isFirst)
        {
            yield return new WaitForSeconds(waitTime);
        }
        if(isMake)
        {
            float currentTime = 0;
            Vector3 t_originEuler = characterCam.transform.eulerAngles;
            /*while (currentTime < starTime)
            {
                  float t_rotX = Random.Range(-m_offset.x, m_offset.x);
                   float t_rotY = Random.Range(-m_offset.y, m_offset.y);
                   float t_rotZ = Random.Range(-m_offset.z, m_offset.z);

                   Vector3 t_randomRot = t_originEuler + new Vector3(t_rotX, t_rotY, t_rotZ);
                   Quaternion t_rot = Quaternion.Euler(t_randomRot);
                   while (Quaternion.Angle(characterCam.transform.rotation, t_rot) > 0.1f)
                   {
                       characterCam.transform.rotation = Quaternion.RotateTowards(characterCam.transform.rotation, t_rot, m_force * Time.deltaTime);
                   currentTime += Time.deltaTime;
                       yield return null;
                   }
                currentTime += Time.deltaTime;
                yield return null;
            }*/
                impulse.GenerateImpulse();
            Debug.Log("??????");
             //   isMake = false;
        }
       // characterCam.transform.rotation = m_originRot;
        //StartCoroutine(Reset());

    }


}

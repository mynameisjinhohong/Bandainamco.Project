using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class StarBackground_yd : MonoBehaviour
{
    [SerializeField] float m_force = 50f;
    // [SerializeField] Vector3 m_offset = Vector3.zero;
    [SerializeField] Vector3 m_offset = new Vector3(10, 10, 10);

    [SerializeField] private 

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
    // Start is called before the first frame update
    void Start()
    {
        m_originRot = transform.rotation;
    }
    private void OnEnable()
    {
        isFirst = true;
    }
    // Update is called once per frame
    void Update()
    {

        if (isOn)

        {
            //별 생성
            if(isFirst)
            {

                GameObject star = Instantiate(starPre, gameObject.transform);

                isMake = true;
                isFirst = false;
            }

            currentTime += Time.deltaTime;
            if (currentTime > createTime)
            {
                Vector3 camPos = new Vector3(characterCam.transform.position.x + 10, characterCam.transform.position.y + 10, characterCam.transform.position.z);
                    GameObject star = Instantiate(starPre, characterCam.transform);
                star.transform.position = camPos;

                isMake = true;
                isCoroutine = false;
                    currentTime = 0;
                
            }
            if(isMake &&!isCoroutine)
            {
                StartCoroutine(ShakeCoroutine());

            }
            if(!isMake)
            {
                //Debug.Log("aaaaaaaaaaaaaaaa");
                StopAllCoroutines();
                // StartCoroutine(ResetEvent());
                characterCam.transform.rotation = m_originRot;

            }
            /*            if (gameObject.transform.childCount == 1)
                        {
                            Debug.Log("Dddddd");
                            StartCoroutine(ShakeCoroutine());
                        }
                        else if (gameObject.transform.childCount == 0)
                        {
                            StopAllCoroutines();
                            StartCoroutine(Reset());
                        }*/
            //   }
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
            while (currentTime < starTime)
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
                yield return null;
            }
                isMake = false;
        }
        characterCam.transform.rotation = m_originRot;
        //StartCoroutine(Reset());

    }
    /*IEnumerator ResetEvent()
    {
        Debug.Log("reset");
        while (Quaternion.Angle(characterCam.transform.rotation, m_originRot) > 0.1f)
        {
            characterCam.transform.rotation = Quaternion.RotateTowards(characterCam.transform.rotation, m_originRot, m_force * Time.deltaTime);
            yield return null;
        }
    }*/

}

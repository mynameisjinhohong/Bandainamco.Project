using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UserData_HJH
{
    public int stage;
    public float volume = 1f;
    public UserData_HJH()
    {
        stage = 0;
        volume = 1f;
    }
}

public class GameManager : MonoBehaviour
{
    public UserData_HJH userData;
    public static GameManager instance = null;

    #region 볼륨 조절 관련
    [SerializeField]
    float volume;

    public List<AudioSource> audios;

    public float Volume
    {
        get
        {
            return volume;
        }
        set
        {
            volume = value;
            if (volume < 0)
            {
                volume = 0;
            }
            else if (volume > 1)
            {
                volume = 1;
            }
            for (int i = 0; i < audios.Count; i++)
            {
                if (audios[i] != null)
                {
                    audios[i].volume = volume;
                }
                else
                {
                    audios.RemoveAt(i);
                    i--;
                }
            }
            userData.volume = volume;
            SaveUserData();
        }
    }
    private void FindAudioSource()
    {
        List<AudioSource> audioSources = new List<AudioSource>();
        GameObject[] all = FindObjectsOfType<GameObject>();
        AudioSource myAudio = gameObject.GetComponent<AudioSource>();
        foreach (GameObject obj in all)
        {
            AudioSource audio;
            if (obj.TryGetComponent<AudioSource>(out audio))
            {
                if (audio != myAudio)
                {
                    audioSources.Add(audio);
                    audio.volume = volume;
                }
            }
        }
        audios = audioSources;
    }
    #endregion
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        string data = PlayerPrefs.GetString("UserData");
        if (data.Length > 1)
        {
            userData = JsonUtility.FromJson<UserData_HJH>(data);
        }
        else
        {
            userData = new UserData_HJH();
        }
        Volume = userData.volume;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAudioSource();
    }
    public void SaveUserData()
    {
        string data = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData", data);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

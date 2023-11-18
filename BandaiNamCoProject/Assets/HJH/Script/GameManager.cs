using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class DataMenu
{
    [MenuItem("Data/DataClear")]
    public static void DataClear()
    {
        PlayerPrefs.DeleteAll();
    }
}

[System.Serializable]
public class UserData_HJH
{
    public int stage;
    public float volume = 1f;
    public StageData_HJH[] stageDatas = new StageData_HJH[4];
    public int langaugeSet = 0;
    public UserData_HJH()
    {
        stage = 0;
        volume = 1f;
        stageDatas = new StageData_HJH[4];
        langaugeSet = 0;
    }
}
[System.Serializable]
public class StageData_HJH
{
    public bool[] itemOnOff = new bool[10];
}


public class GameManager : MonoBehaviour
{
    public int[] itemCount;
    public UserData_HJH userData;
    public static GameManager instance = null;

    #region 언어 관련
    //public const string enFont;
    //public const string koFont;
    //public const string jaFont;

    public void ChangeFont()
    {
        List<LocalizeTextSet_HJH> texts = new List<LocalizeTextSet_HJH>();
        GameObject[] all = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in all)
        {
            LocalizeTextSet_HJH text;
            if (obj.TryGetComponent<LocalizeTextSet_HJH>(out text))
            {
                text.SetLangauge();
            }
        }
    }
    #endregion

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
            if (userData.stageDatas[0] == null)
            {
                for (int i = 0; i < userData.stageDatas.Length; i++)
                {
                    userData.stageDatas[i] = new StageData_HJH();
                    userData.stageDatas[i].itemOnOff = new bool[itemCount[i]];
                }
            }
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[userData.langaugeSet];
        Volume = userData.volume;
    }

    public void LangaugeSet(int langaugeIdx)
    {
        userData.langaugeSet = langaugeIdx;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[langaugeIdx];
        ChangeFont();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindAudioSource();
        //ChangeFont();
    }
    public void SaveUserData()
    {
        string data = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString("UserData", data);
        SaveUserData();
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

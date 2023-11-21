using TMPro;
using UnityEngine;

public class LocalizeTextSet_HJH : MonoBehaviour
{
    public TMP_FontAsset ko;
    public TMP_FontAsset ja;
    public TMP_FontAsset en;
    TMP_Text mytext;
    // Start is called before the first frame update
    void Awake()
    {
        mytext = GetComponent<TMP_Text>();
        SetLangauge();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLangauge()
    {
        switch (GameManager.instance.userData.langaugeSet)
        {
            
            case 0:
                mytext.font = en;
                break;
            case 1:
                mytext.font = ja;
                break;
            case 2:
                mytext.font = ko;
                break;
        }
    }
}

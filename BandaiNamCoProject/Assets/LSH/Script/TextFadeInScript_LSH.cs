using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeInScript_LSH : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text text;

	IEnumerator Fade()
    {
        Color textAlpha = text.color;
		for (float alpha = 0f; alpha <= 1; alpha += 0.1f)
		{
			textAlpha.a = alpha;
			text.color = textAlpha;
            yield return new WaitForSeconds(0.1f);
		}
	}

	void Start()
    {
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

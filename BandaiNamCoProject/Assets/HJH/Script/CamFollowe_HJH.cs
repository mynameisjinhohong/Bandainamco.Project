using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowe_HJH : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public GameObject player;
    public float smoothing = 0.2f;
    public GameObject bg;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;
    public bool camFollow = true;
    public float firstCamSize;
    // Start is called before the first frame update
    void Start()
    {
        firstCamSize = Camera.main.orthographicSize;
        Vector3 bgSize = GetBGSize(bg);
        minCameraBoundary = new Vector2(-(bgSize.x/2) + Camera.main.orthographicSize* Screen.width/Screen.height, -(bgSize.y/2) + Camera.main.orthographicSize);
        maxCameraBoundary = new Vector2((bgSize.x / 2) - (Camera.main.orthographicSize * Screen.width / Screen.height), (bgSize.y / 2) - Camera.main.orthographicSize);
        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    // Update is called once per frame
    void Update()
    {



    }
    private void LateUpdate()
    {
        if (camFollow)
        {
            Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }


    }
    public Vector3 GetBGSize(GameObject bG)
    {
        Vector2 bGSpriteSize = bG.GetComponent<SpriteRenderer>().sprite.rect.size;
        Vector2 localbGSize = bGSpriteSize / bG.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        Vector3 worldbGSize = localbGSize;
        worldbGSize.x *= bG.transform.lossyScale.x;
        worldbGSize.y *= bG.transform.lossyScale.y;
        return worldbGSize;
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class EyeItem_HJH : BaseItem_LJH
{
    public float zoomOutSpeed = 1;
    public float zoomInSpeed = 1;
    public int eyeTime = 5;
    Vector3 firstCamPos;

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && myItem.isVisited)
        {
            EyeActivate();
        }
        base.OnTriggerEnter2D(other);
    }

    async void EyeActivate()
    {
        CameraManager.Instance.SetCamera(CamValues.Whole);
        Camera.main.cullingMask = -1;
        await UniTask.Delay(1000 * eyeTime, true);
        CameraManager.Instance.SetCamera(CamValues.Character);
    }
    IEnumerator CameraZoomOut(float camSize)
    {
        Camera cam = Camera.main;
        firstCamPos = cam.transform.position;
        while (cam.orthographicSize < camSize || (cam.transform.position - Vector3.zero).magnitude > 0.1f)
        {
            if (cam.orthographicSize < camSize)
            {
                cam.orthographicSize += zoomOutSpeed * Time.unscaledDeltaTime;
            }
            if ((cam.transform.position - Vector3.zero).magnitude > 0.1f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, Vector3.zero, Time.fixedDeltaTime * 0.15f);
            }
            yield return null;
        }
        yield return new WaitForSecondsRealtime(eyeTime);
        StartCoroutine(CameraZoomIn(cam.GetComponent<CamFollowe_HJH>().firstCamSize));
    }
    IEnumerator CameraZoomIn(float camSize)
    {
        Camera cam = Camera.main;
        while (cam.orthographicSize > camSize || (cam.transform.position - firstCamPos).magnitude < 0.1f)
        {
            if (cam.orthographicSize > camSize)
            {
                cam.orthographicSize -= zoomInSpeed * Time.unscaledDeltaTime;
            }
            if ((cam.transform.position - firstCamPos).magnitude > 0.1f)
            {
                cam.transform.position = Vector3.Lerp(cam.transform.position, firstCamPos, Time.fixedDeltaTime * 0.15f);
            }
            yield return null;
        }
        Camera.main.cullingMask = -1;
        Time.timeScale = 1f;
        Camera.main.transform.position = firstCamPos;
        cam.GetComponent<CamFollowe_HJH>().camFollow = true;
        gameObject.SetActive(false);
    }
}

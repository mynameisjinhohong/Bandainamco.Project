using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke_HJH : MonoBehaviour
{
    public GameObject smokeCanvas;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            smokeCanvas.SetActive(true);
        }
    }
}

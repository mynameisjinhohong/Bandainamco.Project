using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBackground_LJH : MonoBehaviour
{
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        ItemManager_LJH.Instance.currBackground = this;
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        ItemManager_LJH.Instance.currBackground = null;
    }

    public virtual void Reset() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using KoreanTyper;
using TMPro;

public abstract class BaseItem_LSW : MonoBehaviour
{
    public int itemNum;
    public ItemManager_LSW itemManager;
    public CharacterMovement2D_LSW character
    {
        get
        {
            return GamePlayManager_HJH.Instance.characterMovement2D;
        }
    }

    public virtual void ItemActivate()
    {
        character.lastUsedItem = itemNum;
        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {  
            itemManager.TriggerCount(itemNum);

            if(character != null)
            {
                character.lastUsedItem = itemNum;
            }
            ItemActivate();
            
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingItem : MonoBehaviour
{
    public void UseItem()
    {
        if (GameManager.movable) return;
        Item i = InventoryManager.Instance.Inventory[transform.GetSiblingIndex()];
        if (i is Consumable)
        {
            InventoryManager.Instance.OnUsingItem(transform.GetSiblingIndex());
        }
        else if(i is Readable readable)
        {
            GameManager.Instance.ReadableImage.GetComponent<ReadableImage>().ReadableImages = readable.ReadableSprite;
            GameManager.Instance.ReadableImage.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingItem : MonoBehaviour
{
    void UseItem()
    {
        if (InventoryManager.Instance.Inventory[transform.GetSiblingIndex()] is Story)
        {
            Debug.Log("¿Ãµø");
        }
        else if(InventoryManager.Instance.Inventory[transform.GetSiblingIndex()] is Readable)
        {

        }
    }
}

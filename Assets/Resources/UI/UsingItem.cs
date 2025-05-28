using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingItem : MonoBehaviour
{
    public void UseItemButton()
    {
        if (GameManager.Instance.movable) return;
        InventoryManager.Instance.OnUsingItem(transform.GetSiblingIndex());

    }
}

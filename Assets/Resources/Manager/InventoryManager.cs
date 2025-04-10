using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static InventoryManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    event Action<Item> GetItemHandler;
    public void OnGetItem(Item item)
    {
        GetItemHandler?.Invoke(item);
    }
    event Action UsingItemHandler;
    public void OnUsingItem()
    {
        UsingItemHandler?.Invoke();
    }
    public Item[] Inventory = new Item[5];
    private void Start()
    {
        GetItemHandler += AddItem;
        GetItemHandler += UpdateInventory;
    }
    public void AddItem(Item item)
    {
        for(int i = 0; i < Inventory.Length; i++)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = item;
                return;
            }
        }
        Debug.Log("cant add item");
    }
    public void UpdateInventory(Item item)
    {
        for(int i = 0; i < Inventory.Length;i++)
        {
            if (Inventory[i] != null)
                GameManager.Instance.inventory.transform.GetChild(i).GetComponent<Image>().sprite = Inventory[i].sprite;
        }
    }
}

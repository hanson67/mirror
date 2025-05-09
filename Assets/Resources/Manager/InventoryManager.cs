using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor.Rendering;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;

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
    public Sprite empty;
    event Action<Item> GetItemHandler;
    public void OnGetItem(Item item)
    {
        GetItemHandler?.Invoke(item);
    }
    event Action<int> UsingItemHandler;
    public void OnUsingItem(int i)
    {
        UsingItemHandler?.Invoke(i);
    }
    public Item[] Inventory = new Item[5];
    private void Start()
    {
        GetItemHandler += addItem;
        GetItemHandler += updateInventory;
        UsingItemHandler += alertItem;
        UsingItemHandler += useItem;
        UsingItemHandler += updateInventory;

    }
    public void addItem(Item item)
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
    public void useItem(int n)
    {
        StoryManager.Instance.itemUsed[Inventory[n].id] = true;
        Inventory[n] = null;
    }
    public void alertItem(int n)
    {
        GameManager.Instance.UsingItemUI.SetActive(false);
        GameManager.Instance.DialogFrame.gameObject.SetActive(true);
        if (Inventory[n] is Consumable con)
            GameManager.Instance.DialogFrame.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{con.alert}";
        GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.AddListener(() => endAlertItem());
        GameManager.movable = false;
    }
    void endAlertItem()
    {
        GameManager.Instance.DialogFrame.gameObject.SetActive(false);
        GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.RemoveAllListeners();
        GameManager.movable = true;
    }
    public void updateInventory(Item item)
    {
        for(int i = 0; i < Inventory.Length;i++)
        {
            if (Inventory[i] != null)
                GameManager.Instance.inventory.transform.GetChild(i).GetComponent<Image>().sprite = Inventory[i].sprite;
        }
    }
    public void updateInventory(int n)
    {
        GameManager.Instance.inventory.transform.GetChild(n).GetComponent<Image>().sprite = empty;
    }
}

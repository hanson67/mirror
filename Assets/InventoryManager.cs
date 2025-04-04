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

    string[] Inventory = new string[24];

    private void Start()
    {
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            for (int i = 0; i < Inventory.Length; i ++) { if (Inventory[i] == null) Inventory[i] = "a"; }
        }
    }
}

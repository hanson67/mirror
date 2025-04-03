using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

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
    public static GameManager Instance
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

    public event Action OnHitted;
    public void Hitted()
    {
        OnHitted?.Invoke();
    }
    public Player Player;
    [Header("=== UI ===")]
    public GameObject health_bar;
    private void Start()
    {
        Player = FindAnyObjectByType<Player>();
    }
    void HealthUpdate()
    {
        if (health_bar.transform.childCount != Player.health)
        {
            
        }
    }
}

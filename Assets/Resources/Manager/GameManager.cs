using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    event Action HittedHandler;
    public void OnHitted()
    {
        HittedHandler?.Invoke();
    }
    public Player Player;
    public static bool movable = true;
    [Header("=== UI ===")]
    public GameObject health_bar;
    public GameObject inventory;
    public GameObject hittedUIparticle;
    public GameObject DialogFrame;
    public GameObject UsingItemUI;
    public GameObject ReadableImage;
    private void Start()
    {
        Player = FindAnyObjectByType<Player>();
        HittedHandler += HealthUpdate;
        HittedHandler += HittedUIparticleRun;
    }
    void HealthUpdate()
    {
        int count = health_bar.transform.childCount - Player.health;
        if (count > 0)
        {
            for (int i = 0; i < count; i++) Destroy(health_bar.transform.GetChild(0).gameObject);
        }
        else if(count < 0)
        {
            for (int i = 0; i > count; i--)
                Instantiate(health_bar.transform.GetChild(0).gameObject, health_bar.transform);
        }
    }
    IEnumerator HittedUIparticle()
    {
        hittedUIparticle.SetActive(true);
        float elasped = 0f;
        while (elasped < 1.5f) 
        {
            elasped += Time.deltaTime;
            Color color = Color.red;
            color.a = (1.5f - elasped)/ 1.5f * 0.6f;
            hittedUIparticle.GetComponent<Image>().color = color;
            yield return null;
        }
        hittedUIparticle.SetActive(false);
    }
    void HittedUIparticleRun()
    {
        StopCoroutine("HittedUIparticle");
        StartCoroutine("HittedUIparticle");
    }
}

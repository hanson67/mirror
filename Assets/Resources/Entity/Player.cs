using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Player : Dummy
{
    public float speed;
    public float hide_time;
    public GameObject bullet;

    bool hide;
    public bool GetHide()
    {
        return hide;
    }
    bool attack_delay;

    Canvas PlayerCanvas;
    SpriteRenderer spriterenderer;
    Transform hide_bar;
    void Start()
    {
        health = 3;
        PlayerCanvas = GetComponentInChildren<Canvas>();
        spriterenderer = GetComponent<SpriteRenderer>();
        hide_bar = PlayerCanvas.gameObject.transform.Find("hide_bar");
    }
    private void OnEnable()
    {
        CinemachineVirtualCamera VC = FindAnyObjectByType<CinemachineVirtualCamera>();
        VC.m_Follow = transform;
        if(LoadingSceneManager.Instance?.currentscene != null)
        {
            Transform obj = GameObject.Find(LoadingSceneManager.Instance.currentscene).transform;
            Vector3 pos = obj.childCount > 0 ? obj.GetChild(0).position : obj.position;
            GameManager.Instance.Player.transform.position = pos;
        }
    }
    void FixedUpdate()
    {
        if (!hide & GameManager.movable)
            Move();
    }
    void Update()
    {
        UseItem();
        if (!GameManager.movable) return;
        Hide();
        Attack();
        Examine();
    }
    void Move()
    {
        float movedir = Input.GetAxisRaw("Horizontal");
        if (movedir < 0) spriterenderer.flipX = true;
        else if (movedir > 0) spriterenderer.flipX = false;
        transform.position += new Vector3(movedir, 0, 0) * speed * Time.deltaTime;
    }
    float _hide_elasped;
    public float hide_elasped 
    {
        get { return _hide_elasped; }
        set { _hide_elasped = value; hide_bar.GetComponent<Image>().fillAmount = value/hide_time; } 
    }
    void Hide()
    {
        if (!hide & Input.GetAxisRaw("Vertical") < 0)
        {
            if(Physics2D.Raycast(new Vector2(transform.position.x-1, transform.position.y), Vector2.right, 2.0f, LayerMask.GetMask("Hiding")) )
            {
                hide_elasped += Time.deltaTime;
                if (hide_elasped > hide_time)
                {
                    spriterenderer.enabled = false;
                    hide_bar.gameObject.SetActive(false);
                    hide = true;
                }
            }
        }
        else if(Input.GetAxisRaw("Vertical") == 0)
        {
            hide_elasped = 0;
        }
        else if(hide & Input.GetAxisRaw("Vertical") > 0)
        {
            hide_elasped = 0;
            spriterenderer.enabled = true;
            hide_bar.gameObject.SetActive(true);
            hide = false;
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, spriterenderer.flipX? Vector2.left : Vector2.right, 8.0f, LayerMask.GetMask("Enemy"));
            if (attack_delay || !hit)
            {
                return;
            }
            Vector2 bulpos = new Vector2(transform.position.x, transform.position.y + 0.65f);
            Vector2 buldir = spriterenderer.flipX ? Vector2.left : Vector2.right;
            float angle = Mathf.Atan2(buldir.x, buldir.y) * Mathf.Rad2Deg;
            StartCoroutine(AttackCool());
            Instantiate(bullet, bulpos, Quaternion.Euler(0, angle + 90, 0));
        }
    }
    IEnumerator AttackCool()
    {
        attack_delay = true;
        yield return new WaitForSeconds(1.0f);
        attack_delay = false;
    }
    void Examine()
    {
        if (Input.GetKeyDown(KeyCode.Z) & GameManager.movable)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 1, transform.position.y), Vector2.right, 2.0f, LayerMask.GetMask("Exam"));
            if (hit)
            {
                Item i = Resources.Load<Item>($"Item/{hit.transform.name}");
                Debug.Log(i.name);
                GameManager.Instance.DialogFrame.gameObject.SetActive(true);
                GameManager.Instance.DialogFrame.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{i.itemname}À» È¹µæÇÏ¿´½À´Ï´Ù.";
                GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.AddListener(() => EndExamineDialog(i));
                GameManager.movable = false;
                hit.transform.gameObject.layer = LayerMask.GetMask("Default");
            }
        }
    }
    void EndExamineDialog(Item item)
    {
        GameManager.Instance.DialogFrame.gameObject.SetActive(false);
        GameManager.Instance.DialogFrame.GetComponent<Button>().onClick.RemoveAllListeners();
        GameManager.movable = true;
        InventoryManager.Instance.OnGetItem(item);
    }
    void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.X) & GameManager.movable)
        {
            GameManager.movable = false;
            GameManager.Instance.UsingItemUI.gameObject.SetActive(true);
            InventoryManager.Instance.OnUsingItem();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.movable = true;
            GameManager.Instance.UsingItemUI.SetActive(false);
            GameManager.Instance.ReadableImage.SetActive(false);
        }
    }
}

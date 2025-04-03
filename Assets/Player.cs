using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : Dummy
{
    public float speed;
    public float hide_time;

    bool hide;
    bool attack_delay;

    Canvas PlayerCanvas;
    SpriteRenderer spriterenderer;
    Transform hide_bar;

    IEnumerator IEAttackCool;
    void Start()
    {
        health = 5;
        PlayerCanvas = GetComponentInChildren<Canvas>();
        spriterenderer = GetComponent<SpriteRenderer>();
        hide_bar = PlayerCanvas.gameObject.transform.Find("hide_bar");
    }
    void FixedUpdate()
    {
        if (!hide)
            Move();
    }
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right, Color.yellow);
        Hide();
        Attack();
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
        if (Input.GetKeyDown(KeyCode.C) & IEAttackCool == null)
        {
            Vector2 raypos = new Vector2 (transform.position.x, transform.position.y+0.25f);
            Vector2 raydir = spriterenderer.flipX ? Vector2.left : Vector2.right;
            RaycastHit2D target = Physics2D.Raycast(raypos, raydir, 1.0f, LayerMask.GetMask("Enemy"));
            if (target & attack_delay == false)
            {
                target.transform.GetComponent<Dummy>().Hitted();
                StartCoroutine(AttackCool());
            }
        }
    }
    IEnumerator AttackCool()
    {
        attack_delay = true;
        yield return new WaitForSeconds(1.0f);
        attack_delay = false;
    }

    // Update is called once per frame

}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Enemy : Dummy
{
    public float speed;
    public Transform attack_particle;

    bool hide;
    bool attack_delay;

    SpriteRenderer spriterenderer;
    void Start()
    {
        health = 1;
        spriterenderer = GetComponent<SpriteRenderer>();

    }
    void FixedUpdate()
    {

    }
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right, Color.yellow);
        Trace();
    }
    void Trace()
    {
        if (attack_delay || GameManager.Instance.Player.GetHide())
        {
            return;
        }
        float movedir = GameManager.Instance.Player.transform.position.x - transform.position.x;
        if (Mathf.Abs(movedir) < 1) { Attack(); }
        if (movedir < 0) { spriterenderer.flipX = true; movedir = -1; }
        else if (movedir > 0) { spriterenderer.flipX = false; movedir = 1; }
        else movedir = 0;
        transform.position += new Vector3(movedir, 0, 0) * speed * Time.deltaTime;
    }
    void Attack()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.65f);
        Vector2 raydir = spriterenderer.flipX ? Vector2.left : Vector2.right;
        float angle = Mathf.Atan2(raydir.x, raydir.y) * Mathf.Rad2Deg;
        GameManager.Instance.Player.transform.GetComponent<Dummy>().Hitted();
        StartCoroutine(AttackCool());
        Instantiate(attack_particle, pos, Quaternion.Euler(0, angle+90,0));
    }
    IEnumerator AttackCool()
    {
        attack_delay = true;
        yield return new WaitForSeconds(1.0f);
        attack_delay = false;
    }

    // Update is called once per frame

}

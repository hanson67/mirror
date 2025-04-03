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
        float movedir = Input.GetAxisRaw("Horizontal");
        if (movedir < 0) spriterenderer.flipX = true;
        else if (movedir > 0) spriterenderer.flipX = false;
        transform.position += new Vector3(movedir, 0, 0) * speed * Time.deltaTime;
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (attack_delay)
            {
                return;
            }
            Vector2 raypos = new Vector2 (transform.position.x, transform.position.y+0.25f);
            Vector2 raydir = spriterenderer.flipX ? Vector2.left : Vector2.right;
            float angle = Mathf.Atan2(raydir.x, raydir.y) * Mathf.Rad2Deg;
            RaycastHit2D target = Physics2D.Raycast(raypos, raydir, 1.0f, LayerMask.GetMask("Enemy"));
            if (target)
            {
                target.transform.GetComponent<Dummy>().Hitted();
            }
            StartCoroutine(AttackCool());
            Instantiate(attack_particle, transform.position, Quaternion.Euler(0, angle+90,0));
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

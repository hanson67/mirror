using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float projectile_speed;
    public float buldistance;

    float initiatedx;

    private void Start()
    {
        initiatedx = transform.position.x;
    }
    void Update()
    {
        if (Mathf.Abs(initiatedx - transform.position.x) > buldistance)
        {
            Destroy(gameObject);
        }
        transform.position +=  -transform.right * projectile_speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.health--;
            Destroy(gameObject);
        }
    }
}

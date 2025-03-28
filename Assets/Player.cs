using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    bool hide;


    void Move()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * Time.deltaTime;
    }
    float hide_elasped;
    void Hide()
    {
        if (!hide & Input.GetAxisRaw("Vertical") < 0)
        {
            hide_elasped += Time.deltaTime;
            if (hide_elasped > 1)
            {
                Debug.Log(0);
                transform.GetComponent<SpriteRenderer>().enabled = false;
                hide = true;
            }

        }
        else if(Input.GetAxisRaw("Vertical") == 0)
        {
            hide_elasped = 0;
        }
        else
        {
            hide_elasped = 0;
            transform.GetComponent<SpriteRenderer>().enabled = true;
            hide = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!hide)
        Move();
    }
    void Update()
    {
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if (collision.gameObject.tag == "hiding") Hide();
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public int health = 5;

    bool hide;
    Canvas PlayerCanvas;
    Transform hide_bar;
    void Start()
    {
        PlayerCanvas = GetComponentInChildren<Canvas>();
        hide_bar = PlayerCanvas.gameObject.transform.Find("hide_bar");
    }
    void Move()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * Time.deltaTime;
    }
    float _hide_elasped;
    public float hide_elasped 
    {
        get { return _hide_elasped; }
        set { _hide_elasped = value; hide_bar.GetComponent<Image>().fillAmount = value; } 
    }
    void Hide()
    {
        if (!hide & Input.GetAxisRaw("Vertical") < 0)
        {
            if(Physics2D.Raycast(new Vector2(transform.position.x-1, transform.position.y), Vector2.right, 2.0f, LayerMask.GetMask("Hiding")) )
            {
                hide_elasped += Time.deltaTime;
                if (hide_elasped > 1)
                {
                    transform.GetComponent<SpriteRenderer>().enabled = false;
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
            transform.GetComponent<SpriteRenderer>().enabled = true;
            hide_bar.gameObject.SetActive(true);
            hide = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!hide)
        Move();
    }
    void Update()
    {
        Debug.DrawRay(transform.position, Vector2.right, new Color(0,1,0));
        Hide();
    }
}

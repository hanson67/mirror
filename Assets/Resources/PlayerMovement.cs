using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 3f;
    private Rigidbody2D rigid;
    private Vector2 moveVec;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVec = new Vector2(moveHorizontal, 0);
    }

    void FixedUpdate()
    {
        rigid.velocity = moveVec * moveSpeed;
    }
}

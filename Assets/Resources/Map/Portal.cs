using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool press;
    private void Update()
    {
        PortalEnter();

    }
    void PortalEnter()
    {
        if (!GameManager.movable) return;
        if (Input.GetKeyDown(KeyCode.C) || !press)
        {
            Vector2 pos = new Vector2(transform.position.x - 0.5f, transform.position.y+0.65f);
            if (Physics2D.Raycast(pos, Vector2.right, 1, LayerMask.GetMask("Player")))
            {
                LoadingSceneManager.Instance.LoadScene(gameObject.name);
            }
            
        }
    }
}

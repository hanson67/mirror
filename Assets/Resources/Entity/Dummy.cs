using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    int _health = 3;
    public int health
    {
        get {  return _health; }
        set {
            if (_health >= 1)
            {
                _health = value;
            }
            if (_health == 0)
            {
                if (gameObject.tag != "Player")
                    StartCoroutine(Die());
                else Debug.Log("stop");
            }
            }
    }
    public void ReduceHealth()
    {
        health--;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Die()
    {
        Vector3 direction = (transform.position.x - GameManager.Instance.Player.transform.position.x) * Vector2.right;
        direction.Normalize();

        float elapsed = 0f;

        while (elapsed < 3)
        {
            transform.position += direction * 10 * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}

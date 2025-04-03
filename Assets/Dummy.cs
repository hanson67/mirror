using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    int _health = 5;
    public int health
    {
        get {  return _health; }
        set {
            if (_health > 1) _health = value;
            else { Die(); }
            }
    }
    public void Hitted()
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
    void Die()
    {
        Destroy(gameObject);
    }
}

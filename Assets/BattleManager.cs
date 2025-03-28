using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitType
{
    Player, Enemy
} 
public class BattleManager : MonoBehaviour
{
    public event Action<HitType> Hitted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

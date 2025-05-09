using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(menuName = "Item/Item")]
public class Item : ScriptableObject 
{
    public int id;
    public string itemname;
    public Sprite sprite;
}

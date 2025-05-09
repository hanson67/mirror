using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public Dictionary<int, bool> dialogueRed = new Dictionary<int, bool>();
    public Dictionary<int, bool> itemUsed = new Dictionary<int, bool>();
    //Update is called once per frame
    void Update()
    {
        
    }
}

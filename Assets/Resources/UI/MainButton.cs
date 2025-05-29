using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButton : MonoBehaviour
{
    public void StartButton()
    {
        LoadingSceneManager.Instance.loadScene("police_office");
        GetComponent<Button>().enabled = false;
    }
}

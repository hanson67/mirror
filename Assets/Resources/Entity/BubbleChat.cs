using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleChat : MonoBehaviour
{
    private int count = 0;
    private bool updatable;
    public GameObject bubblePrefab;
    private GameObject uiInstance;

    private void Update()
    {
        UpdateBubble();
    }
    public void showBubble(string text)
     {
          if (count < 2) // 2번만 실행되고 꺼지게 해달라네요 굳굳 :) 
          {
               GameObject canvas = GameObject.Find("BubbleChatCanvas");
               uiInstance = Instantiate(bubblePrefab, canvas.transform);
               updatable = true;
               uiInstance.GetComponentInChildren<Text>().text = text;
               count++;
          }
     }
    private void UpdateBubble()
    {
        if(updatable)
            uiInstance.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position + new Vector3(0, 3.5f, 0));
    }

     public void removeBubble()
     {
          if (uiInstance != null)
          {
               updatable = false;
               Destroy(uiInstance);
          }
     }
}

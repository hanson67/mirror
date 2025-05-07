using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleChat : MonoBehaviour
{
     private int count = 0;
     public GameObject bubblePrefab;
     private GameObject uiInstance;

     public void showBubble(GameObject canvas,GameObject entity, string text)
     {
          if (count < 2) // 2번만 실행되고 꺼지게 해달라네요 굳굳 :) 
          {
               uiInstance = Instantiate(bubblePrefab, canvas.transform);
               uiInstance.transform.position = gameObject.transform.position + new Vector3(0, 2f, 0);
               bubblePrefab.GetComponentInChildren<Text>().text = text;
               Debug.Log(count);
               count++;
          }
     }

     public void removeBubble(GameObject entity)
     {
          if (uiInstance != null)
          {
               Destroy(uiInstance);
          }
     }
}

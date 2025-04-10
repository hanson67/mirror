using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadableImage : MonoBehaviour
{
    public Sprite[] ReadableImages;
    int CurrentImangeIndex;

    private void OnEnable()
    {
        GetComponent<Image>().sprite = ReadableImages?[0];
        CurrentImangeIndex = 0;
    }
    private void OnDisable()
    {
        ReadableImages = null;
    }
    public void NextImage()
    {
        if ( CurrentImangeIndex < ReadableImages.Length)
        {
            GetComponent<Image>().sprite = ReadableImages[CurrentImangeIndex];
            CurrentImangeIndex ++;
        }
        else
        {
            GameManager.movable = true;
            GameManager.Instance.UsingItemUI.SetActive(false);
            GameManager.Instance.ReadableImage.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float intensity = 0.1f;
    public float frequency = 5f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        float shakeX = Mathf.Sin(Time.time * frequency) * intensity;
        float shakeY = Mathf.Cos(Time.time * frequency) * intensity;
        transform.localPosition = originalPos + new Vector3(shakeX, shakeY, 0);
        print(Time.time * frequency);
    }
}
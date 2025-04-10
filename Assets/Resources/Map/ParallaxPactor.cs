using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float ParallaxFactor = 0.5f;

    private Transform cam;
    private Vector3 previouscampos;

    void Start()
    {
        cam = Camera.main.transform;
        previouscampos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cam.position - previouscampos;
        transform.position += new Vector3(delta.x * ParallaxFactor, 0);
        previouscampos = cam.position;
    }
}
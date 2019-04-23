using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTex : MonoBehaviour
{

    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;
    private float OffsetX;
    private float OffsetY;

    void Update()
    {
        OffsetX = Time.time * ScrollX * Time.deltaTime;
        OffsetY = Time.time * ScrollY * Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
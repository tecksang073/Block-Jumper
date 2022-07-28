using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDisplayBound : MonoBehaviour
{

    float mapX = 100.0f;

    [HideInInspector]
    public float Left;
    [HideInInspector]
    public float Right;



    void Awake()
    {
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float minX = horzExtent - mapX / 2.0f;
        float maxX = mapX / 2.0f - horzExtent;

        Left = maxX - 50;
        Right = minX + 50;
    }

}
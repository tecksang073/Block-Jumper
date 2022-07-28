using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingEffect : MonoBehaviour
{
    public float minTime = 0.8f;
    public float maxTime = 0.8f;

    private float timer;
    private Text textFlicker;

    void Start()
    {
        textFlicker = GetComponent<Text>();
        timer = Random.Range(minTime, maxTime);
    }


    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            textFlicker.enabled = !textFlicker.enabled;
            timer = Random.Range(minTime, maxTime);
        }
    }
}

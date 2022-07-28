using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    public float distance = 3;
    public float velocity = 2;

    float angle = 0;   

    void Update()
    {   
        if(Time.timeScale != 1) return;
        MoveSideToSide();
    }

    void MoveSideToSide()
    {
        transform.position = new Vector2(Mathf.Sin(angle) * distance, transform.position.y);
        angle += velocity / 100;
    }

    public void StartCoroutine_LandingEffect()
    {
        StartCoroutine(LandingEffect());
    }

    IEnumerator LandingEffect()
    {
        Vector2 originalPosition = transform.position;
        float originalPosition_Y = transform.position.y;
        float YChangeValue = 1.5f;

        while (YChangeValue > 0)
        {
            YChangeValue -= 0.1f;
            YChangeValue = Mathf.Clamp(YChangeValue, 0, 1.5f);
            transform.position = new Vector3(transform.position.x, originalPosition_Y - YChangeValue);
            yield return 0;
        }
        yield break;
    }

}

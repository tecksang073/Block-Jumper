using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{

    public GameObject StepPrefab;

    [Space]
    public float stepWidth;
    public float stepHeight;
    [Space]
    public float decreasStepWidth;
    public float minimumStepWidth;

    public int NumberOfStartSteps = 4;
    public int DistanceToNextStep = 6;

    int stepIndex = 0;

    float halfWidth;

    float LeftEnd;
    float RightEnd;

    private void Awake()
    {
        LeftEnd = GameObject.Find("GameManager").GetComponent<GetDisplayBound>().Left;
        RightEnd = GameObject.Find("GameManager").GetComponent<GetDisplayBound>().Right;
    }


    void Start()
    {
        halfWidth = GameObject.Find("GameManager").GetComponent<GetDisplayBound>().Right;
        InitSteps();
    }

    void InitSteps()
    {
        for (int i = 0; i < NumberOfStartSteps; i++)
        {
            MakeStep();
        }
    }

    public void MakeStep()
    {
        int randomPosx;
        if (stepIndex == 0)
            randomPosx = 0;
        else
            randomPosx = Random.Range((int)LeftEnd, (int)RightEnd);

        Vector2 pos = new Vector2(randomPosx, stepIndex * DistanceToNextStep);
        GameObject stepObj = Instantiate(StepPrefab, pos, Quaternion.identity);
        stepObj.transform.SetParent(transform);
        stepObj.transform.localScale = new Vector2(stepWidth, stepHeight);

        DecreaseStepWidth();
        SetSpeed(stepObj);
        SetDirection(stepObj);
        IncreaseStepIndex();

    }

    void SetSpeed(GameObject newStepObj)
    {
        newStepObj.GetComponent<Step>().distance = Random.Range(1, halfWidth);
        if (stepIndex == 0)
            newStepObj.GetComponent<Step>().velocity = 0;
        else
            newStepObj.GetComponent<Step>().velocity = Random.Range(3, 5);
    }

    void SetDirection(GameObject newStepObj)
    {
        if (Random.Range(0, 2) == 0)
        {
            newStepObj.GetComponent<Step>().velocity *= -1;
        }
    }

    void IncreaseStepIndex()
    {
        stepIndex++;
    }

    void DecreaseStepWidth()
    {
        if (stepWidth > minimumStepWidth)
        {
            stepWidth -= decreasStepWidth;
        }
    }

}

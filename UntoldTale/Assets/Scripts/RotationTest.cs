using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    public int length;
    public float angleVal;
    public float xSpace, ySpace; // Space ySpaceetween the spirals
    LineRenderer lineRenderer;
    Vector3[] poses;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = length;
        poses = new Vector3[length];
    }

    void FixedUpdate()
    {
        SpiralCalculator();
    }
    void SpiralCalculator()
    {
        float x = 0;
        float y = 0;

        for (int i = 0; i < length; i++)
        {
            var angle = angleVal * i;
            x = (xSpace + ySpace * angle) * Mathf.Cos(angle);
            y = (xSpace + ySpace * angle) * Mathf.Sin(angle);
            poses[i] = new Vector3(x,y,0f);
        }
        lineRenderer.SetPositions(poses);
    }
}

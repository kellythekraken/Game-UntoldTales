using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    public int length;
    public LineRenderer lineRenderer;
    public Vector3[] segmentPoses,segmentVelocity;
    public Transform targetDir;
    public Transform wiggleDir;
    public float targetDistance;
    public float smoothSpeed;

    public float wiggleSpeed,wiggleMagnitude;

    private void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocity = new Vector3[length];
        ResetPosition();
    }
    void FixedUpdate()
    {
        if(!HeadMovement.move) return;
        wiggleDir.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;
        for(int i = 1; i < length ; i++)
        {
            //Vector3 wiggle = new Vector3(0,0, wiggleDir.localRotation.z);
            Vector3 targetPos = segmentPoses[i-1] + (segmentPoses[i] - segmentPoses[i-1]).normalized * targetDistance;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],targetPos,ref segmentVelocity[i], smoothSpeed);
            //segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],segmentPoses[i-1] + targetDir.right * targetDistance, ref segmentVelocity[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }

    void ResetPosition()
    {
        segmentPoses[0] = targetDir.position;
        for(int i = 1;i<length; i++)
        {
            segmentPoses[i] = segmentPoses[i-1] + targetDir.right * targetDistance;
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}

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
    public float smoothSpeed, curlSpeed;
    HeadMovement wormi;

    public float wiggleSpeed,wiggleMagnitude;

    private void Start()
    {
        wormi = HeadMovement.Instance;
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocity = new Vector3[length];
        ResetCurl();
        //ResetPosition();
    }
    void FixedUpdate()
    {
        if(!wormi.move) return;
        //wiggleDir.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);
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

    void ResetCurl()
    {
        segmentPoses[0] = targetDir.position;

        float x = 0;
        float y = 0;
        for (int i = 1; i < length; i++)
        {
            var angle = angleVal * i;
            x = (xSpace + ySpace * angle) * Mathf.Cos(angle);
            y = (xSpace + ySpace * angle) * Mathf.Sin(angle);
            segmentPoses[i] = new Vector3(x,y,0f) + segmentPoses[i-1];
        }
        lineRenderer.SetPositions(segmentPoses);
    }
    public float angleVal;
    public float xSpace, ySpace; // Space ySpaceetween the spirals
    bool curlStarted; //avoid repeated coroutine call
    public void CurlUp()
    {
        if(!curlInCoolDown && !curlStarted) 
        {
            curlStarted = true;
            StartCoroutine(SmoothCurl());
        }
   //     Vector3 targetPos = segmentPoses[i-1] + (segmentPoses[i] - segmentPoses[i-1]).normalized * targetDistance;
    }
    bool curlInCoolDown = false;
    IEnumerator SmoothCurl()
    {
        GameManager.Instance.DisableInput();
        segmentPoses[0] = targetDir.position;

        float x = 0;
        float y = 0;
        Vector3 velocity = Vector3.zero;
        float time = 0;
        float duration = 2f;
        while(time < duration)//animate the curl
        {
            for (int i = 1; i < length; i++)    //creating the curl bit by bit
            {
                var angle = angleVal * i;
                x = (xSpace + ySpace * angle) * Mathf.Cos(angle);
                y = (xSpace + ySpace * angle) * Mathf.Sin(angle);
                Vector3 targetPos = new Vector3(x,y,0f) + segmentPoses[i-1];
                segmentPoses[i] = Vector3.Lerp(segmentPoses[i],targetPos, time / duration);

            }
            lineRenderer.SetPositions(segmentPoses);
            time += Time.deltaTime;
            yield return null;
        }
        GameManager.Instance.EnableInput();
        StartCoroutine(CurlCooldown());
        curlStarted = false;
    }

    IEnumerator CurlCooldown()
    {
        curlInCoolDown = true;
        yield return new WaitForSeconds(3f);
        curlInCoolDown = false;
    }
    /*    IEnumerator SmoothCurl()
    {
        Debug.Log("start curling");
        isCurling = true;
        //segmentPoses[0] = targetDir.position;

        float x = 0;
        float y = 0;
        Vector3 velocity = Vector3.zero;
        float time = 0;
        float duration = 5f;
        while(time < duration)//animate the curl
        {
            for (int i = 1; i < length; i++)    //animating each point of linerender
            {
                var angle = angleVal * i;
                x = (xSpace + ySpace * angle) * Mathf.Cos(angle);
                y = (xSpace + ySpace * angle) * Mathf.Sin(angle);
                Vector3 targetPos = new Vector3(x,y,0f) + segmentPoses[i-1];
                lineRenderer.SetPosition(0,targetPos);
                yield return null;

                //segmentPoses[i] = targetPos;
                // segmentPoses[i] = Vector3.Lerp(segmentPoses[i],targetPos, time / duration);
                //segmentPoses[0] = Vector3.Lerp(segmentPoses[0],targetPos,time / duration);
            }
            time += Time.deltaTime;
        }
    }
    */
}

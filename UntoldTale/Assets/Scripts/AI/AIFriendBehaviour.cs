using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY ENABLED AFTER BEFRIENDED
public enum AISTATE { FOLLOW, INVITE, RETREAT, REST}   
public class AIFriendBehaviour : MonoBehaviour
{
    //follow : hard follow ; invite: stay still and make space for player when player come close ; retreat: back to default pos
    public AISTATE myState {get{return _mystate;} set { if(value !=myState) _mystate=value;}}
    AISTATE _mystate;
    public bool follow = false;
    Transform wormi;
    Rigidbody2D centerRb;
    float calculatedRadius;
    public float followSpeed = .3f;
    Vector3 startPosition;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius + 3.5f;
        startPosition = transform.position;
        _mystate = AISTATE.REST;
    }

    void FixedUpdate()
    {
        SetAIBehaviourByState();
    }
    public void SetAIBehaviourByState()
    {
        switch(_mystate)
        {
            case AISTATE.FOLLOW:
                if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
                {
                    Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed * Time.deltaTime);
                    centerRb.MovePosition(newPosition);
                }
            return;
            case AISTATE.INVITE:
            return;
            case AISTATE.RETREAT:
                if(Vector2.Distance(centerRb.position,startPosition) > 1f)
                {
                    var lerpVal = Vector2.Lerp(centerRb.position, startPosition, 0.02f);
                    centerRb.MovePosition(lerpVal);
                }
                else{ myState = AISTATE.REST;}
                return;
            case AISTATE.REST:
            return;
        }
    }
    public void MoveBackToDefaultPosition()
    {
        StartCoroutine(ResetPos());
    }
    IEnumerator ResetPos()
    {
        float elapsed = 0f;
        var startpos = centerRb.position;
        while(elapsed< 2f)
        {
            var lerpVal = Vector2.Lerp(startpos, startPosition, elapsed/2f);
            centerRb.MovePosition(lerpVal);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FollowBehaviour()
    {
        var timeElapsed = 0f;
        while(timeElapsed < 5f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, 0.01f);
                centerRb.MovePosition(newPosition);
            }
            else {yield break;}

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    void FloatBehaviour()
    {
        //randomly move around, like a boil animation
    }

}

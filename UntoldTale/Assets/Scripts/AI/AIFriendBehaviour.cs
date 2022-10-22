using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY ENABLED AFTER BEFRIENDED
public enum AISTATE { FOLLOW, RETREAT }   
public class AIFriendBehaviour : BaseAI
{
    //follow : hard follow ; invite: stay still and make space for player when player come close ; retreat: back to default pos
    public AISTATE myState {get{return _mystate;} set {  rest = false; if(value !=myState) _mystate=value;}}
    AISTATE _mystate;
    bool rest;

    protected override void Start()
    {
        base.Start();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius + 3.5f;
        rest = true;
    }

    void FixedUpdate()
    {
        if(rest) return;
        SetAIBehaviourByState();
    }
    public void SetAIBehaviourByState()
    {
        switch(_mystate)
        {
            case AISTATE.FOLLOW:
                if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
                {
                    Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, 0.01f);
                    centerRb.MovePosition(newPosition);
                }
                else
                {
                    Vector2 direction = transform.position - wormi.transform.position;
                    direction.Normalize();
                    centerRb.AddForce(direction * 30f * centerRb.mass);
                }
                return;
            case AISTATE.RETREAT:
                if(Vector2.Distance(centerRb.position,startPosition) > 1f)
                {
                    var lerpVal = Vector2.Lerp(centerRb.position, startPosition, 0.01f);
                    centerRb.MovePosition(lerpVal);
                }
                else{ rest = true;}
                return;
        }
    }
    public void MoveBackToDefaultPosition()
    {
        StopCoroutine(FollowBehaviour());
        StartCoroutine(ResetPos());
    }
    
    IEnumerator InviteBehaviour()
    {
        float elapsed = 0;
        while(elapsed<100f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FollowBehaviour()
    {
        var timeElapsed = 0f;
        while(timeElapsed < 100f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, 0.01f);
                centerRb.MovePosition(newPosition);
            }
            else
            {
                Vector2 direction = transform.position - wormi.transform.position;
                direction.Normalize();
                centerRb.AddForce(direction *5f * centerRb.mass);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

}

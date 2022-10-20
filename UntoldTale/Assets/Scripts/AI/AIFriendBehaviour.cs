using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY ENABLED AFTER BEFRIENDED
public enum AISTATE { FOLLOW, INVITE, RETREAT, REST}   
public class AIFriendBehaviour : BaseAI
{
    //follow : hard follow ; invite: stay still and make space for player when player come close ; retreat: back to default pos
    public AISTATE myState {get{return _mystate;} set { if(value !=myState) _mystate=value; SetAIBehaviourByState();}}
    AISTATE _mystate;

    protected override void Start()
    {
        base.Start();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius + 3.5f;
        _mystate = AISTATE.REST;
    }

    void FixedUpdate()
    {
        //SetAIBehaviourByState();
    }
    public void SetAIBehaviourByState()
    {
        switch(_mystate)
        {
            case AISTATE.FOLLOW:
                //maybe go back to original position after two seconds?
                StartCoroutine(FollowBehaviour());
            return;
                if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
                {
                    Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed * Time.deltaTime);
                    centerRb.MovePosition(newPosition);
                }
            case AISTATE.INVITE:
            return;
            case AISTATE.RETREAT:
                ResetPos();
                return;
            case AISTATE.REST:
                return;
        }
    }
    public void MoveBackToDefaultPosition()
    {
        StartCoroutine(ResetPos());
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
        ResetPos(1f);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFriendBehaviour : MonoBehaviour
{
    //behaviours:
    // do next: sound touchy effect > get sound after befriended
/* 
    //stop at level 1!!

    complete the AI behaviour: 
        - friend come greet you when you're leaving, and stuff the exit after you're gone
        - friend turn a bit blue when you're leaving
*/
    public enum AIBehaviour{FORCE, FOLLOW, FLOAT, NOTHING};
    public bool animateOnGameStart = false;
    private AIBehaviour _behaviour;
    internal AIBehaviour Behaviour {get {return _behaviour;} set {_behaviour = value; ChangeAIBehaviour(value);} }
    [SerializeField] AIBehaviour startBehaviour;
    Transform wormi;
    Rigidbody2D centerRb;
    float calculatedRadius;
    [SerializeField] float followSpeed;
    [SerializeField] float defaultForce = 500f;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        GameManager.Instance.GameStartEvent.AddListener(GameStarInit);
        StartCoroutine(FamilyOnGameStartBehaviour());
    }

    IEnumerator FamilyOnGameStartBehaviour()
    {
        //hug
        var timeElapsed = 0f;
        while(timeElapsed < 3f)
        {

            Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, timeElapsed/3f);
            centerRb.MovePosition(newPosition * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //move away
        timeElapsed = 0f;
        Debug.Log("start moving away");
        Vector2 direction = transform.position - wormi.transform.position;
        direction.Normalize();
        centerRb.transform.right = direction;
        //centerRb.AddForce(centerRb.transform.right * force,ForceMode2D.Impulse);
        while(timeElapsed < 2f)
        {
            centerRb.AddForce(centerRb.transform.right *5f * centerRb.mass);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("stop moving away");

        /*behaviour:
            - slowly march towards wormi and hug tight
            - after a few seconds, move away slowly and leave her a trail
            - now you can move, the path to friends and tunnel 
            - pathfinding? follow a path
          */  
    }

    void GameStarInit()
    {
        ChangeAIBehaviour(startBehaviour); 
    }

    void ChangeAIBehaviour(AIBehaviour newBehaviour)
    {
        switch(newBehaviour)
        {
            case AIBehaviour.FORCE:
                ForceBehaviour(defaultForce);
                return;
            case AIBehaviour.FOLLOW:
                StartCoroutine(FollowBehaviour());
                return;
            case AIBehaviour.FLOAT:
                FloatBehaviour();
                return;
            case AIBehaviour.NOTHING:
                return;
        }
    }

    /*void FixedUpdate()
    {
            //transform.position = Vector3.Lerp(transform.position,centerRb.position, followSpeed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position,wormi.position, followSpeed * Time.deltaTime);
    }*/

    void ForceBehaviour(float force)
    {
        centerRb.transform.right = (Vector2)wormi.position - centerRb.position;
        centerRb.AddForce(centerRb.transform.right * force,ForceMode2D.Impulse);
    }
    IEnumerator FollowBehaviour(bool persistent = false)
    {
        var timeElapsed = 0f;
        while(timeElapsed < 5f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed * Time.deltaTime);
                centerRb.MovePosition(newPosition);
            }
            else if(!persistent) {yield break;}

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    void FloatBehaviour()
    {
        //randomly move around, like a boil animation
    }

}

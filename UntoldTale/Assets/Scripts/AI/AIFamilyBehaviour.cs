using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
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
    }

    IEnumerator FamilyOnGameStartBehaviour()
    {
        StartCoroutine(MoveAway());
//        StartCoroutine(FollowBehaviour());
        yield return new WaitForSeconds(2);

        /*behaviour:
            - slowly march towards wormi and hug tight
            - after a few seconds, move away slowly and leave her a trail
            - now you can move, the path to friends and tunnel 
            - pathfinding? follow a path
          */  
    }

    void GameStarInit()
    {
        if(animateOnGameStart) StartCoroutine(FamilyOnGameStartBehaviour());
        else {ChangeAIBehaviour(startBehaviour); }
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

    /*    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            follow = false;
        }
    } */
    IEnumerator MoveAway()
    {
        var timeElapsed = 0f;
        Vector2 direction = transform.position - wormi.transform.position;
        direction.Normalize();
        centerRb.transform.right = direction;
        //centerRb.AddForce(centerRb.transform.right * force,ForceMode2D.Impulse);
        Debug.Log("start moving away");
        while(timeElapsed < 5f)
        {
            centerRb.AddForce(centerRb.transform.right *50f * Time.deltaTime,ForceMode2D.Force);

            //centerRb.MovePosition(direction);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Debug.Log("stop moving away");
    }
}

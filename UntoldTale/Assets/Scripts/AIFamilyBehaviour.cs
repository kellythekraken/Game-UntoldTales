using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    public enum AIBehaviour{FORCE, FOLLOW, FLOAT, NOTHING};
    private AIBehaviour _behaviour;
    internal AIBehaviour Behaviour {get {return _behaviour;} set {_behaviour = value; ChangeAIBehaviour(value);} }
    [SerializeField] AIBehaviour startBehaviour;
    bool follow = true; //follow on startup, to create that start animation
    public float speed;
    Transform wormi;
    Rigidbody2D rb;
    float calculatedRadius;
    [SerializeField] float startForce;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        rb = GetComponentInChildren<Rigidbody2D>();

        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        follow = false;
        ForceBehaviour(startForce);
    }
    void ChangeAIBehaviour(AIBehaviour newBehaviour)
    {
        switch(newBehaviour)
        {
            case AIBehaviour.FORCE:
                ForceBehaviour(startForce);
                return;
            case AIBehaviour.FOLLOW:
                 FollowBehaviour();
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
            //transform.position = Vector3.Lerp(transform.position,rb.position, speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position,wormi.position, speed * Time.deltaTime);
    }*/

    void ForceBehaviour(float force)
    {
        rb.transform.right = (Vector2)wormi.position - rb.position;
        rb.AddForce(rb.transform.right * force,ForceMode2D.Impulse);
    }
    IEnumerator FollowBehaviour()
    {
        var timeElapsed = 0f;
        while(timeElapsed<5f)
        {
            timeElapsed += Time.deltaTime;
            if(Vector2.Distance(rb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(rb.position,wormi.position, speed * Time.deltaTime);
                rb.MovePosition(newPosition);
            }
            else {yield break;}

            yield return null;
        }
    }
    void FloatBehaviour()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            follow = false;
        }
    }
}

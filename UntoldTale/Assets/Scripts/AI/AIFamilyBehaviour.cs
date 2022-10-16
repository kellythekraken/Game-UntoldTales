using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    public enum AIBehaviour{FORCE, FOLLOW, FLOAT, NOTHING};
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
    IEnumerator FollowBehaviour()
    {
        var timeElapsed = 0f;
        while(timeElapsed < 5f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed * Time.deltaTime);
                centerRb.MovePosition(newPosition);
            }
            else{ yield break;}

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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFriendBehaviour : MonoBehaviour
{
/* 
    complete the AI behaviour: 
        - friend come greet you when you're leaving, and stuff the exit after you're gone
        - friend turn a bit blue when you're leaving
        - reset position after you're gone. 
*/
    bool follow;
    public bool animateOnGameStart = false;
    Transform wormi;
    Rigidbody2D centerRb;
    float calculatedRadius;
    [SerializeField] float followSpeed;
    [SerializeField] float defaultForce = 500f;
    Vector3 startPosition;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if(follow)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed * Time.deltaTime);
                centerRb.MovePosition(newPosition);
            }
        }
    }

    void MoveBackToDefaultPosition()
    {
        while(true)
        {
            var dist = Vector3.Distance(centerRb.position, startPosition);
            if(dist>1f)
            {
                var lerpVal = Vector2.Lerp(centerRb.position, startPosition, 0.05f);
            }
            else {break;}
        }
    }

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

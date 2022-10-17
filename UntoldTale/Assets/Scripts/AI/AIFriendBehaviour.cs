using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ONLY ENABLED AFTER BEFRIENDED
public class AIFriendBehaviour : MonoBehaviour
{
/* 
    complete the AI behaviour: 
        - friend come greet you when you're leaving, and stuff the exit after you're gone
        - friend turn a bit blue when you're leaving
*/
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
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +1.5f;
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

    public void MoveBackToDefaultPosition()
    {
        StartCoroutine(ResetPos());
    }
    IEnumerator ResetPos()
    {
        follow = false;
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

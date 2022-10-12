using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    bool follow = true; //follow on startup, to create that start animation
    public float speed;
    Transform wormi;
    Rigidbody2D rb;
    float calculatedRadius;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        rb = GetComponentInChildren<Rigidbody2D>();

        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
    }

    void FixedUpdate()
    {
        if(follow)
        {
            if(Vector2.Distance(rb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(rb.position,wormi.position, speed * Time.deltaTime);
                rb.MovePosition(newPosition);
            }
            else{ follow = false;}
            
            //transform.position = Vector3.Lerp(transform.position,rb.position, speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position,wormi.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) 
        {
            follow = false;
        }
    }
}

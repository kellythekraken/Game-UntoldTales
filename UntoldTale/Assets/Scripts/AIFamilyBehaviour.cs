using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    public bool follow = true; //follow on startup, to create that start animation
    public float speed;
    Transform wormi;
    Rigidbody2D rb;
    Transform center;
    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        rb = GetComponentInChildren<Rigidbody2D>();
        center = rb.transform;
    }

    void FixedUpdate()
    {
        if(follow)
        {
            //Vector2 newPosition = Vector2.Lerp(center.position,wormi.position, speed * Time.deltaTime);
            //rb.MovePosition(newPosition);

            transform.position = Vector3.MoveTowards(transform.position,wormi.position, speed * Time.deltaTime);
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

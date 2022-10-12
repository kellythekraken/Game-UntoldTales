using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    bool follow = true; //follow on startup, to create that start animation
    public float speed, minimumDistance;
    public Transform wormi;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(follow)
        {
            if(Vector2.Distance(transform.position,wormi.position) > minimumDistance)
            {
                Vector2 newPosition = Vector2.MoveTowards(transform.position,wormi.position, speed * Time.deltaTime);
                rb.MovePosition(newPosition);

                //rb.AddRelativeForce(Vector3.forward * speed, ForceMode2D.Force);
                //transform.position = Vector3.MoveTowards(transform.position,wormi.position, speed * Time.deltaTime);
            }
        }

    }
}

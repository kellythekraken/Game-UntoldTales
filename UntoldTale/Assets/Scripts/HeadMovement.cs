using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HeadMovement : MonoBehaviour
{
    Camera mainCam;
    public float rotSpeed;
    public float moveSpeed;
    private Vector2 direction;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //MoveToCursor();
        MoveFromInput();
    }

    void MoveFromInput()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    void OnMove(InputValue value)
    {
        Debug.Log("detect move input");
        moveInput = value.Get<Vector2>();
    }

    /*
    void RotateToCursor()
    {
        direction = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation,rotation,rotSpeed * Time.deltaTime);
    }
    
    void MoveToCursor()
    {
        Vector2 cursorPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = Vector2.MoveTowards(transform.position,cursorPos,moveSpeed * Time.deltaTime);
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HeadMovement : MonoBehaviour
{
    public static HeadMovement Instance;
    Camera mainCam;
    public float rotSpeed;
    public float moveSpeed;
    private Vector2 direction;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public static bool move = true;
    TailMovement tailScript;
    InputActionAsset input;

    void Awake() => Instance = this;
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        tailScript = GetComponentInChildren<TailMovement>();
        //input = GetComponent<PlayerInput>().
    }

    void FixedUpdate()
    {
        if(!move) return;
        //MoveToCursor();
        MoveFromInput();
    }

    void MoveFromInput()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
        tailScript.isCurling = false;
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if(moveInput == Vector2.zero) move = false;
        else{move = true;}
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

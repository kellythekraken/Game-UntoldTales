using System.Collections;
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
    public bool move = true;
    TailMovement tailScript;

    void Awake() => Instance = this;
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        tailScript = GetComponentInChildren<TailMovement>();
    }

    void FixedUpdate()
    {
        if(!move) return;
        MoveFromInput();
    }

    void MoveFromInput()
    {
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if(moveInput == Vector2.zero) move = false;
        else{move = true;}
    }

}

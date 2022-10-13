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
    InputActionMap playerInput;

    void Awake() => Instance = this;
    void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        tailScript = GetComponentInChildren<TailMovement>();
        playerInput = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        FreezeInput(1f);
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
    public void FreezeInput(float freezeTime = 1f)
    {
        StartCoroutine(FreezeInputTimer(freezeTime));
    }
    IEnumerator FreezeInputTimer(float freezeTime)
    {
        playerInput.Disable();
        yield return new WaitForSeconds(freezeTime);
        playerInput.Enable();
    }
    public void EnableInput() => playerInput.Enable();
    public void DisableInput() => playerInput.Disable();
}

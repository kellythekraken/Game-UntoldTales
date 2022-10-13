using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent GameStartEvent;
    [SerializeField] GameObject TitleCanvasUI;
    [SerializeField] Button startBtn;
    [SerializeField] PlayerInput playerInput;
    InputAction restartAction;

    void Awake()
    {
        if(GameStartEvent == null) GameStartEvent = new UnityEvent();
        Instance = this;
    }
    void Start()
    {
        startBtn.onClick.AddListener(()=>ShowTitleScreen());
        InputActionMap map = playerInput.actions.FindActionMap("Player");
        restartAction = map["Restart"];
        restartAction.performed += ReloadGame;
    }
    void OnEnable()
    {
        ShowTitleScreen(true);
    }
    void OnDisable() => restartAction.performed -= ReloadGame;
    void ShowTitleScreen(bool show = false)
    {
        if(show){
            PauseGame();
            TitleCanvasUI.SetActive(true);
        }
        else {
            ResumeGame();
            TitleCanvasUI.SetActive(false);
            GameStartEvent.Invoke();
        }
    }
    void ReloadGame(InputAction.CallbackContext ctx)
    {
       SceneManager.LoadScene(0);
    }
    void PauseGame ()
    {
        Time.timeScale = 0;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
    }
}

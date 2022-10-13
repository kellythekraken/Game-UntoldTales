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
    [SerializeField] PlayerInput playerInput;
    InputActionMap actionMap; 
    InputAction restartAction, startGameAction;
    bool gameStarted = false;
    void Awake()
    {
        if(GameStartEvent == null) GameStartEvent = new UnityEvent();
        Instance = this;
    }
    void Start()
    {
        actionMap = playerInput.actions.FindActionMap("Player");
        restartAction = actionMap["Restart"];
        startGameAction = actionMap["StartGame"];
        restartAction.performed += ReloadGame;
        startGameAction.performed += HideTitleScreen;
        actionMap.Enable();
        ShowTitleScreen(true);
    }
    void OnDisable()
    {
        restartAction.performed -= ReloadGame; 
        actionMap.Disable();
    }
    void ShowTitleScreen(bool show = false)
    {
        Debug.Log("show title screen " + show); 
        if(show){   //start screen
            PauseGame();
            TitleCanvasUI.SetActive(true);
            restartAction.Disable();
        }
        else {  //play
            ResumeGame();
            TitleCanvasUI.SetActive(false);
            startGameAction.performed -= HideTitleScreen;
            startGameAction.Disable();
            restartAction.Enable();
            GameStartEvent.Invoke();
            FreezeInput(2f);    //freeze player input on start up
        }
    }
    void HideTitleScreen(InputAction.CallbackContext ctx)
    {
        ShowTitleScreen(false);
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

#region PlayerInputControls
    public void FreezeInput(float freezeTime = 1f)
    {
        StartCoroutine(FreezeInputTimer(freezeTime));
    }
    IEnumerator FreezeInputTimer(float freezeTime)
    {
        actionMap.Disable();
        yield return new WaitForSeconds(freezeTime);
        actionMap.Enable();
    }
    public void EnableInput() => actionMap.Enable();
    public void DisableInput() => actionMap.Disable();
#endregion
}

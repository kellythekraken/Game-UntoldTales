using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

//controls scene loading and restart
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    internal UnityEvent GameStartEvent;
    [SerializeField] GameObject TitleCanvasUI;
    [SerializeField] PlayerInput playerInput;
    InputActionMap actionMap; 
    InputAction restartAction, startGameAction;
    bool gameStarted = false;
    CanvasGroup startCanvas;
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
        startCanvas = TitleCanvasUI.GetComponent<CanvasGroup>();
        ShowTitleScreen(true);
    }
    void OnDisable()
    {
        restartAction.performed -= ReloadGame; 
        actionMap.Disable();
    }

#region scene managing
    void ShowTitleScreen(bool show = false)
    {
        if(show){   //start screen
            PauseGame();
            StartCoroutine(FadeInScreen(startCanvas));
            restartAction.Disable();
        }
        else {  //play
            Debug.Log("fade out start");
            StartCoroutine(FadeOutScreen(startCanvas));
            startGameAction.performed -= HideTitleScreen;
            startGameAction.Disable();
            restartAction.Enable();
            GameStartEvent.Invoke();
            FreezeInput(.7f);    //freeze player input on start up
            AudioManager.Instance.PlayBGM("Home");
            ResumeGame();
        }
    }

    IEnumerator FadeOutScreen(CanvasGroup canvas,float lerpTime = 1f)  //disappear
    {
        float timeElapsed = 0f;
        while(timeElapsed < .5f)
        {
            canvas.alpha = Mathf.Lerp(1,0,timeElapsed/ 0.5f);
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
        TitleCanvasUI.SetActive(false);
    }
    IEnumerator FadeInScreen(CanvasGroup canvas, float lerpTime = 1f)   //appear
    {
        float timeElapsed = 0f;
        while(timeElapsed < .5f)
        {
            timeElapsed += Time.fixedDeltaTime;
            canvas.alpha = Mathf.Lerp(0,1,timeElapsed / 0.5f);
            yield return null;
        }
        TitleCanvasUI.SetActive(true);
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
    /*
    IEnumerator StartButtonAnimation()
    {
        //lerp button alpha from 0 to 1
        float timeElapsed =0f;
        while(timeElapsed<1f)
        {
            startBtnCanvas.alpha = Mathf.Lerp(0f,1f,timeElapsed);
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
    } */
#endregion

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

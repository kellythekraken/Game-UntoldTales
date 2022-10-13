using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject TitleCanvasUI;
    [SerializeField] Button startBtn;
    void Start()
    {
        startBtn.onClick.AddListener(()=>ShowTitleScreen());
    }
    void OnEnable()
    {
        ShowTitleScreen(true);
    }

    void ShowTitleScreen(bool show = false)
    {
        if(show){
            PauseGame();
            TitleCanvasUI.SetActive(true);
        }
        else {
            ResumeGame();
            TitleCanvasUI.SetActive(false);
        }
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

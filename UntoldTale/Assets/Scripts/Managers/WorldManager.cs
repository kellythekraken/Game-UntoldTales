using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public List<string> currentSceneInLoad;

    void Awake() => Instance = this;

    void Start()
    {
        currentSceneInLoad = new List<string>();
        InitWorld();
    }

    public void InitWorld()
    {
        //LoadScene("Level_1");
    }

    public void ReloadGame()
    {
       SceneManager.LoadScene(0);
       currentSceneInLoad.Clear();
    }

    public void LoadScene(string sceneName)
    {
        var duplicate = currentSceneInLoad.Find(t => t == sceneName);   //check if they are already loaded
        if(duplicate == null)
        {
            SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            currentSceneInLoad.Add(sceneName);
        }
    }
    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}

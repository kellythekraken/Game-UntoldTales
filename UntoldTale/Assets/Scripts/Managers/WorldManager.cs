using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//manages the load and unload of sceneList

[System.Serializable]
public struct LabeledScene
{
    public string sceneName;
    public AsyncOperation scene;

    public LabeledScene(string _name, AsyncOperation op)
    {
        sceneName = _name;
        scene = op;
    }
}
public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;
    public List<string> currentSceneInLoad; //for name search
    public List<LabeledScene> sceneList;//stores all the scenes
    void Awake() => Instance = this;

    void Start()
    {
        sceneList = new List<LabeledScene>();
        sceneList.Clear();
        currentSceneInLoad.Clear();
    }
    public void ReloadGame()
    {
       AudioManager.Instance.StopAllBGM();
       SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    void PrepareScenesToLoad()
    {
        for(int i = 1; i<4;i++)
        {
            var async = SceneManager.LoadSceneAsync(i,LoadSceneMode.Additive);
            async.allowSceneActivation = false;
            string name = SceneManager.GetSceneByBuildIndex(i).name;
            var scene = new LabeledScene(name,async);
            Debug.Log("added " + scene.sceneName + " | " + scene.scene);
            sceneList.Add(scene);
        }
    }

    public void LoadScene(string sceneName)
    {
        var duplicate = currentSceneInLoad.Find(t => t == sceneName);   //check if they are already loaded
        if(duplicate == null)
        {
            var scene = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            var op = new LabeledScene(sceneName,scene);
            sceneList.Add(op);
            currentSceneInLoad.Add(sceneName);
        }
        //LabeledScene targetScene = sceneList.Find(t=>t.sceneName == sceneName);
        //targetScene.scene.allowSceneActivation = true;
    }
    public void UnloadScene(string sceneName)
    {
        currentSceneInLoad.Remove(sceneName);
        SceneManager.UnloadSceneAsync(sceneName);
        //var targetScene = sceneList.Find(t=>t.sceneName == sceneName);
        //targetScene.scene.allowSceneActivation = false;
    }
}

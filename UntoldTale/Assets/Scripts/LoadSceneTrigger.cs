using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attached to tunnel triggers
//load the next area scene when you're in the tunnel
public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] string[] sceneToLoad;
    [SerializeField] string[] sceneToUnload;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            SceneTransition();
    }    
    //when scenes are loaded: 
    /*
        - objects go back to original place
        - unload scenes
        - add bgm object to audiomanager
    */
    void SceneTransition()
    {
        foreach(var i in sceneToLoad) WorldManager.Instance.LoadScene(i);

        foreach(var i in sceneToUnload) WorldManager.Instance.UnloadScene(i);
    }

}

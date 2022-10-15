using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] string[] sceneToLoad;
    [SerializeField] string[] sceneToUnload;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            foreach(var i in sceneToLoad) GameManager.Instance.LoadScene(i);

            foreach(var i in sceneToUnload) GameManager.Instance.UnloadScene(i);
        }

    }    
}

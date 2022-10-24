using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBoundaryTrigger : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) levelManager.ActivateLevel.Invoke();
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))  levelManager.DeactivateLevel.Invoke();
    }
}

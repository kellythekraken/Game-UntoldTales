using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveZoneTrigger : MonoBehaviour
{
    [SerializeField] LevelManager levelManager;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) levelManager.playerEnterLeaveZoneEvent.Invoke();
        else if (collider.gameObject.CompareTag("Friend")) {collider.GetComponentInParent<AIFriendBehaviour>().myState = AISTATE.REST;} 
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))  levelManager.playerLeaveSceneEvent.Invoke();
    }
}

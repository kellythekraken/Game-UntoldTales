using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetection : MonoBehaviour
{
    [SerializeField] List<Befriendable> befriendList;
    public float befriendSpeed = 1f;

    void OnEnable() =>befriendList = new List<Befriendable>();
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("FriendToBe")) 
        {
            befriendList.Add(collider.GetComponentInParent<Befriendable>());
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(!collider.CompareTag("FriendToBe")) return;
        foreach(var i in befriendList) 
        {
            if(i.friendliness<100) i.friendliness += befriendSpeed * Time.deltaTime;
            else{
                i.befriended = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("FriendToBe")) befriendList.Remove(collider.GetComponentInParent<Befriendable>());
    }
}

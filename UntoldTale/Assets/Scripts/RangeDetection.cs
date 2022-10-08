using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetection : MonoBehaviour
{
    [SerializeField] List<Befriendable> befriendList;
    public float befriendSpeed = 1f;    //could be modified as you become better at it
    public float exhaustSpeed = 1f;      //will DOUBLE if you befriending multiple at once
    public int exhaustRate = 0;         //how many you're socializing at once
    public float rechargeSpeed = 1f;
    public float socialBattery = 100f;   //reduce when you're befriending blobs

    void OnEnable() =>befriendList = new List<Befriendable>();

    void Update()
    {
        if(exhaustRate>0) {
            if(socialBattery>0f) socialBattery -= exhaustSpeed * exhaustRate * Time.deltaTime;
        }
        else{ 
        if(socialBattery<100f) socialBattery += rechargeSpeed * Time.deltaTime;
        }

        if(socialBattery>0)
        {
            foreach(var i in befriendList) 
            {
                if(i.friendliness<100) i.friendliness += befriendSpeed * Time.deltaTime;
                else
                {
                    i.befriended = true;
                    befriendList.Remove(i);
                    exhaustRate --;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("FriendToBe")) 
        {
            befriendList.Add(collider.GetComponentInParent<Befriendable>());
            exhaustRate ++;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("FriendToBe")) 
        {
            befriendList.Remove(collider.GetComponentInParent<Befriendable>());
            exhaustRate --;
        }
    }
}

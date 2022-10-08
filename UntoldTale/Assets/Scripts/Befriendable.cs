using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Befriendable : MonoBehaviour
{
    private bool _befriended = false;
    public bool befriended {get {return _befriended;} set {_befriended = value; Befriend(value);}}
    public float friendliness = 0f; //goes up to 100%
    [SerializeField] Color trueColor;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Befriend(bool befriend)
    {
        if(!befriend) return;
        gameObject.tag = "Friend";
        sprite.color = trueColor;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Befriendable : MonoBehaviour
{
    public bool befriended {get {return _befriended;} set {_befriended = value; if(value) Befriend();}}
    public float friendliness = 0f; //goes up to 100%
    private bool _befriended = false;
    [SerializeField] Color trueColor;
    SpriteSkin spriteSkin;
    SpriteRenderer sprite;
    Collider2D myCollider;
    GameObject childObj;

    Color startColor, currentColor;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        spriteSkin = GetComponent<SpriteSkin>();
        myCollider = GetComponent<Collider2D>();
        childObj = transform.GetChild(0).gameObject;

        spriteSkin.enabled = befriended;
        startColor = sprite.color;
        childObj.SetActive(false);
    }


    void Befriend()
    {
        gameObject.tag = "Friend";
        sprite.color = trueColor;
        myCollider.enabled = false;
        spriteSkin.enabled = true;
        childObj.SetActive(true);
    }

    //called in the update method of rangedetection
    public void StartBefriending()
    {
        //sync up the friendliness to the color value
        var lerpValue = friendliness/100f;
        sprite.color = Color.Lerp(startColor,trueColor,lerpValue);
    }
}

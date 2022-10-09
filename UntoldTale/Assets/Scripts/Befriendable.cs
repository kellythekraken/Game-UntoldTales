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

    Color startColor, currentColor;
    public float lerpValue = 0f;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
    }


    void Befriend(bool befriend)
    {
        if(!befriend) return;
        gameObject.tag = "Friend";
        sprite.color = trueColor;
    }

    //called in the update method of rangedetection
    public void StartBefriending()
    {
        //sync up the friendliness to the color value
        sprite.color = CalculateColor();
    }

    Color CalculateColor()
    {
        lerpValue = friendliness/100f;
        return Color.Lerp(startColor,trueColor,lerpValue);
    }
/*
    IEnumerator ChangeColor(float startcolor)
    {
        for(float i = 0f; i>=friendliness; i-=0.05f)
        {
            Color newColor;
            trueColor.r * i;
        }
    }
    */
}

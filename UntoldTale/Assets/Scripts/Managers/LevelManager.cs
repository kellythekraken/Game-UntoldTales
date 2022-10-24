using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//attatched to each level parent
//check how familiar you have became
public class LevelManager : MonoBehaviour
{
    internal UnityEvent ActivateLevel = new UnityEvent();
    internal UnityEvent DeactivateLevel = new UnityEvent();
    internal UnityEvent playerLeaveSceneEvent = new UnityEvent();
    internal UnityEvent playerEnterLeaveZoneEvent =  new UnityEvent();
    [SerializeField] SpriteRenderer borderSprite;
    [SerializeField] Sprite borderUncovered;
    [SerializeField] string levelName;
    [SerializeField] FMODUnity.StudioEventEmitter sceneBGMEmitter;
    [SerializeField] Color levelTrueColor;
    SpriteRenderer levelBackground;
    [SerializeField] Material prevTunnelMaterial, nextTunnelMaterial;
    Transform blobParent;
    List<Befriendable> blobLists;
    public float blobTotalCount, friendCount;
    public float familiarMeter = 0f;   //if this goes above 0.5, you could proceed to the next level
    Color startColor;
    [SerializeField] GameObject doorToPreviousRoom, doorToNextRoom;
    
    void Start()
    {
        blobParent = transform.Find("Blobs");
        levelBackground = transform.Find("Color_BG").GetComponent<SpriteRenderer>();
        InitializeList();
        blobTotalCount = blobLists.Count;
        friendCount = 0f;
        startColor = levelBackground.color;
        doorToPreviousRoom.SetActive(false);
        
        AudioManager.Instance.AddToBGMEventList(levelName, sceneBGMEmitter);
        playerLeaveSceneEvent.AddListener(LeaveSceneEventAction);
        playerEnterLeaveZoneEvent.AddListener(InLeaveZoneEventAction);
    }
    void OnDisable() => AudioManager.Instance.RemoveSound(levelName);
    void InitializeList()
    {        
        blobLists = new List<Befriendable>();
        
        foreach(Transform i in blobParent)
        {
            var befriendable = i.GetComponent<Befriendable>();
            blobLists.Add(befriendable);
            befriendable.levelManager = this;
        }
    }

    void InLeaveZoneEventAction()
    {
        if(familiarMeter== 0) return;

        foreach(var i in blobLists)
        {
            if(!i.gameObject.activeSelf) continue;
            
            var ai = i.GetComponent<AIFriendBehaviour>();
            if(ai.enabled) ai.myState = AISTATE.FOLLOW;
        }
    }
    void LeaveSceneEventAction()    //player leave the scene
    {
        if(familiarMeter== 0) return;
        ResetBlobLocations();
    }
    void ResetBlobLocations()
    {
        foreach(var i in blobLists)
        {
            if(!i.gameObject.activeSelf) continue;

            var ai = i.GetComponent<AIFriendBehaviour>();
            if(ai.enabled) 
            {
                ai.myState = AISTATE.RETREAT;
            }
        }
    }


    //called when you befriend the blob, in Befriendable.cs
    public void IncreaseFamiliarity()
    {
        friendCount++;
        familiarMeter = friendCount / blobTotalCount;
        //gradually warm up the color
        StartCoroutine(BackgroundColorTransition());
        //AudioManager.Instance.SetGlobalParam("Familiarity", familiarMeter);
        //check if you could proceed to next level
        if(familiarMeter>=0.5f) 
        {
            Debug.Log("proceed to next level!");
            borderSprite.sprite = borderUncovered;
        }
    }

    IEnumerator BackgroundColorTransition()
    {
        float elapsedTime = 0;
        float waitTime = 1f;

        Color newColor = Color.Lerp(startColor,levelTrueColor,familiarMeter);
        Color currentColor = levelBackground.color;

        while (elapsedTime < waitTime)
        {
            var smoothColor = Color.Lerp(currentColor, newColor, (elapsedTime / waitTime));
            levelBackground.color = smoothColor;
            prevTunnelMaterial.SetColor("_Color2",smoothColor);
            nextTunnelMaterial.SetColor("_Color",smoothColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        } 
        //tunnelBackground.material.SetColor("_Color2",newColor);
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//attatched to each level parent
//check how familiar you have became
public class LevelManager : MonoBehaviour
{
    [SerializeField] string levelName;
    [SerializeField] FMODUnity.StudioEventEmitter sceneBGMEmitter;
    [SerializeField] Color levelTrueColor;
    SpriteRenderer levelBackground,tunnelBackground;
    Transform blobParent;
    List<Befriendable> blobLists;
    public float blobTotalCount, friendCount;
    public float familiarMeter = 0f;   //if this goes above 0.5, you could proceed to the next level
    Color startColor;
    [SerializeField] Collider2D doorToPreviousRoom, doorToNextRoom;
    
    void Start()
    {
        blobParent = transform.Find("Blobs");
        levelBackground = transform.Find("Background").GetComponent<SpriteRenderer>();
        tunnelBackground = transform.Find("Tunnel").GetComponent<SpriteRenderer>();
        InitializeList();
        blobTotalCount = blobLists.Count;
        friendCount = 0f;
        startColor = levelBackground.color;

        AudioManager.Instance.AddToBGMEventList(levelName, sceneBGMEmitter);
    }

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
    //called when you befriend the blob, in Befriendable.cs
    public void IncreaseFamiliarity()
    {
        friendCount++;
        familiarMeter = friendCount / blobTotalCount;
        //gradually warm up the color
        StartCoroutine(BackgroundColorTransition());
        
        //check if you could proceed to next level
        if(familiarMeter>=0.5f) Debug.Log("proceed to next level!");
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
            tunnelBackground.material.SetColor("_Color2",smoothColor);
            elapsedTime += Time.deltaTime;
            yield return null;
        } 
        //tunnelBackground.material.SetColor("_Color2",newColor);
        yield return null;
    }
}

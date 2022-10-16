using System.Collections;
using UnityEngine;
using FMODUnity;

//gradually fade _parameter ontrigger enter/exit 2D

public class TriggerAudioControl : MonoBehaviour
{
    public string _parameter;
    [SerializeField] [Range(0,1f)] float fadeFrom;
    [SerializeField] [Range(0,1f)] float fadeTo;

    Collider2D _collider;
    
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeParameter());
        }
    }

    IEnumerator FadeParameter()
    {
        float elapsedTime = 0f;
        while(elapsedTime < 1f)
        {
            var value = Mathf.Lerp(fadeFrom,fadeTo,elapsedTime);
            RuntimeManager.StudioSystem.setParameterByName(_parameter,value);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        RuntimeManager.StudioSystem.setParameterByName(_parameter,fadeTo);
    }
}

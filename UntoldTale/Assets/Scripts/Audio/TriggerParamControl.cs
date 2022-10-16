using UnityEngine;

//gradually fade _parameter ontrigger enter/exit 2D
//or control song switch
public class TriggerParamControl : MonoBehaviour
{
    [SerializeField] ParamScope paramScope;
    public string _eventName;
    public string _parameter;
    [SerializeField] [Range(0,1f)] float fadeFrom;
    [SerializeField] [Range(0,1f)] float fadeTo;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.SetParameter(_eventName, paramScope, _parameter, fadeFrom, fadeTo);
        }
    }

}

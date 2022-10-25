using UnityEngine;

public class LeaveSceneTrigger : MonoBehaviour
{
    LevelManager levelManager;
    //[SerializeField] GameObject levelSceneObject;

    void Start() => levelManager = GetComponentInParent<LevelManager>();

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) {levelManager.ActivateLevel.Invoke(); Debug.Log("set active");}
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) {levelManager.DeactivateLevel.Invoke(); Debug.Log("deactivate ");}
    }
}

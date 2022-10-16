using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBGMChange : MonoBehaviour
{
    [SerializeField] string TargetBGMName;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayBGM(TargetBGMName);
        }
    }
}

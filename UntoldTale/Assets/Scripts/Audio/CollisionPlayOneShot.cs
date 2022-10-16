using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionPlayOneShot : MonoBehaviour
{
    FMODUnity.StudioEventEmitter emitter;
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.CompareTag("Player")) 
        {
            emitter.Play();
        }
    }
}

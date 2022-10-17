using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    Transform wormi;
    float calculatedRadius;
    [SerializeField] float followSpeed;
    [SerializeField] float defaultForce = 500f;
    Rigidbody2D centerRb;
    List<Rigidbody2D> bones;

    bool boil = false;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        GameManager.Instance.GameStartEvent.AddListener(GameStarInit);

        bones = new List<Rigidbody2D>();
        foreach(Transform i in centerRb.transform)
        {
            bones.Add(i.GetComponent<Rigidbody2D>());
        }
    }

    IEnumerator FamilyOnGameStartBehaviour()
    {
        // slowly march towards wormi and hug tight
        var timeElapsed = 0f;
        while(timeElapsed < 1.5f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, 0.01f);
                centerRb.MovePosition(newPosition);
            }
            else{ 
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, 0.008f);
                centerRb.MovePosition(newPosition);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        // after a few seconds, move away slowly and leave her a trail
        timeElapsed = 0f;
        Vector2 direction = transform.position - wormi.transform.position;
        direction.Normalize();
        while(timeElapsed < 4f)
        {
            centerRb.AddForce(direction *5f * centerRb.mass);
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
        }
        //should only invoke repeat if player is in sight
        //InvokeRepeating("BoilAnimation",0f,.9f);
        StartCoroutine(BoilAnimation());

        //move towards the target position to leave wormi a trail

        //when the player is gone, reset position to start position (without repeating all this animation)
    }

    void GameStarInit()
    {
        StartCoroutine(FamilyOnGameStartBehaviour());
    }

    IEnumerator BoilAnimation()
    {
        //randomly move around each bone under center rb
        for(int c = 0; c< 999; c++)
        {
            foreach(var i in bones)
            {
                Vector2 direction = new Vector2((float)Random.Range(-50f, 50f), (float)Random.Range(-50f, 50f));
                i.AddForce(direction * 3f);
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(0.6f);
        }
    }
}

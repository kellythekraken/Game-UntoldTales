using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : MonoBehaviour
{
    Transform wormi;
    float calculatedRadius;
    [SerializeField] float followSpeed;
    [SerializeField] bool isFriend;
    Rigidbody2D centerRb;
    List<Rigidbody2D> bones;
    bool boil = false;
    Vector2 startPosition;

    void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        GameManager.Instance.GameStartEvent.AddListener(GameStarInit);
        startPosition = transform.position;

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
        var huggingSpeed = followSpeed *.6f;
        while(timeElapsed < 3f)
        {
            if(Vector2.Distance(centerRb.position,wormi.position) > calculatedRadius)
            {
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, followSpeed);
                centerRb.MovePosition(newPosition);
            }
            else{ 
                Vector2 newPosition = Vector2.Lerp(centerRb.position,wormi.position, huggingSpeed);
                centerRb.MovePosition(newPosition);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // move away slowly
        StartCoroutine(ResetPos());
        yield break;
        timeElapsed = 0f;
        Vector2 direction = transform.position - wormi.transform.position;
        float randNum = Random.Range(1f,6f);
        direction.Normalize();
        while(timeElapsed < 4f)
        {
            centerRb.AddForce(direction * randNum * centerRb.mass);
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

    IEnumerator ResetPos()
    {
        float elapsed = 0f;
        var startpos = centerRb.position;
        float randTime = Random.Range(0.7f,2.7f);

        while(elapsed< randTime)
        {
            var lerpVal = Vector2.Lerp(startpos, startPosition, elapsed/randTime);
            centerRb.MovePosition(lerpVal);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

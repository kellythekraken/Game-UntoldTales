using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFamilyBehaviour : BaseAI
{
    protected override void Start()
    {
        base.Start();
        calculatedRadius = transform.localScale.x * GetComponent<CircleCollider2D>().radius +0.5f;
        GameManager.Instance.GameStartEvent.AddListener(GameStarInit);
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
        StartCoroutine(BoilAnimation());
        yield break;
    }

    void GameStarInit()
    {
        StartCoroutine(FamilyOnGameStartBehaviour());
    }
}

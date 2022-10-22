using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    protected Transform wormi;
    protected float calculatedRadius;
    protected Rigidbody2D centerRb;
    public Vector3 startPosition;
    public float followSpeed = .3f;
    protected List<Rigidbody2D> bones;

    protected virtual void Start()
    {
        wormi = HeadMovement.Instance.transform;
        centerRb = GetComponentInChildren<Rigidbody2D>();
        startPosition = transform.position;
        
        bones = new List<Rigidbody2D>();
        foreach(Transform i in centerRb.transform)
        {
            bones.Add(i.GetComponent<Rigidbody2D>());
        }
    }

    protected IEnumerator ResetPos(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        var startpos = centerRb.position;
        while(elapsed< 2f)
        {
            var lerpVal = Vector2.Lerp(startpos, startPosition, elapsed/2f);
            centerRb.MovePosition(lerpVal);
            elapsed += Time.deltaTime;
            yield return null;
        }
        centerRb.MovePosition(startpos);
    }
    protected IEnumerator BoilAnimation()
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

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ResetPos(1f);
        }
    }
}

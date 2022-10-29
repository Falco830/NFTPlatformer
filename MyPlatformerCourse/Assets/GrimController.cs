using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimController : MonoBehaviour
{
  public Transform[] patrolPoints;
  public float speed;
  int currentPointIndex;

  float waitTime;
  public float startWaitTime;

  public bool asleep = true;
  public bool wakingUp = false;
  public bool awake = false;
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if (awake)
    {
      transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
      if (transform.position == patrolPoints[currentPointIndex].position)
      {
        if (waitTime <= 0)
        {
          if (currentPointIndex + 1 < patrolPoints.Length)
          {
            currentPointIndex++;
          }
          else
          {
            currentPointIndex = 0;
          }
          waitTime = startWaitTime;
        }
        else
        {
          waitTime -= Time.deltaTime;
        }
      }
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.tag.Equals("Player"))
    {

      FindObjectOfType<CameraShake>().ZoomOut();
      if (!asleep)
      {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
      }
      else if (!wakingUp)
      {
        //Color awakening = new Color(4, 55, 255, 255);
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;//awakening;
       
        wakingUp = true;
      }

    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.collider.tag.Equals("Player"))
    {
      gameObject.GetComponent<AudioSource>().Stop();
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Player")
    {
      collision.transform.parent = transform;
    }
  }

  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.tag == "Player")
    {
      collision.transform.parent = null;
    }
  }

}

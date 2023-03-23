using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GretasFist : MonoBehaviour
{
  public Transform[] patrolPoints;
  public float speed;
  int currentPointIndex;

  public GameObject Fist;
  public bool asleep;
  public bool onGreta;
  float waitTime;
  public float startWaitTime;
  IAstarAI ai;
  // Start is called before the first frame update
  void Start()
    {
    transform.position = patrolPoints[0].position;
    waitTime = startWaitTime;

    ai = GetComponent<IAstarAI>();
  }

    // Update is called once per frame
    void Update()
    {
      if(target == null)
      {
        Destroy(Fist);
      }
      if (asleep)
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
      else
      {
        if (onGreta){
          //punch Player
          this.gameObject.GetComponent<AIPath>().enabled = true;
          Debug.Log("Destination " + ai.destination);
        //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
        //ai.destination = FindObjectOfType<Player>().transform.position;
        ai.destination = FindObjectOfType<Player>().transform.position;
        if (ai.destination == Vector3.one)
        {
          //transform.position = Vector3.MoveTowards(transform.position, FindObjectOfType<Player>().transform.position, speed);
          //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
          ai.destination = FindObjectOfType<Player>().transform.position;
        }
      }
      else
        {
        //orbit Greta
        if(target != null)
        {
          Debug.Log("Orbit: " + target.transform.position);
          //GetComponent<AIPath>().enabled = false;
          //this.gameObject.GetComponent<AIPath>().destination = Vector3.one;
          //this.gameObject.GetComponent<AIDestinationSetter>().target = null;
          ai.destination = Vector3.one;
          transform.RotateAround(target.transform.position, Vector3.forward, OrbitSpeed * Time.deltaTime);
          //transform.position = Vector2.MoveTowards(transform.position, this.GetComponentInParent<Transform>().position + Mathf., speed * Time.deltaTime);
        }

      }
    }
  }
  public GameObject target;
  public float OrbitSpeed = 20f;
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

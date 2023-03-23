using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GongController : MonoBehaviour
{

  public Transform[] patrolPoints;
  public float speed;
  public int currentPointIndex;

  float waitTime;
  public float startWaitTime;

  public BattleBegins battleState;

  public bool asleep = true;
  public bool wakingUp = false;
  public bool awake = false;
  public CinemachineVirtualCamera mainCamera;


  public GameObject fireBall;
  public float timeBetweenShots;
  float nextShotTime;
  public Transform shotPoint;
  public Transform player;
  public Slider slider;
  public Transform target;

  public GameObject Fist;
  // Start is called before the first frame update
  void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.layer = 8;
    }

    // Update is called once per frame
    void Update()
    {

      if (awake)
      {
        //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-30, 30);
        gameObject.layer = 9;
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
        if ((Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position)) <= 5)
        {
          if (waitTime <= 0)
          {
            if (currentPointIndex < patrolPoints.Length - 1)
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
        if (Time.time > nextShotTime)
        {
          //shotPoint.transform.LookAt(target);
          Instantiate(fireBall, shotPoint.position, shotPoint.rotation);
          nextShotTime = Time.time + timeBetweenShots;
        }
      slider.value = ((float) this.GetComponent<Enemy>().health / 300f);
    }


    }


  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.collider.tag.Equals("Player"))
    {
     

      if (!asleep)
      {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        Fist.GetComponentInChildren<GretasFist>().onGreta = true;
      }
      else if (!wakingUp)
      {
        //Color awakening = new Color(4, 55, 255, 255);
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;//awakening;
        mainCamera.GetComponent<CameraShake>().ZoomOut();
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //asleep = false;
        wakingUp = true;
      }

    }
  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    if (collision.collider.tag.Equals("Player"))
    {
      Fist.GetComponentInChildren<GretasFist>().onGreta = false;
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
      Fist.GetComponentInChildren<GretasFist>().onGreta = false;
    }
  }

  private void OnDestroy()
  {
    battleState.DeadHeads();
  }

}
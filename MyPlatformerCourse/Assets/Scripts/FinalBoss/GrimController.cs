using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

  public BattleBegins battleState;
  public Slider slider;
  public GameObject fireBall;
  public float timeBetweenShots;
  float nextShotTime;
  public Transform shotPoint;
  public Transform player;

  // Start is called before the first frame update
  void Start()
    {
      //gameObject.GetComponent<Enemy>().enabled = false;
      gameObject.layer = 8;
    }

    // Update is called once per frame
    void Update()
    {
    if (awake)
    {
      //gameObject.GetComponent<Enemy>().enabled = true;
      gameObject.layer = 9;
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
      if (Time.time > nextShotTime)
      {
        //shotPoint.transform.LookAt(target);
        Instantiate(fireBall, shotPoint.position, shotPoint.rotation);
        nextShotTime = Time.time + timeBetweenShots;
      }
      slider.value = ((float)this.GetComponent<Enemy>().health / 300f);
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
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
       
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
  private void OnDestroy()
  {
    battleState.DeadHeads();
  }
}

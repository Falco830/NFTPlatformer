using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
public class Freezer : MonoBehaviour
{
  public int damage;
  public int health;

  public GameObject blood;
  public GameObject deathEffect;

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (!SceneManager.GetActiveScene().name.Equals("Level3Custom"))
      {
        if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Enemy>() != null)
        {
          Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);

            this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Enemy>().transform;

            smallestDistance = preyRadius;
            GetClosestInRadius(hitColliders, false);


        }
        else if(this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Prey>() != null)
        {
          Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);
          this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Prey>().transform;
          smallestDistance = preyRadius;
          GetClosestInRadius(hitColliders, true);

        }
        else if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Player>() != null)
        {

          this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
        }
      }
    }


  float smallestDistance = preyRadius;
  public static float preyRadius = 500;
  public void GetClosestInRadius(Collider2D[] hitColliders, bool preyOrEnemy)
  {
    foreach (var hitCollider in hitColliders)
    {
      if (preyOrEnemy)
      {
        if (FindObjectOfType<Prey>() ? hitCollider.GetComponent<Prey>().isActiveAndEnabled : false)
        {
          float distanceSqr = (this.gameObject.transform.position - hitCollider.transform.position).sqrMagnitude;
          if (smallestDistance > distanceSqr)
          {
            smallestDistance = distanceSqr;
            this.gameObject.GetComponent<AIDestinationSetter>().target = hitCollider.transform;
          }
        }
      }else
      {
        if (FindObjectOfType<Enemy>() ? hitCollider.GetComponent<Enemy>().isActiveAndEnabled : false)
        {
          float distanceSqr = (this.gameObject.transform.position - hitCollider.transform.position).sqrMagnitude;
          if (smallestDistance > distanceSqr)
          {
            smallestDistance = distanceSqr;
            this.gameObject.GetComponent<AIDestinationSetter>().target = hitCollider.transform;
          }
        }
      }
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Player")
    {
      collision.GetComponent<Player>().TakeDamage(damage);
    }
    if (collision.tag == "Prey")
    {
      collision.GetComponent<Prey>().TakeDamage(damage);
    }
    if(collision.gameObject.layer == 9)
    {
      collision.GetComponent<Enemy>().TakeDamage(damage);
    }
      
  }

  public void TakeDamage(int damage)
  {
    health -= damage;
    if (health <= 0)
    {
      //this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
      Instantiate(deathEffect, transform.position, Quaternion.identity);
      Destroy(gameObject);
    }
    else
    {
      Instantiate(blood, transform.position, Quaternion.identity);
    }
  }
}

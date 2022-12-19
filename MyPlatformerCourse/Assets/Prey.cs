using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Prey : MonoBehaviour
{
  public int damage;
  public int health;

  public GameObject blood;
  public GameObject deathEffect;
  public static float preyRadius = 30;
  public static float blitzRadius = 10;

  public int preyState = 0;

  Path p = null;
  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Enemy>() != null )
        {

          Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);
          Collider2D[] blitzColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, blitzRadius);

          if (blitzColliders.Length != 0 && preyState != 1)
          {
                GetBlitzRadius(blitzColliders);
                preyState = 1;
          }
              else if (hitColliders.Length != 0 && preyState != 2)
          {
            GetClosestInRadius(hitColliders);
            preyState = 2;
          }
          else if(preyState != 0)
          {
            wander();
            preyState = 0;
           }

        }
    }
  float smallestDistance = preyRadius;
  int maxCount = 0;

  void GetBlitzRadius(Collider2D[] hitColliders)
  {
    hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, blitzRadius);
    shouldFlee(hitColliders.ElementAt<Collider2D>(/*Random.Range(0,hitColliders.Length)*/hitColliders.Length-1).transform.position);
  }

  public void wander()
  {
    RandomPath rm = RandomPath.Construct(this.gameObject.transform.position,5000);
    rm.BlockUntilCalculated();
    Seeker seek = this.gameObject.GetComponent<Seeker>();
    Path p = seek.StartPath(rm);
    p.BlockUntilCalculated();
  }
  void GetClosestInRadius(Collider2D[] hitColliders)
  {
    hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);
    if (hitColliders.Count<Collider2D>() == 0)
    {
      smallestDistance = preyRadius;
    }
    if (hitColliders.Count<Collider2D>() > maxCount)
    {
      maxCount = hitColliders.Count<Collider2D>();
    }
    else if (hitColliders.Count<Collider2D>() < maxCount)
    {
      smallestDistance = preyRadius;
    }
    foreach (var hitCollider in hitColliders)
    {
      if (hitCollider.GetComponent<Enemy>())
      {
 
        float distanceSqr = Mathf.Sqrt((this.gameObject.transform.position - hitCollider.transform.position).sqrMagnitude);

        Debug.Log("ENEMY" + distanceSqr);
        if (smallestDistance > distanceSqr)
        {
          smallestDistance = distanceSqr;
          shouldFlee(hitCollider.transform.position);
        }
      }
    }
  }

  public void FleeToTarget(Path p)
  {
    p.BlockUntilCalculated();
    IAstarAI ai = GetComponent<IAstarAI>();
    if (ai != null) ai.destination = p.vectorPath.Last();
  }
public void shouldFlee(Vector3 hitCollider)
  {
 
    FleePath fp = FleePath.Construct(this.gameObject.transform.position, hitCollider, 5000);
    fp.aimStrength = 2;
    fp.spread = 6000;
    Seeker seek = this.gameObject.GetComponent<Seeker>();
    Path p = seek.StartPath(fp);
    p.BlockUntilCalculated();

  }

  public void multiUniversalFlee(Enemy[] enemies)
  {
    Vector3[] hitColliders = new Vector3[enemies.Length];
    int i = 0;
    foreach(Enemy en in enemies)
    {
      hitColliders[i++] = en.transform.position;
    }
    Seeker seek = this.gameObject.GetComponent<Seeker>();
    MultiTargetPath mt = seek.StartMultiTargetPath(this.gameObject.transform.position, hitColliders, false);
    mt.BlockUntilCalculated();
    Debug.Log("Target: " + mt.chosenTarget + " enemie " + enemies.Length);
    FleePath fp = FleePath.Construct(this.gameObject.transform.position, hitColliders[mt.chosenTarget], 3000);
    fp.aimStrength = 1;
    fp.spread = 4000;
    fp.aim = transform.position + transform.forward * 100;

    Path p = seek.StartPath(fp);
    p.BlockUntilCalculated();
  }

  public void TakeDamage(int damage)
  {
    health -= damage;
    Debug.Log("damage" + health);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
using System.Linq;


public class Enemy : MonoBehaviour
{
    public int damage;
    public int health;

    public GameObject blood;
    public GameObject deathEffect;

  public static float preyRadius = 30;
  public static float blitzRadius = 10;

  public int preyState = 1;


  private void Start()
  {
    ai = GetComponent<IAstarAI>();
  }
  private void Update()
  {
    if (!SceneManager.GetActiveScene().name.Equals("Level3Custom"))
    {
      if (FindObjectOfType<Freezer>() != null)
      {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);        
        Collider2D[] blitzColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, blitzRadius);

        
        int hitPreyIndexor = 0;
        foreach (Collider2D ht in hitColliders)
        {
          if (ht.TryGetComponent<Prey>(out var htp))
          {
            hitPreyIndexor++;
          }
        }
        Collider2D[] hitPreyColliders = new Collider2D[hitPreyIndexor];
        hitPreyIndexor = 0;
        foreach (Collider2D ht in hitColliders)
        {
          if (ht.TryGetComponent<Prey>(out var htp))
          {
            hitPreyColliders[hitPreyIndexor++] = ht;
          }
        }

        int hitBlitzIndexor = 0;
        foreach (Collider2D ht in blitzColliders)
        {
          if (ht.TryGetComponent<Prey>(out var htp))
          {
            hitBlitzIndexor++;
          }
        }
        Collider2D[] hitBlitzColliders = new Collider2D[hitBlitzIndexor];
        hitBlitzIndexor = 0;
        foreach (Collider2D ht in blitzColliders)
        {
          if (ht.TryGetComponent<Prey>(out var htp))
          {
            hitBlitzColliders[hitBlitzIndexor++] = ht;
          }
        }

        if (hitBlitzColliders.Length != 0 && ai.destination != null)
        {
          GetClosestInRadius(hitBlitzColliders);
          preyState = 1;
        }
        else if (hitPreyColliders.Length != 0 && ai.destination != null)
        {
          GetClosestInRadius(hitPreyColliders);
          preyState = 2;
        }
        else
        {
          if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Prey>() != null)
          {
            //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Prey>().transform;
            IAstarAI ai;
            ai = GetComponent<IAstarAI>();
            ai.destination = FindObjectOfType<Prey>().transform.position;
            smallestDistance = preyRadius;
            GetClosestInRadius();

          }
          else if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Player>() != null)
          {

            //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
            IAstarAI ai;
            ai = GetComponent<IAstarAI>();
            ai.destination = FindObjectOfType<Player>().transform.position;
          }
          else
          {
            wander();
            preyState = 0;
          }

        }

      }
      if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Prey>() != null)
      {
        //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Prey>().transform;
        IAstarAI ai;
        ai = GetComponent<IAstarAI>();
        ai.destination = FindObjectOfType<Prey>().transform.position;
        smallestDistance = preyRadius;
        GetClosestInRadius();

      }
      else if (this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Player>() != null)
      {

        //this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
        IAstarAI ai;
        ai = GetComponent<IAstarAI>();
        ai.destination = FindObjectOfType<Player>().transform.position;
        smallestDistance = preyRadius;
      }
      else
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
    shouldFlee(hitColliders.ElementAt<Collider2D>(/*Random.Range(0,hitColliders.Length)*/hitColliders.Length - 1).transform.position);
  }

  public void wander()
  {
    RandomPath rm = RandomPath.Construct(this.gameObject.transform.position, 5000);
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
      if (hitCollider.GetComponent<Freezer>())
      {

        float distanceSqr = Mathf.Sqrt((this.gameObject.transform.position - hitCollider.transform.position).sqrMagnitude);

        if (smallestDistance > distanceSqr)
        {
          smallestDistance = distanceSqr;
          shouldFlee(hitCollider.transform.position);
        }
      }
    }
  }
  IAstarAI ai = null;
  public void FleeToTarget(Path p)
  {
    p.BlockUntilCalculated();
    
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

  public void multiUniversalFlee(Freezer[] freezies)
  {
    Vector3[] hitColliders = new Vector3[freezies.Length];
    int i = 0;
    foreach (Freezer fz in freezies)
    {
      hitColliders[i++] = fz.transform.position;
    }
    Seeker seek = this.gameObject.GetComponent<Seeker>();
    MultiTargetPath mt = seek.StartMultiTargetPath(this.gameObject.transform.position, hitColliders, false);
    mt.BlockUntilCalculated();
    FleePath fp = FleePath.Construct(this.gameObject.transform.position, hitColliders[mt.chosenTarget], 3000);
    fp.aimStrength = 1;
    fp.spread = 4000;
    fp.aim = transform.position + transform.forward * 100;

    Path p = seek.StartPath(fp);
    p.BlockUntilCalculated();
  }

  public void GetClosestInRadius()
  {
    Collider2D[] hitColliders = Physics2D.OverlapCircleAll(this.gameObject.transform.position, preyRadius);
    foreach (var hitCollider in hitColliders)
    {
      if (hitCollider.GetComponent<Prey>())
      {

        float distanceSqr = (this.gameObject.transform.position - hitCollider.transform.position).sqrMagnitude;
        if (smallestDistance > distanceSqr)
        {
          smallestDistance = distanceSqr;
          IAstarAI ai;
          ai = GetComponent<IAstarAI>();
          ai.destination = hitCollider.transform.position;
          smallestDistance = preyRadius;
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
  }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else {
            Instantiate(blood, transform.position, Quaternion.identity);
        }
    }

}

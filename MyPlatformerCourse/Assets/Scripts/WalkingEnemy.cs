using Assets.HeroEditor.Common.Scripts.CharacterScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WalkingEnemy : Enemy
{

  Animator anim;
  AudioSource source;
  public Character Character;


  [SerializeField]
  Transform[] patrolPoints;
  [SerializeField]
  float speed;
  int currentPointIndex;
  float waitTime;
  [SerializeField]
  float startWaitTime;
  [SerializeField]
  int stopDistance;

  bool runningDirection = true;
  [SerializeField]
  float moveSpeed;

  [SerializeField]
  float agroRange;

  [SerializeField]
  float attackRange;

  [SerializeField]
  float timeBetweenAttacks;
  [SerializeField]
  float nextAttackTime;

  [SerializeField]
  float swingRange;
  [SerializeField]
  Transform attackPoint;
  [SerializeField]
  LayerMask playerLayer;
  [SerializeField]
  LayerMask enemyLayer;

  [SerializeField]
  int size;

  [SerializeField]
  Transform castPoint;

  [SerializeField]
  Rigidbody2D rb2d;

  public GameMaster gm;

  public AnimationEvents AnimationEvents;

  bool isSearching;
  bool isFacingLeft = true;

  public int shotsize = 120;
  public float positiony = 0;
  public float screeny = .69f;

  public int softsize = 60;
  //public float positiony = 0;
  //public float screeny = .69f;

  private bool isAgro = false;
  private bool isAttacking = false;

    // Start is called before the first frame update
  void Start()
    {
    //transform.position = patrolPoints[0].position;
    //transform.rotation = patrolPoints[0].rotation;
    waitTime = startWaitTime;

    anim = GetComponentInChildren<Animator>();
    AnimationEvents.OnCustomEvent += OnAnimationEvent;
    source = GetComponent<AudioSource>();

    gm = FindObjectOfType<GameMaster>();
    rb2d = GetComponent<Rigidbody2D>();

    }

      void OnDestroy()
      {
          AnimationEvents.OnCustomEvent -= OnAnimationEvent;
      }

    // Update is called once per frame
    void Update()
    {

      if (CanSeePlayer(agroRange, "Player"))
      {
        //agro enemy
        isAgro = true;
      //ChasePlayer();
        if (CanSeePlayer(attackRange, "Player"))
        {
          isAttacking = true;
        }
        else
        {
          isAttacking = false;
        }
    }
      else
      {
        if (isAgro)
        {
           if (!isSearching)
            {
              isSearching = true;
              Invoke("StopChasingPlayer", 5);
            }
          if (CanSeePlayer(attackRange, "Player"))
          {
            isAttacking = true;
          }
          else
          {
            isAttacking = false;
          }
        }
        else
        {
        Patrolling();
        }
        //StopChasingPlayer();
      }

      if (isAgro)
      {
        ChasePlayer();
        if (isAttacking)
        {
          if (Time.time > nextAttackTime)
          {
            anim.SetTrigger("Slash");
            nextAttackTime = Time.time + timeBetweenAttacks;
            //AttackPlayer();
          }
        }
      else 
        {
          StopAttacking();
        }
      }
    }


    void Patrolling()
    {
    //transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
    float distance = Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position);
    bool canSeeEnemy = CanSeePlayer(attackRange, "Enemy");
    if (distance < stopDistance)
    {
      Character.SetState(CharacterState.Idle);
      transform.rotation = patrolPoints[currentPointIndex].rotation;
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
        //runningDirection = !runningDirection;
        waitTime = startWaitTime;
      }
      else
      {
        waitTime -= Time.deltaTime;
      }

    }
    else
    {     
      if (canSeeEnemy)
      {
        transform.rotation = patrolPoints[currentPointIndex].rotation;

          if (currentPointIndex + 1 < patrolPoints.Length)
          {
            currentPointIndex++;
          }
          else
          {
            currentPointIndex = 0;
          }
      }
        if (transform.position.x < patrolPoints[currentPointIndex].position.x)
        {
          rb2d.velocity = new Vector2(speed, 0);
          transform.localScale = new Vector2(size, size);
          isFacingLeft = false;
        }
        else if (transform.position.x > patrolPoints[currentPointIndex].position.x)
        {
          rb2d.velocity = new Vector2(-speed, 0);
          transform.localScale = new Vector2(size, size);
          isFacingLeft = true;
        }


      Character.SetState(CharacterState.Walk);
    }
  }



    void ChasePlayer()
    {
      gm.ChangeShot(shotsize, positiony, screeny);
      if(transform.position.x < FindObjectOfType<Player>().transform.position.x)
      {
        transform.rotation = Quaternion.Euler(0,0,0);
        rb2d.velocity = new Vector2(moveSpeed, 0);
        transform.localScale = new Vector2(size, size);

        isFacingLeft = false;
      }
      else
      {
        transform.rotation = Quaternion.Euler(0,180,0);
        rb2d.velocity = new Vector2(-moveSpeed, 0);
        transform.localScale = new Vector2(size, size);
        isFacingLeft = true;
      }
    Character.SetState(CharacterState.Run);
  }

  private void OnAnimationEvent(string eventName)
  {
    switch (eventName)
    {
      case "Hit":
        AttackPlayer();
        break;
      default: return;
    }
  }
    public void AttackPlayer()
    {
      //FindObjectOfType<CameraShake>().Shake();
      //Character.Slash();
      //Instantiate(swordSwingEffect, attackPoint.position, Quaternion.identity);
      Collider2D[] playersToDamage = Physics2D.OverlapCircleAll(attackPoint.position, swingRange, playerLayer);
      foreach (Collider2D col in playersToDamage)
      {
        col.GetComponent<Player>().TakeDamage(damage);
      }
    }
    void StopAttacking()
    {
      Character.IsReady();
    }

    bool CanSeePlayer(float distance, String targetLayer)
    {
      bool val = false;
      float castDist = distance;

      if (isFacingLeft)
      {
        castDist = -distance;
      }
    RaycastHit2D hit;
      Vector2 endPos = castPoint.position + Vector3.right * castDist;
    string tagName;
    if (LayerMask.NameToLayer(targetLayer) == LayerMask.NameToLayer("Enemy"))
      {
       hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Enemy")); //1 << LayerMask.NameToLayer("Player")
        tagName = "Enemy";
    }
    else
    {
       hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player")); //1 << LayerMask.NameToLayer("Player")
        tagName = "Player";
    }
   
    if (hit.collider != null)
      {
        if (hit.collider.gameObject.CompareTag(tagName)) //CompareTag("Player")
        {
          val = true;
          //ChasePlayer();
        }
        else
        {
          val = false;
          //StopChasingPlayer();
        }
        Debug.DrawLine(castPoint.position, hit.point, Color.blue);
      }
      else
      {
        Debug.DrawLine(castPoint.position, endPos, Color.blue);
      }
        return val;
    }


    void StopChasingPlayer()
    {
      gm.ChangeShot(softsize, positiony, screeny);
      Character.SetState(CharacterState.Idle);
      isAgro = false;
      isSearching = false;
      rb2d.velocity = new Vector2(0, 0);
    }
}

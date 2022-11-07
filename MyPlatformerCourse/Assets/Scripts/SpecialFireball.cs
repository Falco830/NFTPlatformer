using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFireball : Fireball
{
  private Transform player;
  private Vector2 target;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    target = new Vector2(player.position.x, player.position.y);

    Destroy(gameObject, lifeTime);
    }

  private void Update()
  {
    transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    if (transform.position.x == target.x && transform.position.y == target.y)
    {
      Destroy(gameObject);
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag == "Player")
    {
      collision.GetComponent<Player>().TakeDamage(damage);
      Destroy(gameObject);
    }
   
  }
}

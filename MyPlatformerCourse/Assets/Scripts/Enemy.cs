﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public int damage;
    public int health;

    public GameObject blood;
    public GameObject deathEffect;

  private void Start()
  {
    
  }
  private void Update()
  {
    if(this.gameObject.GetComponent<AIDestinationSetter>()?.target == null)
    {
      this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(damage);
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

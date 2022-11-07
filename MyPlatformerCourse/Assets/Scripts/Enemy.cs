using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

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
    if (!SceneManager.GetActiveScene().name.Equals("Level3Custom"))
    {
      if(this.gameObject.GetComponent<AIDestinationSetter>()?.target == null && FindObjectOfType<Player>() != null)
      {

        this.gameObject.GetComponent<AIDestinationSetter>().target = FindObjectOfType<Player>().transform;
      }
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
        Debug.Log("damage" + health);
        if (health <= 0)
        {
            //this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else {
            Instantiate(blood, transform.position, Quaternion.identity);
        }
    }

}

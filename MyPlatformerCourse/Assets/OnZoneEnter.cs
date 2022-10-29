using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnZoneEnter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.tag.Equals("Player"))
    {
      gameObject.GetComponent<AudioSource>().Play();
    }
  }
  private void OnTriggerExit2D(Collider2D collision)
  {
    if (collision.tag.Equals("Player"))
    {
      gameObject.GetComponent<AudioSource>().Stop();
    }
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {

  }

  private void OnCollisionExit2D(Collision2D collision)
  {
    
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    Collider2D colliderPlatform;
    SpriteRenderer spriteRenderer;

  [SerializeField]
  string playerTag = "Player";
  [SerializeField]
  float disappearTime = 3;

  Animator myAnim;

  [SerializeField]
  bool canReset;
  [SerializeField]
  float resetTime = 3;


  // Start is called before the first frame update
  void Start()
    {
      myAnim = GetComponent<Animator>();
      myAnim.SetFloat("DisappearTime", 1 / disappearTime);

      if(colliderPlatform == null)
      {
      colliderPlatform = this.gameObject.GetComponent<Collider2D>();
      }
      if (spriteRenderer == null)
      {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
      }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
      if(collision.transform.tag == playerTag)
      {
        myAnim.SetBool("Trigger", true);
      }
    }

    public void TriggerReset()
    {
      if (canReset)
      {
        StartCoroutine(Reset());
      }
    }

    IEnumerator Reset()
    {
      yield return new WaitForSeconds(resetTime);
      myAnim.SetBool("Trigger", false);
    }



}
